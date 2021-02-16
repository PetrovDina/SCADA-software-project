using RealTimeUnit.ServiceReference1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealTimeUnit
{
    class RealTimeUnitMain
    {
        static CspParameters csp = new CspParameters();
        static RSACryptoServiceProvider rsa = null;
        const string EXPORT_FOLDER = @"C:\SNUS_PROJECT\";

        static void Main(string[] args)
        {

            Console.WriteLine("Input address: ");
            string address = Console.ReadLine();
            int addressInt = -1;
            bool success = int.TryParse(address, out addressInt);

            if (!success)
            {
                Console.WriteLine("Invalid address number! Exiting.." );
                return;
            }

            Console.WriteLine("Enter value low and high limit: ");
            double lowLimit, highLimit;
            bool successLow = double.TryParse(Console.ReadLine(), out lowLimit);
            bool successHigh = double.TryParse(Console.ReadLine(), out highLimit);

            if (!successLow || !successHigh)
            {
                Console.WriteLine("Invalid limits!");
                return;
            }


            string path = Path.Combine(EXPORT_FOLDER, "key" + address + ".txt");

            RealTimeClient proxy = new RealTimeClient();
            if (!proxy.register(addressInt, path))
            {
                Console.WriteLine("Another RTU already registered with that address");
                Thread.Sleep(5000);
                return;
                    
            }

            rsa = new RSACryptoServiceProvider(csp);
            if (!Directory.Exists(EXPORT_FOLDER))
            {
                Directory.CreateDirectory(EXPORT_FOLDER);
            }


            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(rsa.ToXmlString(false));
            }


            Random rand = new Random();
            double value;
            while (true)
            {

                value = rand.NextDouble() * (highLimit - lowLimit) + lowLimit;
                Console.WriteLine("SENDING VALUE " + value + " TO ADDRESS " + address);
                byte[] signature = createSignature(value);
                proxy.sendValue(addressInt, value, signature);
                Thread.Sleep(3000);
            }

            

        }

        private static byte[] createSignature(double value)
        {
            byte[] hash = null;

            using (SHA256 sha = SHA256Managed.Create())
            {
                hash = sha.ComputeHash(Encoding.UTF8.GetBytes(value.ToString()));
                RSAPKCS1SignatureFormatter f = new RSAPKCS1SignatureFormatter(rsa);
                f.SetHashAlgorithm("SHA256");
                return f.CreateSignature(hash);

            }
        }
    }
}

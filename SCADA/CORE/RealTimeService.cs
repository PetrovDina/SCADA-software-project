using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CORE
{
    public class RealTimeService : IRealTime

    {
        static CspParameters csp = new CspParameters();
        static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);

        static Dictionary<string, string> keys = new Dictionary<string, string>();
        static Dictionary<string, double> realTimeValues = new Dictionary<string, double>();

        static object locker = new object();


        public bool register(string address, string publicKeyPath)
        {
            lock (keys)
            {
                if (keys.ContainsKey(address))
                {
                    Console.WriteLine("Already a real time unit registered to address " + address);
                    return false;
                }

                keys[address] = publicKeyPath;

                return true;
            }

        }


        public void sendValue(string address, double value, byte[] signature)
        {
            lock (locker)
            {
                byte[] hash = null;

                using (SHA256 sha = SHA256Managed.Create())
                {
                    hash = sha.ComputeHash(Encoding.UTF8.GetBytes(value.ToString()));
                }

                using (var sr = new StreamReader(keys[address]))
                {
                    rsa.FromXmlString(sr.ReadToEnd());
                }


                RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(rsa);
                deformatter.SetHashAlgorithm("SHA256");

                if (!deformatter.VerifySignature(hash, signature))
                {
                    Console.WriteLine("NEUSPESNA VERIFIKACIJA POTPISA!");
                    return;
                }

                //if we're here that means it was successful
                realTimeValues[address] = value;
            }
            

        }

        public static double? getValueByAddress(string address)
        {
            lock (locker)
                return realTimeValues.ContainsKey(address) ? realTimeValues[address] : (double?)null; 

        }
    }
}

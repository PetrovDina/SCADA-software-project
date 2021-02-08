using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CORE
{
    class COREMain
    {
        static void Main(string[] args)
        {
            ServiceHost svc = new ServiceHost(typeof(DatabaseManagerService));
            svc.Open();
            Console.WriteLine("Welcome to SCADA");
            Console.ReadKey();
            svc.Close();
        }

    }
}

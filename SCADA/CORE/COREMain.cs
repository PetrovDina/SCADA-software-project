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
            ServiceHost svc2 = new ServiceHost(typeof(TrendingService));
            ServiceHost svc3 = new ServiceHost(typeof(AlarmDisplayService));
            ServiceHost svc4 = new ServiceHost(typeof(ReportManagerService));

            svc.Open();
            svc2.Open();
            svc3.Open();
            svc4.Open();
            Console.WriteLine("Welcome to SCADA");

            //using (var db = new TagValueContext())
            //{
            //    var v = (from entry in db.TagValueEntries
            //            where entry.InputTagId == "6"
            //            select entry).OrderBy(x => x.DateTime).ToList();

            //    v.ForEach(x => Console.WriteLine(x));
            //}


            Console.ReadKey();
            svc.Close();
            svc2.Close();
            svc3.Close();
            svc4.Close();
        }

    }
}

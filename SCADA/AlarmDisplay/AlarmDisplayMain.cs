using AlarmDisplay.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    class AlarmDisplayMain
    {
        static AlarmClient proxy;

        static void Main(string[] args)
        {
            proxy = new AlarmClient(new InstanceContext(new AlarmCallback()));
            proxy.alarmInit();
            Console.WriteLine("Successfully subscribed");
            Console.WriteLine("-----------------------");
            Console.ReadLine();

        }
    }
}

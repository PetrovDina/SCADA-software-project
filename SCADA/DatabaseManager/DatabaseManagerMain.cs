using DatabaseManager.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DatabaseManager
{
    class DatabaseManagerMain
    {
        static void Main(string[] args)
        {
            DatabaseManagerClient proxy = new DatabaseManagerClient();
            DigitalOutput digo = new DigitalOutput
            {
                Id = "1",
                Description = "Some desc",
                DriverType = DriverType.SIMULATION,
                IOAddress = "0",
                InitialValue = 100
                 
            };


            proxy.addTag(digo);
            proxy.showOutputTagValues();
            proxy.setOutputTagValue(digo.Id, 666);
            proxy.showOutputTagValues();
            proxy.removeTag(digo.Id);
            Console.ReadKey();

        }
    }
}

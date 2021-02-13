using ReportManager.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace ReportManager
{
    class ReportManagerMain
    {
        static ReportManagerClient proxy;

        static void Main(string[] args)
        {
            proxy = new ReportManagerClient();

            int option = -1;

            while (option != 0)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("1. Show alarms by time interval");
                Console.WriteLine("2. Show alarms by priority");
                Console.WriteLine("3. Show tag values by time interval");
                Console.WriteLine("4. Show analog input values");
                Console.WriteLine("5. Show digital input values");
                Console.WriteLine("6. Get tag values by tag id");

                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Pick a number: ");
                bool success = int.TryParse(Console.ReadLine(), out option);

                if (success)
                {
                    switch (option)
                    {
                        case 1:
                            alarmsByTimeStamps();
                            break;

                        case 2:
                            alarmsByPriority();
                            break;

                        case 3:
                            tagValuesByTimeStamps();
                            break;

                        case 4:
                            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
                            proxy.getAnalogInputValues().ToList().ForEach(x => Console.WriteLine(x));
                            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
                            break;

                        case 5:
                            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
                            proxy.getDigitalInputValues().ToList().ForEach(x => Console.WriteLine(x));
                            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
                            break;

                        case 6:
                            tagValuesById();
                            break;
                            

                    }
                }
            }

            Console.ReadKey();
        }

        private static void alarmsByPriority()
        {
            int temp;
            Console.WriteLine("Enter 1, 2 or 3");
            bool success = int.TryParse(Console.ReadLine(), out temp);
            if (!success || (temp != 1 && temp != 2 && temp != 3))
            {
                Console.WriteLine("Invalid number for priority!");
                return;
            }

            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            proxy.getAlarmsByPriority((AlarmPriority)temp).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");



        }

        private static void alarmsByTimeStamps()
        {
            
            DateTime start = getTimeStamp("start time");
            DateTime end = getTimeStamp("end time");

            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            proxy.getAlarmsByTimestamps(start, end).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");



        }

        private static void tagValuesByTimeStamps()
        {

            DateTime start = getTimeStamp("start time");
            DateTime end = getTimeStamp("end time");

            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            proxy.getTagValuesByTimestamps(start, end).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");



        }

        private static DateTime getTimeStamp(string v)
        {
            Console.WriteLine("Enter " + v);
            DateTime time;
            bool success = DateTime.TryParse(Console.ReadLine(), out time);
            return time;
            }



        private static void tagValuesById()
        {
            Console.WriteLine("Enter id: ");
            string inputId = Console.ReadLine();
            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            proxy.getValuesById(inputId).ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
        }
    }
}

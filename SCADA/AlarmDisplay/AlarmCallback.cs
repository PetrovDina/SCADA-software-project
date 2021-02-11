using System;
using AlarmDisplay.ServiceReference1;
using Model;

namespace AlarmDisplay
{
    internal class AlarmCallback : IAlarmCallback
    {
        public AlarmCallback()
        {
        }

        public void onAlarmActivated(Alarm a, double value, DateTime time)
        {
            int priority = (int)a.Priority;

            for (int i = 0; i < priority; ++i)
            {
                Console.WriteLine(a);
                Console.WriteLine("Value read: " + value);
                Console.WriteLine("At: " + time);
            }

            Console.WriteLine("-------------------------------");
        }
    }
}
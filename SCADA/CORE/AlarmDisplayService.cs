using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CORE
{
    class AlarmDisplayService : IAlarm
    {
        static IAlarmCallback proxy = null; 

        public void alarmInit()
        {
            proxy = OperationContext.Current.GetCallbackChannel<IAlarmCallback>();

            TagProcessing.onAlarmActivated += proxy.onAlarmActivated;

            Console.WriteLine("New alarm display app initialised!");
        }
    }
}

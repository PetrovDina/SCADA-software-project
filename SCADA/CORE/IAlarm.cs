using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CORE
{
    [ServiceContract(CallbackContract = typeof(IAlarmCallback))]
    public interface IAlarm
    {
        [OperationContract]
        void alarmInit();
    }
}

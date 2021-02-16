using Model;
using System;
using System.ServiceModel;

namespace CORE
{
    public interface IAlarmCallback
    {
        [OperationContract]
        void onAlarmActivated(Alarm a, double value, DateTime time);
    }
}
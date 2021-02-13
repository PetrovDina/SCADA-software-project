using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CORE
{
    [ServiceContract]
    public interface IReportManager
    {
        [OperationContract]
        List<AlarmEntry> getAlarmsByTimestamps(DateTime start, DateTime end);

        [OperationContract]
        List<AlarmEntry> getAlarmsByPriority(AlarmPriority priority);

        [OperationContract]
        List<TagValueEntry> getTagValuesByTimestamps(DateTime start, DateTime end);

        [OperationContract]
        List<TagValueEntry> getAnalogInputValues();

        [OperationContract]
        List<TagValueEntry> getDigitalInputValues();

        [OperationContract]
        List<TagValueEntry> getValuesById(string id);

    }
}

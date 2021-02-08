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
    public interface IDatabaseManager
    {

        [OperationContract]
        bool addTag(Tag t);

        [OperationContract]
        bool removeTag(string id);

        [OperationContract]
        bool setTagScan(string id, bool scan);

        [OperationContract]
        bool setOutputTagValue(string id, double value);

        [OperationContract]
        void showOutputTagValues();


    }
}

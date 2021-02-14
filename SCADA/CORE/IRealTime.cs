using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CORE
{
    [ServiceContract]
    public interface IRealTime
    {
        [OperationContract]
        void sendValue(string address, double value, byte[] signature);

        [OperationContract]
        bool register(string address, string publicKeyPath);

    }
}

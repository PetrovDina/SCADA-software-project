using Model;
using System;
using System.ServiceModel;

namespace CORE
{
    public interface ITrendingCallback
    {
        [OperationContract]
        void onValueRead(Tag t, double value, DateTime time);
    }
}
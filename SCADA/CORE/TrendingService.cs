using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.ServiceModel;

namespace CORE
{
    public class TrendingService : ITrending
    {

        static ITrendingCallback proxy = null; //todo maybe put in trendingInit method

        public void trendingInit()
        {
            proxy = OperationContext.Current.GetCallbackChannel<ITrendingCallback>();

            TagProcessing.onValueRead += proxy.onValueRead;

            Console.WriteLine("New trending app initialised!");
        }
    }
}

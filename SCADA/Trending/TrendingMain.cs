using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Trending.ServiceReference1;
using System.ServiceModel;

namespace Trending
{
    class TrendingMain
    {
        static TrendingClient proxy;

        static void Main(string[] args)
        {
            proxy = new TrendingClient(new InstanceContext(new TrendingCallback()));
            proxy.trendingInit();
            Console.WriteLine("Successfully subscribed");
            Console.ReadLine();

        }
    }
}

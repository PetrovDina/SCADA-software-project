using System;
using Model;
using Trending.ServiceReference1;

namespace Trending
{
    public class TrendingCallback : ITrendingCallback
    {
        public TrendingCallback()
        {
        }

        public void onValueRead(Tag t, double value, DateTime time)
        {
            Console.WriteLine(t);
            Console.WriteLine("Read value " + value + " at " + time);
            Console.WriteLine("-------------------------------");
        }
    }
}
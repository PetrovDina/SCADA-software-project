using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE
{
    public static class SimulationDriver
    {
        public static double ReturnValue(string address)
        {

            if (address == "0") return Sine();
            else if (address == "1") return Cosine();
            else if (address == "2") return Ramp();
            else return -1000;
        }

        private static double Sine()
        {
            return 10 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
        }

        private static double Cosine()
        {
            return 10 * Math.Cos((double)DateTime.Now.Second / 60 * Math.PI);
        }

        private static double Ramp()
        {
            return 10 * DateTime.Now.Second / 60;
        }
    }
}

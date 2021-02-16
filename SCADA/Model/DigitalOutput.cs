using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class DigitalOutput : OutputTag
    {
        //public DigitalOutput(string id, string desc, string addr, DriverType dt, double initial) : base(id, desc, addr, dt, initial)
        //{
        //}

        public override string ToString()
        {
            return "Digital output: " + base.ToString();
        }
    }
}

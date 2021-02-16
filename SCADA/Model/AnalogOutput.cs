using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class AnalogOutput : OutputTag
    {
        [DataMember]
        public double LowLimit { get; set; }

        [DataMember]
        public double HighLimit { get; set; }

        //public AnalogOutput(string id, string desc, string addr, DriverType dt, double initial, double ll, double hl) : base(id, desc, addr, dt, initial)
        //{
        //    this.LowLimit = ll;
        //    this.HighLimit = hl;
        //}

        public override string ToString()
        {
            return "Analog output: " + base.ToString() + $" LowLimit: {LowLimit}, HighLimit: {HighLimit}";
        }
    }
}

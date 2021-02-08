using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class AnalogInput : InputTag
    {
        [DataMember]
        public double LowLimit { get; set; }

        [DataMember]
        public double HighLimit { get; set; }

        //public AnalogInput(string id, string desc, string addr, DriverType dt, int scanTime, bool scanOn, double ll, double hl) : base(id, desc, addr, dt, scanTime, scanOn)
        //{
        //    this.LowLimit = ll;
        //    this.HighLimit = hl;
        //}

        public override string ToString()
        {
            return "Analog input: " + base.ToString() + $" LowLimit: {LowLimit}, HighLimit: {HighLimit}";
        }
    }
}

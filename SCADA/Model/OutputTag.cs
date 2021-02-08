using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    [KnownType(typeof(DigitalOutput))]
    [KnownType(typeof(AnalogOutput))]

    public abstract class OutputTag : Tag
    {
        [DataMember]
        public double InitialValue { get; set; }

        //public OutputTag(string id, string desc, string addr, DriverType dt, double initial) : base(id, desc, addr, dt)
        //{
        //    this.InitialValue = initial;
        //}

        public override string ToString()
        {
            return base.ToString() + $" InitialValue: {InitialValue}";
        }


    }
}

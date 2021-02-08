using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class DigitalInput : InputTag
    {
        //todo add alarms
        //public DigitalInput(string id, string desc, string addr, DriverType dt, int scanTime, bool scanOn) : base(id, desc, addr, dt, scanTime, scanOn)
        //{
        //}

        public override string ToString()
        {
            return "Digital input: " + base.ToString();
        }
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    [KnownType(typeof(DigitalInput))]
    [KnownType(typeof(AnalogInput))]
    public abstract class InputTag : Tag
    {
        [DataMember]
        public int ScanTime { get; set; }

        [DataMember]
        public bool ScanOn { get; set; }
        //todo add alarm list

        //public InputTag(string id, string desc, string addr, DriverType dt, int scanTime, bool scanOn) : base(id, desc, addr, dt)
        //{
        //    this.ScanTime = scanTime;
        //    this.ScanOn = scanOn;
        //    //todo add alarms 
        //}

        public override string ToString()
        {
            return base.ToString() + $"ScanTime: {ScanTime}, ScanOn: {ScanOn}";
        }
    }
}

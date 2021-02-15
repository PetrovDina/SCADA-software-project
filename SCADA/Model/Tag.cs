using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Model { 

    [DataContract]
    [KnownType(typeof(DigitalInput))]
    [KnownType(typeof(DigitalOutput))]
    [KnownType(typeof(AnalogInput))]
    [KnownType(typeof(AnalogOutput))]
    public abstract class Tag
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string IOAddress { get; set; }

        //[DataMember]
        //public DriverType DriverType { get; set; }

        //public Tag(string id, string desc, string addr, DriverType dt)
        //{
        //    this.Id = id;
        //    this.Description = desc;
        //    this.IOAddress = addr;
        //    this.DriverType = dt;
        //}

        public override string ToString()
        {
            return $"Id: {Id}, Desc: {Description}, IOAddress: {IOAddress} ";
        }

    }
}

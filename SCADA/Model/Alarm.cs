using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class Alarm
    {
        [DataMember]
        public string Id { get; set; } //todo figure out if you need this

        [DataMember]
        public string TagId { get; set; }

        [DataMember]
        public AlarmType AlarmType { get; set; }

        [DataMember]
        public double Limit { get; set; }

        [DataMember]
        public AlarmPriority Priority { get; set; }


        public override string ToString()
        {
            return $"Alarm - Id: {Id}, TagId: {TagId}, AlarmType: {AlarmType}, Limit: {Limit}, Priority: {Priority}";

        }
    }
}

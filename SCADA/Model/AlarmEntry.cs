using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class AlarmEntry
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public double Value { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }
        
        [DataMember]
        public AlarmPriority Priority { get; set; }

        [DataMember]
        public string InputTagId { get; set; }

        [DataMember]
        public string AlarmId { get; set; }

       

        public override string ToString()
        {
            return $"Alarm entry = id: {Id}, Priority: {Priority}, InputTag: {InputTagId}, AlarmId: {AlarmId}, Time: {DateTime}, Value: {Value}";
        }
    }

    
}

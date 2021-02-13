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

        public string InputTagId { get; set; }

        public string AlarmId { get; set; }

        public override string ToString()
        {
            return $"Alarm entry = id: {Id}, Value: {Value}, Time: {DateTime}, InputTag: {InputTagId}, AlarmId : {AlarmId}";
        }
    }

    
}

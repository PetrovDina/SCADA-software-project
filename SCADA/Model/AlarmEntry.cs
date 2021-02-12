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

        //[ForeignKey("InputTag")]
        public string InputTagId { get; set; }
        //public virtual InputTag InputTag { get; set; }

        //[ForeignKey("Alarm")]
        public string AlarmId { get; set; }
        //public virtual Alarm Alarm { get; set; }
    }
}

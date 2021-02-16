using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [DataContract]
    public class TagValueEntry
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public double Value { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string TagId { get; set; }

        public override string ToString()
        {
            return $"Tag Value entry = id: {Id}, TagId: {TagId}, Time: {DateTime}, Value: {Value}";
        }
    }
}

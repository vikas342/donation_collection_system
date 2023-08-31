using System;
using System.Collections.Generic;

namespace Exam.Models
{
    public partial class Object
    {
        public Object()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string? Object1 { get; set; }
        public int? TypeId { get; set; }

        public virtual ObjectType? Type { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}

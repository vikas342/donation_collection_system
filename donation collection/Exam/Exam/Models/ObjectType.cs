using System;
using System.Collections.Generic;

namespace Exam.Models
{
    public partial class ObjectType
    {
        public ObjectType()
        {
            Objects = new HashSet<Object>();
        }

        public int Id { get; set; }
        public string? Type { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<Object> Objects { get; set; }
    }
}

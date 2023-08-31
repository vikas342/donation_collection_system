using System;
using System.Collections.Generic;

namespace Exam.Models
{
    public partial class Address
    {
        public int Aid { get; set; }
        public int? Uid { get; set; }
      

        public virtual User? UidNavigation { get; set; }
    }
}

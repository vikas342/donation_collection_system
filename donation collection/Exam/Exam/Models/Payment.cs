using System;
using System.Collections.Generic;

namespace Exam.Models
{
    public partial class Payment
    {
        public int Pid { get; set; }
        public int? Uid { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? Amount { get; set; }
        public int? Status { get; set; }

        public virtual Object? StatusNavigation { get; set; }
        public virtual User? UidNavigation { get; set; }
    }
}

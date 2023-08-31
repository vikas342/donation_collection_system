using System;
using System.Collections.Generic;

namespace Exam.Models
{
    public partial class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string? Uid { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Photo { get; set; }
        public string? Email { get; set; }
        public int? Role { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public DateTime? Createddate { get; set; } = DateTime.Now;

        public string? Flatno { get; set; }
        public string? Area { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }

        public virtual Role? RoleNavigation { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}

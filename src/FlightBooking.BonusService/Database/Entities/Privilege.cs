using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.BonusService.Database.Entities
{
    [Table("privilege")]
    [Index("Username", Name = "privilege_username_key", IsUnique = true)]
    public partial class Privilege
    {
        public Privilege()
        {
            PrivilegeHistories = new HashSet<PrivilegeHistory>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        [StringLength(80)]
        public string Username { get; set; } = null!;
        [Column("status")]
        [StringLength(80)]
        public string Status { get; set; } = null!;
        [Column("balance")]
        public int Balance { get; set; }

        [InverseProperty("Privilege")]
        public virtual ICollection<PrivilegeHistory> PrivilegeHistories { get; set; }
    }
}

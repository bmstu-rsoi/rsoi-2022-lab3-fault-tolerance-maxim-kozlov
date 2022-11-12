using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.BonusService.Database.Entities
{
    [Table("privilege_history")]
    public partial class PrivilegeHistory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("privilege_id")]
        public int? PrivilegeId { get; set; }
        [Column("ticket_uid")]
        public Guid TicketUid { get; set; }
        [Column("datetime", TypeName = "timestamp without time zone")]
        public DateTime Datetime { get; set; }
        [Column("balance_diff")]
        public int BalanceDiff { get; set; }
        [Column("operation_type")]
        [StringLength(20)]
        public string OperationType { get; set; } = null!;

        [ForeignKey("PrivilegeId")]
        [InverseProperty("PrivilegeHistories")]
        public virtual Privilege? Privilege { get; set; }
    }
}

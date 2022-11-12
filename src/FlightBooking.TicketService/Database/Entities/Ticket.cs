using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.TicketService.Database.Entities
{
    [Table("ticket")]
    [Index("TicketUid", Name = "ticket_ticket_uid_key", IsUnique = true)]
    public partial class Ticket
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("ticket_uid")]
        public Guid TicketUid { get; set; }
        [Column("username")]
        [StringLength(80)]
        public string Username { get; set; } = null!;
        [Column("flight_number")]
        [StringLength(20)]
        public string FlightNumber { get; set; } = null!;
        [Column("price")]
        public int Price { get; set; }
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = null!;
    }
}

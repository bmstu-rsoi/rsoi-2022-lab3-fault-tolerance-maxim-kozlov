using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.FlightService.Database.Entities
{
    [Table("flight")]
    public partial class Flight
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("flight_number")]
        [StringLength(20)]
        public string FlightNumber { get; set; } = null!;
        [Column("datetime")]
        public DateTime Datetime { get; set; }
        [Column("from_airport_id")]
        public int? FromAirportId { get; set; }
        [Column("to_airport_id")]
        public int? ToAirportId { get; set; }
        [Column("price")]
        public int Price { get; set; }

        [ForeignKey("FromAirportId")]
        [InverseProperty("FlightFromAirports")]
        public virtual Airport? FromAirport { get; set; }
        [ForeignKey("ToAirportId")]
        [InverseProperty("FlightToAirports")]
        public virtual Airport? ToAirport { get; set; }
    }
}

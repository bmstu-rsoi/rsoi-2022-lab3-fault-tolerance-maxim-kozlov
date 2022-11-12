using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking.FlightService.Database.Entities
{
    [Table("airport")]
    public partial class Airport
    {
        public Airport()
        {
            FlightFromAirports = new HashSet<Flight>();
            FlightToAirports = new HashSet<Flight>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(255)]
        public string? Name { get; set; }
        [Column("city")]
        [StringLength(255)]
        public string? City { get; set; }
        [Column("country")]
        [StringLength(255)]
        public string? Country { get; set; }

        [InverseProperty("FromAirport")]
        public virtual ICollection<Flight> FlightFromAirports { get; set; }
        [InverseProperty("ToAirport")]
        public virtual ICollection<Flight> FlightToAirports { get; set; }
    }
}

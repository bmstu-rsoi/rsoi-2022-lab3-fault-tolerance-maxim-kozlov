using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FlightBooking.TicketService.Database.Entities;

namespace FlightBooking.TicketService.Database
{
    public class TicketsContext : DbContext
    {
        public TicketsContext()
        {
        }

        public TicketsContext(DbContextOptions<TicketsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
    }
}

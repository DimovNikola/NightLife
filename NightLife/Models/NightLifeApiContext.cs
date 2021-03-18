using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightLife.Models
{
    public class NightLifeApiContext : DbContext
    {
        public NightLifeApiContext(DbContextOptions<NightLifeApiContext> options) : base(options)
        {   

        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}

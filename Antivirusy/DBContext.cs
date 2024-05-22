using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Antivirusy
{
    

    internal class DBContext : DbContext
    {
        public DbSet<Signatura> Signaturas { get; set; }
        public DbSet<Bytee> Bytees { get; set; }

        public DBContext() 
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Bytee>().HasData(new List<Bytee>
            {
                new Bytee()
                {
                    Id = 1,
                    Data = (byte)76,
                    SignaturaId = 1,
                },
                new Bytee()
                {
                    Id = 2,
                    Data = (byte)0,
                    SignaturaId = 1,
                },
                new Bytee()
                {
                    Id = 3,
                    Data = (byte)0,
                    SignaturaId = 1,
                },
                new Bytee()
                {
                    Id = 4,
                    Data = (byte)0,
                    SignaturaId = 1,
                },

                new Bytee()
                {
                    Id = 5,
                    Data = (byte)144,
                    SignaturaId = 2,
                },
                new Bytee()
                {
                    Id = 6,
                    Data = (byte)0,
                    SignaturaId = 2,
                },
                new Bytee()
                {
                    Id = 7,
                    Data = (byte)248,
                    SignaturaId = 2,
                },
                new Bytee()
                {
                    Id = 8,
                    Data = (byte)0,
                    SignaturaId = 2,
                },
            });

            builder.Entity<Signatura>().HasData(new List<Signatura> 
            {
                new Signatura() { Id = 1,},
                new Signatura() { Id = 2,}
            }); 

            base.OnModelCreating(builder);
        
            
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=antivirusy;Trusted_Connection=True;");
        }
    }
}

using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Models.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Machine = Machine_Setup_Worksheet.Models.Machine;

namespace Machine_Setup_Worksheet.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Machine> Machines { get; set; }
        public DbSet<Jaw> Jaws { get; set; }
        public DbSet<Setup> Setups { get; set; }
        public DbSet<WorkSetup> WorkSetups { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Machine>().HasData(new Machine() { MachineName="Hwacheon", MachineId = Guid.NewGuid()});
            modelBuilder.Entity<Jaw>().HasData(new Jaw() { JawName = "Hard Jaws", JawId= Guid.NewGuid() });
        }

    }
}

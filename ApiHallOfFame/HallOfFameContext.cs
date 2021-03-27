using ApiHallOfFame.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiHallOfFame
{
    public class HallOfFameContext : DbContext
    {
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public HallOfFameContext(DbContextOptions<HallOfFameContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasMany(x => x.Skills).WithOne(x=>x.Person).OnDelete(DeleteBehavior.Cascade);
        }
    }
}

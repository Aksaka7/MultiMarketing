using Microsoft.EntityFrameworkCore;
using MultiMarketing.Context.Domain;

namespace MultiMarketing.Context
{
    public class MarketingDBContext(DbContextOptions<MarketingDBContext> options) : DbContext(options)
    {
        public DbSet<UserRegister> Registers { get; set; }
        public DbSet<UserRoles> Rollers { get; set; }
    }
}

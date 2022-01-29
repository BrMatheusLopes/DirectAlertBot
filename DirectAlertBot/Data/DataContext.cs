using DirectAlertBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectAlertBot.Data
{
    public class DataContext : DbContext
    {
#pragma warning disable CS8618
        public DataContext(DbContextOptions options) : base(options)
#pragma warning restore CS8618
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}

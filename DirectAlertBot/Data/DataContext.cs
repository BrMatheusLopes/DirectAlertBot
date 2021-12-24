using DirectAlertBot.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectAlertBot.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}

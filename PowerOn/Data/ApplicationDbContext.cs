using Microsoft.EntityFrameworkCore;
using PowerOn.Models; // Namespace onde seu modelo Usuario está

namespace PowerOn.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
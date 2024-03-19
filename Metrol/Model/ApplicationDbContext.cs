using Microsoft.EntityFrameworkCore;

namespace Metrol.Model
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Пользователь.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Конфигурация.
        /// </summary>
        /// <param name="optionsBuilder">Билдер.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MetrolTest;Trusted_Connection=True;TrustServerCertificate=true;");
        }
    }
}

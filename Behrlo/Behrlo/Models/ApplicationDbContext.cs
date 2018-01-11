using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Behrlo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Word> Words { get; set; }

        /// <summary>
        /// Makes sure the SQLite database exists.
        /// </summary>
        public async Task InitializeDatabaseAsync()
        {
            await Database.EnsureCreatedAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite("Data Source=data.db", (options) =>
            {
                options.SuppressForeignKeyEnforcement(false);
            });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>(word =>
            {
                word
                    .ToTable("Words");

                word
                    .HasKey(props => props.Id);
            });

            modelBuilder.Entity<Section>(section =>
            {
                section
                    .ToTable("Sections");

                section
                    .HasKey(props => props.Id);

                section
                    .HasMany(props => props.Words)
                        .WithOne(word => word.Section)
                        .HasForeignKey(word => word.SectionId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Notebook>(notebook =>
            {
                notebook
                    .ToTable("Notebooks");

                notebook
                    .HasKey(props => props.Id);

                notebook
                    .HasMany(props => props.Sections)
                        .WithOne(section => section.Notebook)
                        .HasForeignKey(section => section.NotebookId)
                        .OnDelete(DeleteBehavior.Cascade);

                notebook
                    .Ignore(props => props.CoverImage);
            });
        }
    }
}

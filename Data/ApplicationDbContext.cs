using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonRegistrationMvc.Models;


namespace PersonRegistrationMvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(p => p.FullName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Age).IsRequired();
                entity.Property(p => p.Major).IsRequired().HasMaxLength(100);
                entity.Property(p => p.CreatedBy).HasMaxLength(256);
                entity.Property(p => p.UpdatedBy).HasMaxLength(256);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.CreatedBy).HasMaxLength(256);
                entity.Property(b => b.UpdatedBy).HasMaxLength(256);
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(a => a.CreatedBy).HasMaxLength(256);
                entity.Property(a => a.UpdatedBy).HasMaxLength(256);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.CreatedBy).HasMaxLength(256);
                entity.Property(c => c.UpdatedBy).HasMaxLength(256);
            });

            modelBuilder.Entity<Borrow>(entity =>
            {
                entity.HasOne(br => br.Person)
                      .WithMany()
                      .HasForeignKey(br => br.PersonId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(br => br.Book)
                      .WithMany()
                      .HasForeignKey(br => br.BookId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(br => br.CreatedBy).HasMaxLength(256);
                entity.Property(br => br.UpdatedBy).HasMaxLength(256);
            });
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.CreatedBy).HasMaxLength(256);
                entity.Property(u => u.UpdatedBy).HasMaxLength(256);

                entity.HasOne(u => u.Person)
                      .WithOne()
                      .HasForeignKey<ApplicationUser>(u => u.PersonId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(u => u.PersonId)
                      .IsUnique()
                      .HasFilter("[PersonId] IS NOT NULL");
            });

            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);
        }
    }
}
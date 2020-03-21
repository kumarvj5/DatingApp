using KaamDatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace KaamDatingApp.API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options){}
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected  override void OnModelCreating(ModelBuilder builder)
        { 
            builder.Entity<Like>()
                .HasKey(c=> new {c.LikerId, c.LikeeId});

            builder.Entity<Like>()
                .HasOne(v =>v.Likee)
                .WithMany(v =>v.Likers)
                .HasForeignKey(v =>v.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(v =>v.Liker)
                .WithMany(v =>v.Likees)
                .HasForeignKey(v =>v.LikerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
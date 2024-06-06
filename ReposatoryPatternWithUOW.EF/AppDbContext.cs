using Microsoft.EntityFrameworkCore;
using ReposatoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposatoryPatternWithUOW.EF
{
    public class AppDbContext:DbContext
    {
        
        public DbSet<User> Users { get; set; }
        public DbSet<EmailVerificationCode> EmailVerificationCodes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AppDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RefreshToken>(x =>{
                x.HasKey(x => new { x.UserId, x.Token });
                x.HasOne(x=>x.User).WithMany(x=>x.RefreshTokens).HasForeignKey(x=>x.UserId);
                x.Property(w => w.Token).HasColumnType("varchar").HasMaxLength(44);
            });
            modelBuilder.Entity<EmailVerificationCode>(x =>
            {
                x.HasKey(x => new { x.UserId, x.Code });
                x.Property(w => w.Code).HasMaxLength(10).HasColumnType("varchar");
            });
        }
    }
}

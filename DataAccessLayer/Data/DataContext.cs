using ModelsLayer.DTO;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Entity;
using System.Net;

namespace DataAccessLayer.Data
{
#pragma warning disable CS1591
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<StudentCourses> StudentCourses { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<TeachersCourses> TeachersCourses { get; set; }
        public DbSet<Users> Users {  get; set; }
        public DbSet<Roles> Roles {  get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>()
                   .Property(x => x.BirthDate)
                   .HasColumnType("date");

            modelBuilder.Entity<StudentCourses>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourses>()
                .HasOne(sc => sc.Students)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourses>()
                .HasOne(sc => sc.Courses)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<Students>()
                .HasOne(s => s.Address)
                .WithOne(sa => sa.Students)
                .HasForeignKey<Addresses>(sa => sa.Id);

            modelBuilder.Entity<TeachersCourses>()
                .HasKey(tpc => new { tpc.TeacherId, tpc.CourseId });

            modelBuilder.Entity<TeachersCourses>()
                .HasOne(tpc => tpc.Teachers)
                .WithMany(t => t.TeachersCourses)
                .HasForeignKey(tpc => tpc.TeacherId);

            modelBuilder.Entity<TeachersCourses>()
                .HasOne(tpc => tpc.Courses)
                .WithMany(c => c.TeachersCourses)
                .HasForeignKey(tpc => tpc.CourseId);

            modelBuilder.Entity<UserRoles>()
                .HasKey (ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRoles>()
                .HasOne(u => u.Users)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRoles>()
                .HasOne(r => r.Roles)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(r => r.RoleId);
        }
    }
    #pragma warning restore CS1591
}

using Hospital_FinalP.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Hospital_FinalP.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<AppUser> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<WorkingSchedule> WorkingSchedules { get; set; }

        public DbSet<DocPhoto> DocPhotos { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorDetail> DoctorDetails { get; set; }
        public DbSet<ExaminationRoom> ExaminationRooms { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorType> DoctorTypes { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>()
            //    .Property(p => p.Status)
            //    .HasConversion<string>();


            base.OnModelCreating(modelBuilder);
        }

    }
}

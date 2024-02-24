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

        //public DbSet<DocPhoto> DocPhotos { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorDetail> DoctorDetails { get; set; }
        public DbSet<ExaminationRoom> ExaminationRooms { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DoctorType> DoctorTypes { get; set; }
        public DbSet<WorkingDay> WorkingDays { get; set; }
        public DbSet<Email> Emails { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>()
            //    .Property(p => p.Status)
            //    .HasConversion<string>();

            modelBuilder.Entity<WorkingSchedule>()
        .HasOne(ws => ws.Doctor) 
        .WithOne(d => d.WorkingSchedule) 
        .HasForeignKey<WorkingSchedule>(ws => ws.DoctorId);

            modelBuilder.Entity<Doctor>()
          .Property(p => p.PhotoPath)
          .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }

    }
}

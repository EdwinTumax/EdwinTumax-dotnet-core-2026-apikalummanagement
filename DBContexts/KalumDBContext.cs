using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiKalumManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiKalumManagement.DBContexts
{
    public class KalumDBContext : DbContext
    {
        public DbSet<AdmissionExam> AdmissionExams {get;set;}
        public DbSet<Applicant> Applicants {get;set;}
        public DbSet<Schedule> Schedules {get;set;}
        public DbSet<TechnicalCareer> TechnicalCareers {get;set;}
        public DbSet<AdmissionExamResult> AdmissionExamResults {get;set;}
        public KalumDBContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdmissionExam>().ToTable("AdmissionExam").HasKey(ae => ae.ExamId); 
            modelBuilder.Entity<Applicant>().ToTable("Applicant").HasKey(a => a.FileNumber);
            modelBuilder.Entity<Schedule>().ToTable("Schedule").HasKey(a => a.ScheduleId);
            modelBuilder.Entity<TechnicalCareer>().ToTable("TechnicalCareer").HasKey(tc => tc.CareerId);
            modelBuilder.Entity<AdmissionExamResult>().ToTable("AdmissionExamResult").HasKey(aer => new {aer.FileNumber, aer.Year});

            modelBuilder.Entity<Applicant>().HasOne<AdmissionExam>(a => a.AdmissionExam).WithMany(ea => ea.Applicants).HasForeignKey(a => a.ExamId);
            modelBuilder.Entity<Applicant>().HasOne<Schedule>(a => a.Schedule).WithMany(s => s.Applicants).HasForeignKey(a => a.ScheduleId);
            modelBuilder.Entity<Applicant>().HasOne<TechnicalCareer>(a => a.TechnicalCareer).WithMany(tc => tc.applicants).HasForeignKey(a => a.CareerId);
            modelBuilder.Entity<AdmissionExamResult>().HasOne<Applicant>(aer => aer.Applicant).WithMany(a => a.AdmissionExamResults).HasForeignKey(a => a.FileNumber);
        }

    }
}
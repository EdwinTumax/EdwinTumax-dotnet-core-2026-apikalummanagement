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
        public DbSet<EnrollmentPayment> EnrollmentPayments {set;get;}
        public DbSet<Student> Students {get;set;}
        public DbSet<Enrollment> Enrollments {get;set;}
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
            modelBuilder.Entity<EnrollmentPayment>().ToTable("EnrollmentPayment").HasKey(ep => ep.PaymentReceipt);
            modelBuilder.Entity<Student>().ToTable("Student").HasKey(s => s.StudentId);
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment").HasKey(e => e.EnrollmentId);


            modelBuilder.Entity<Applicant>().HasOne<AdmissionExam>(a => a.AdmissionExam).WithMany(ea => ea.Applicants).HasForeignKey(a => a.ExamId);
            modelBuilder.Entity<Applicant>().HasOne<Schedule>(a => a.Schedule).WithMany(s => s.Applicants).HasForeignKey(a => a.ScheduleId);
            modelBuilder.Entity<Applicant>().HasOne<TechnicalCareer>(a => a.TechnicalCareer).WithMany(tc => tc.Applicants).HasForeignKey(a => a.CareerId);
            modelBuilder.Entity<AdmissionExamResult>().HasOne<Applicant>(aer => aer.Applicant).WithMany(a => a.AdmissionExamResults).HasForeignKey(a => a.FileNumber);
            modelBuilder.Entity<EnrollmentPayment>().HasOne<Applicant>(ep => ep.Applicant).WithMany(a => a.EnrollmentPayments).HasForeignKey(a => a.FileNumber);
            modelBuilder.Entity<Enrollment>().HasOne<TechnicalCareer>(e => e.TechnicalCareer).WithMany(tc => tc.Enrollments).HasForeignKey(e => e.CareerId);
            modelBuilder.Entity<Enrollment>().HasOne<Schedule>(e => e.Schedule).WithMany(s => s.Enrollments).HasForeignKey(e => e.ScheduleId);
            modelBuilder.Entity<Enrollment>().HasOne<Student>(e => e.Student).WithMany(e => e.Enrollments).HasForeignKey(e => e.StudentId);

        }

    }
}
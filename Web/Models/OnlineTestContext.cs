﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Web.Models
{
    public partial class OnlineTestContext : DbContext
    {
        public OnlineTestContext()
        {
        }

        public OnlineTestContext(DbContextOptions<OnlineTestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionBank> QuestionBanks { get; set; }
        public virtual DbSet<RandomExam> RandomExams { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("data source=localhost;initial catalog=OnlineTest;persist security info=True;user id=sa;password=123456a@;multipleactiveresultsets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("Exam");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EndTime).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartTime).HasMaxLength(50);

                entity.Property(e => e.Time)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_Exam_QuestionBank");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exam_Account");
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.HasKey(e => new { e.ExamId, e.QuestionId });

                entity.ToTable("Exam_Question");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK_ExamKit_Exam");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExamKit_Question");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_Question_QuestionBank");
            });

            modelBuilder.Entity<QuestionBank>(entity =>
            {
                entity.ToTable("QuestionBank");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ModifiedDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.QuestionBanks)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionBank_Account");
            });

            modelBuilder.Entity<RandomExam>(entity =>
            {
                entity.ToTable("RandomExam");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EndTime).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartTime).HasMaxLength(50);

                entity.Property(e => e.Time)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.RandomExams)
                    .HasForeignKey(d => d.BankId)
                    .HasConstraintName("FK_RandomExam_Exam");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.RandomExams)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RandomExam_Account");
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.ToTable("Score");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AnswerContent).IsRequired();

                entity.Property(e => e.ExamName).HasMaxLength(50);

                entity.Property(e => e.Score1).HasColumnName("Score");

                entity.Property(e => e.StartTime)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Time)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK_Score_Exam");

                entity.HasOne(d => d.RandomExam)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.RandomExamId)
                    .HasConstraintName("FK_Score_RandomExam1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Score_Account1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("UserDetail");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserDetail)
                    .HasForeignKey<UserDetail>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDetail_Account");
            });
        }
    }
}

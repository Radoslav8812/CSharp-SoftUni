﻿using System;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
	public class StudentSystemContext : DbContext
	{
		public StudentSystemContext()
		{
		}

        public StudentSystemContext(DbContextOptions options) : base (options)
        {

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=StudentSystem;Trusted_Connection=True;Integrated Security=false;User Id=sa;Password=Docker@123");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>(x => x.HasKey(x => new
            {
                x.CourseId,
                x.StudentId
            }));

            base.OnModelCreating(modelBuilder);
        }
    }
}


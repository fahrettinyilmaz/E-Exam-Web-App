using EExamWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using EExamWebApp.Data;

namespace EExamWebApp.Seeders
{
    public static class DbInitializer 
    {
        public static void Seed(AppDbContext context)
        {   
            Console.Out.WriteLine("Seeding started!");

            // Check if the database has been seeded already
            if (context.Users.Any())
            {
                Console.Out.WriteLine("Database has been seeded already!");
                return;   // DB has been seeded
            }
            Console.Out.WriteLine("Seed Admin username:admin password:123");
            context.Users.Add(new User { Username = "admin", Email = "admin@yopmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), UserType = UserType.Admin, IsApproved = true});
            
            // Seed Teachers
            Console.Out.WriteLine("Seed Teachers");
            var teachers = new List<User>();
            for (int i = 1; i <= 5; i++)
            {
                teachers.Add(new User { Username = $"teacher{i}", Email = $"teacher{i}@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password"), UserType = UserType.Teacher, IsApproved = true });
            }
            context.Users.AddRange(teachers);
            context.SaveChanges();
            Console.Out.WriteLine("Teachers seeded!");

            // Seed Students
            Console.Out.WriteLine("Seed Students");
            var students = new List<User>();
            for (int i = 1; i <= 50; i++)
            {
                students.Add(new User { Username = $"student{i}", Email = $"student{i}@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password"), UserType = UserType.Student, IsApproved = true });
            }
            context.Users.AddRange(students);
            context.SaveChanges();
            Console.Out.WriteLine("Students seeded!");

            // Seed Courses and Exams
            Console.Out.WriteLine("Seed Courses and Exams");
            foreach (var teacher in teachers)
            {
                if (teacher == null)
                {
                    Console.Out.WriteLine("Teacher is null.");
                    continue;
                }

                Console.Out.WriteLine($"Creating course for {teacher.Username}");
                var course = new Course { Title = $"{teacher.Username}'s Course", Description = "Course Description", Teacher = teacher };
                if (course.Exams == null)
                {
                    Console.Out.WriteLine("Course.Exams collection is null. Initializing...");
                    course.Exams = new List<Exam>();
                }

                Console.Out.WriteLine("Creating exam...");
                var exam = new Exam { Title = "Exam Title", Description = "Exam Description" };
                if (exam.Questions == null)
                {
                    Console.Out.WriteLine("Exam.Questions collection is null. Initializing...");
                    exam.Questions = new List<Question>();
                }

                // Seed Questions and Options
                for (int q = 0; q < 10; q++)
                {
                    var question = new Question { Text = $"Question {q + 1}", Options = new List<Option>() };

                    // Seed Options
                    for (int o = 0; o < 5; o++)
                    {
                        question.Options.Add(new Option { Text = $"Option {o + 1}", IsCorrect = o == 0 }); // Mark the first option as correct
                    }

                    exam.Questions.Add(question);
                }

                Console.Out.WriteLine("Adding exam to course.");
                course.Exams.Add(exam);

                Console.Out.WriteLine("Adding course to context.");
                context.Courses.Add(course);
            }

            Console.Out.WriteLine("Saving to db!");
            context.SaveChanges();
            Console.Out.WriteLine("Seeding done!");
        }
    }
}

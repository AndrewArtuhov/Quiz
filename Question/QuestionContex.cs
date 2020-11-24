using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionModel
{
    public class QuestionContex : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
             => options.UseSqlite("Data Source=question.db");

        public DbSet<Theme> Themes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Timer> Timers { get; set; }

    }

    public class Question
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public Theme Theme { get; set; }
        public Timer Timer { get; set; }
    }

    public class Timer
    {
        [Key]
        public long Id { get; set; }
        public int Time { get; set; }
    }

    public class Answer
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Condition { get; set; }
        public Question Question { get; set; }
    }

    public class Theme
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }

}

using Kviz.Migrations.Tables;
using Microsoft.EntityFrameworkCore;

namespace Kviz.Data
{
    public class DataContext: DbContext
    {
        protected readonly IConfiguration Configuration;
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<UserTable> Users { get; set; }
        public DbSet<QuizTable> Quizes { get; set; }
        public DbSet<SessionTable> Sessions { get; set; }
        public DbSet<QuestionTable> Questions { get; set; }
        public DbSet<AnswerTable> Answers { get; set; }
        public DbSet<HistoryTable> Histories { get; set; }
        public DbSet<QuizHistoryTable> QuizHistories { get; set; }
        public DbSet<QuestionHistoryTable> QuestionHistories { get; set; }
        public DbSet<AnswerHistoryTable> AnswerHistories { get; set; }
    }
}

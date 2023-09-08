using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.DAL;

public partial class ELearningContext : IdentityDbContext
{
    public ELearningContext()
    {
    }

    public ELearningContext(DbContextOptions<ELearningContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Assighment> Assighments { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Lecture> Lectures { get; set; }

    public virtual DbSet<LectureCode> LectureCodes { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quize> Quizes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAnswer> UserAnswers { get; set; }

    public virtual DbSet<UserAssighment> UserAssighments { get; set; }
    public virtual DbSet<UserClassRequists>  UserClassRequists { get; set; }
    public virtual DbSet<Notification>  Notifications { get; set; }


    public virtual DbSet<UserLecture> UserLectures { get; set; }

    public virtual DbSet<UserQuiz> UserQuizzes { get; set; }

    public virtual DbSet<VideoPart> VideoParts { get; set; }

    public virtual DbSet<Year> Years { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=E-Learningk ;Trusted_Connection=True; encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUser>().UseTptMappingStrategy();

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasIndex(e => e.Questionid, "IX_Answers_Questionid");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.Questionid)
                .HasConstraintName("FK_Answers_Questions").OnDelete(DeleteBehavior.Cascade)


            ;
        });

        modelBuilder.Entity<Assighment>(entity =>
        {
            entity.ToTable("Assighment");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasIndex(e => e.Yearid, "IX_Classes_yearid");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Yearid).HasColumnName("yearid");

            entity.HasOne(d => d.Year).WithMany(p => p.Classes)
                .HasForeignKey(d => d.Yearid)
                .HasConstraintName("FK_Classes_Year").OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Lecture>(entity =>
        {
            entity.ToTable("Lecture");

            entity.HasIndex(e => e.Assighnmentid, "IX_Lecture_Assighnmentid");

            entity.HasIndex(e => e.Classid, "IX_Lecture_Classid");

            entity.HasIndex(e => e.Quizid, "IX_Lecture_Quizid");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");

            entity.HasOne(d => d.Assighnment).WithMany(p => p.Lectures)
                .HasForeignKey(d => d.Assighnmentid)
                .HasConstraintName("FK_Lecture_Assighment").OnDelete(DeleteBehavior.SetNull)
;

            entity.HasOne(d => d.Class).WithMany(p => p.Lectures).OnDelete(DeleteBehavior.Cascade)

                .HasForeignKey(d => d.Classid)
                .HasConstraintName("FK_Lecture_Classes");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Lectures).OnDelete(DeleteBehavior.SetNull)

                .HasForeignKey(d => d.Quizid)
                .HasConstraintName("FK_Lecture_Quizes");
        });

        modelBuilder.Entity<LectureCode>(entity =>
        {
            entity.ToTable("LectureCode");

            entity.HasIndex(e => e.Lectureid, "IX_LectureCode_Lectureid");

            entity.HasIndex(e => e.StudentId, "IX_LectureCode_StudentId");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");
            entity.Property(e => e.Used).HasColumnName("used");
            entity.Property(e => e.Usedate).HasColumnType("datetime");

            entity.HasOne(d => d.Lecture).WithMany(p => p.LectureCodes)
                .HasForeignKey(d => d.Lectureid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LectureCode_Lecture").OnDelete(DeleteBehavior.Cascade)
;

            entity.HasOne(d => d.Student).WithMany(p => p.LectureCodes)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_LectureCode_User").OnDelete(DeleteBehavior.Cascade)
;
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasIndex(e => e.QuizId, "IX_Questions_QuizId");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.Quiz).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK_Questions_Quizes").OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(a => a.RightAnswer).WithOne(x => x.RQuestion)
       .HasForeignKey<Question>(a => a.RightAnswerid).OnDelete(DeleteBehavior.NoAction);


        });
       

        modelBuilder.Entity<Quize>(entity =>
        {
            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Header).HasColumnName("header");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Yearid, "IX_User_Yearid");

            entity.Property(e => e.Id)
.ValueGeneratedNever()                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.ParentPhoneNumber).HasMaxLength(50);
            entity.Property(e => e.StudentPhoneNumber).HasMaxLength(50);
            entity.Property(e => e.SecondName).HasMaxLength(50);

            entity.HasOne(d =>d.Year).WithMany(d=>d.Users).HasForeignKey(d => d.Yearid).OnDelete(DeleteBehavior.SetNull);
            entity.HasMany(d => d.Classes).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserClass",
                    r => r.HasOne<Class>().WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_UserClasses_Classes"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_UserClasses_User"),
                    j =>
                    {
                        j.HasKey("UserId", "ClassId");
                        j.ToTable("UserClasses");
                        j.HasIndex(new[] { "ClassId" }, "IX_UserClasses_ClassId");
                    });
        });

        modelBuilder.Entity<UserAnswer>(entity =>
        {
            entity.ToTable("UserAnswer");

            entity.HasIndex(e => e.QuestionId, "IX_UserAnswer_QuestionId");

            entity.HasIndex(e => e.UserId, "IX_UserAnswer_UserID");

            entity.HasIndex(e => e.UserQuizId, "IX_UserAnswer_UserQuizId");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");
            entity.Property(e => e.Right).HasColumnName("right");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Question).WithMany(p => p.UserAnswers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_UserAnswer_Questions").OnDelete(DeleteBehavior.NoAction)
;

            entity.HasOne(d => d.User).WithMany(p => p.UserAnswers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserAnswer_User").OnDelete(DeleteBehavior.NoAction)
;

            entity.HasOne(d => d.UserQuiz).WithMany(p => p.UserAnswers)
                .HasForeignKey(d => d.UserQuizId)
                .HasConstraintName("FK_UserAnswer_UserQuiz").OnDelete(DeleteBehavior.Cascade)
;
        });

        modelBuilder.Entity<UserAssighment>(entity =>
        {
            entity.ToTable("UserAssighment");

            entity.HasIndex(e => e.Assighmentid, "IX_UserAssighment_Assighmentid");

            entity.HasIndex(e => e.Studentid, "IX_UserAssighment_Studentid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Checked).HasColumnName("checked");
            entity.Property(e => e.Solved).HasColumnName("solved");

            entity.HasOne(d => d.Assighment).WithMany(p => p.UserAssighments)
                .HasForeignKey(d => d.Assighmentid)
                .HasConstraintName("FK_UserAssighment_Assighment").OnDelete(DeleteBehavior.Cascade)
;

            entity.HasOne(d => d.Student).WithMany(p => p.UserAssighments)
                .HasForeignKey(d => d.Studentid)
                .HasConstraintName("FK_UserAssighment_User").OnDelete(DeleteBehavior.Cascade)
;
        });

        modelBuilder.Entity<UserLecture>(entity =>
        {
            entity.ToTable("UserLecture");

            entity.HasIndex(e => e.Lectureid, "IX_UserLecture_Lectureid");

            entity.HasIndex(e => e.StudentId, "IX_UserLecture_StudentId");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");
            entity.Property(e => e.End)
                .HasColumnType("datetime")
                .HasColumnName("end");
            entity.Property(e => e.Start).HasColumnType("datetime");

            entity.HasOne(d => d.Lecture).WithMany(p => p.UserLectures)
                .HasForeignKey(d => d.Lectureid)
                .HasConstraintName("FK_UserLecture_Lecture").OnDelete(DeleteBehavior.Cascade)
;

            entity.HasOne(d => d.Student).WithMany(p => p.UserLectures)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK_UserLecture_User").OnDelete(DeleteBehavior.Cascade)
;
        });

        modelBuilder.Entity<UserQuiz>(entity =>
        {
            entity.ToTable("UserQuiz");

            entity.HasIndex(e => e.Quizid, "IX_UserQuiz_Quizid");

            entity.HasIndex(e => e.Studentid, "IX_UserQuiz_Studentid");

            entity.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnName("id");
            entity.Property(e => e.End).HasColumnType("datetime");
            entity.Property(e => e.Start).HasColumnType("datetime");

            entity.HasOne(d => d.Quiz).WithMany(p => p.UserQuizzes)
                .HasForeignKey(d => d.Quizid)
                .HasConstraintName("FK_UserQuiz_Quizes").OnDelete(DeleteBehavior.Cascade)
;

            entity.HasOne(d => d.Student).WithMany(p => p.UserQuizzes)
                .HasForeignKey(d => d.Studentid)
                .HasConstraintName("FK_UserQuiz_User").OnDelete(DeleteBehavior.Cascade)
;
        });

        modelBuilder.Entity<VideoPart>(entity =>
        {
            entity.ToTable("videoParts");

            entity.HasIndex(e => e.Leactureid, "IX_videoParts_Leactureid");

            entity.Property(e => e.Id)               .UseIdentityColumn();
            entity.Property(e => e.UpdatedBy).HasDefaultValueSql("(N'')");

            entity.HasOne(d => d.Leacture).WithMany(p => p.VideoParts)
                .HasForeignKey(d => d.Leactureid)
                .HasConstraintName("FK_videoParts_Lecture").OnDelete(DeleteBehavior.Cascade)
;
        });

        modelBuilder.Entity<Year>(entity =>
        {
            entity.ToTable("Year");

            entity.Property(e => e.Id).UseIdentityColumn();

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdatedBy).HasDefaultValueSql("(N'')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

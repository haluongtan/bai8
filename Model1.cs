using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WindowsFormsApp15
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<Major> Major { get; set; }
        public virtual DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>()
                .HasMany(e => e.Major)
                .WithRequired(e => e.Faculty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Major>()
                .HasMany(e => e.Student)
                .WithOptional(e => e.Major)
                .HasForeignKey(e => new { e.FacultyID, e.MajorID });

            modelBuilder.Entity<Student>()
                .Property(e => e.StudentID)
                .IsFixedLength();
        }
    }
}

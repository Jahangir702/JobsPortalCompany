using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Final.Models
{
    public enum AppliedFor { Manager = 1, MarketingOfficer, OfficeAssistant, Others }

    public class Candidate
    {
        public int CandidateId { get; set; }
        [Required, StringLength(50)]
        public string CandidateName { get; set; }
        [Required, DataType(DataType.Date)]
        public System.DateTime BirthDate { get; set; }
        [Required, EnumDataType(typeof(AppliedFor))]
        public AppliedFor AppliedFor { get; set; }
        [Required, Column(TypeName = "money"), DataType(DataType.Currency)]
        public decimal ExpectedSalary { get; set; }
        public bool Conditions { get; set; }
        [Required, StringLength(30)]
        public string Picture { get; set; }
        public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();
    }
    public class Qualification
    {
        public int QualificationId { get; set; }
        [Required, StringLength(50)]
        public string Degree { get; set; }
        [Required]
        public int PassingYear { get; set; }
        [Required, StringLength(50)]
        public string Institute { get; set; }
        [Required, StringLength(20)]
        public string Result { get; set; }
        [Required, ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
    }
    public class CandidateDbContext : DbContext
    {
        public CandidateDbContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
    }
    public class DbInitializer : DropCreateDatabaseIfModelChanges<CandidateDbContext>
    {
        protected override void Seed(CandidateDbContext context)
        {
            Candidate c = new Candidate { CandidateName = "Jahangir Alam", BirthDate = new DateTime(1999, 4, 1), AppliedFor = AppliedFor.Manager, ExpectedSalary = 10500.00M, Picture = "1.jpg", Conditions = true };
            c.Qualifications.Add(new Qualification { Degree = "HSC", PassingYear = 2015, Institute = "Hossenpur", Result = "4.31" });
            context.Candidates.Add(c);
            context.SaveChanges();
        }
    }
}

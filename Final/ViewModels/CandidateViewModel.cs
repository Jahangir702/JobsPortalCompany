using Final.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Final.ViewModels
{
    public enum AppliedFor { Manager = 1, MarketingOfficer, OfficeAssistant, Others }

    public class CandidateViewModel
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
        [Required]
        public HttpPostedFileBase Picture { get; set; }
        public virtual List<Qualification> Qualifications { get; set; } = new List<Qualification>();
    }
}
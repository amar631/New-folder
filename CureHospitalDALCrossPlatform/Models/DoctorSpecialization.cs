using System;
using System.Collections.Generic;

namespace CureHospitalDALCrossPlatform.Models
{
    public partial class DoctorSpecialization
    {
        public int DoctorId { get; set; }
        public string SpecializationCode { get; set; }
        public DateTime SpecializationDate { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Specialization SpecializationCodeNavigation { get; set; }
    }
}

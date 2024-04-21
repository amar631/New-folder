using System;
using System.Collections.Generic;

namespace CureHospitalDALCrossPlatform.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            DoctorSpecialization = new HashSet<DoctorSpecialization>();
            Surgery = new HashSet<Surgery>();
        }

        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        public virtual ICollection<DoctorSpecialization> DoctorSpecialization { get; set; }
        public virtual ICollection<Surgery> Surgery { get; set; }
    }
}

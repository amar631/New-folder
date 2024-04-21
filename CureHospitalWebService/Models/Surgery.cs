using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CureHospitalWebService.Models
{
    public class Surgery
    {
        //To Do: Implement appropriate logic
        [Required]
        public int SurgeryId { get; set; }
        public int? DoctorId { get; set; }
        //[Required]
        public DateTime SurgeryDate { get; set; }
        [Required]
        public decimal StartTime { get; set; }
        [Required]
        public decimal EndTime { get; set; }
        //[Required]
        public string SurgeryCategory { get; set; }
    }
}

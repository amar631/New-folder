using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CureHospitalDALCrossPlatForm;
using CureHospitalWebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CureHospitalWebService.Controllers
{
    [Route("api/[controller]/[action]")]

    [ApiController]
    public class HomeController : Controller
    {
        HospitalRepository hospital;
        public HomeController()
        {
            //To Do: Implement appropriate logic
            hospital = new HospitalRepository(); 
        }

        #region FetchDoctorIDs - Do not modify the signature
       [HttpGet]
        public JsonResult FetchDoctorIDs(string specializationCode)
        {
            List<int> docL = null;
            try
            {
                docL = hospital.FetchDoctorIDs(specializationCode);
            }
            catch (Exception)
            {
                docL = null;
            }
            return Json(docL);
        }
        #endregion
        [HttpPost]
        #region AddDoctorSpecialization - Do not modify the signature
       
        public bool AddDoctorSpecialization(int doctorId, string specializationCode, DateTime specializationDate)
        {
            bool status = false;
            try
            {
                status = hospital.AddDoctorSpecialization(doctorId, specializationCode, specializationDate);
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        #endregion

        #region UpdateSurgeryTime - Do not modify the signature

      [HttpPut]
        public int UpdateSurgeryTime(Surgery surgery)
        {
            int status = 0;
            try
            {
                Surgery sur = new Surgery();
                sur.SurgeryId = surgery.SurgeryId;
                sur.EndTime = surgery.EndTime;
                status = hospital.UpdateSurgeryTime(sur.SurgeryId, sur.EndTime);
            }
            catch (Exception)
            {
                status = 0;
            }
            return status;
        }
        #endregion

        #region RemoveSurgeryDetails - Do not modify the signature
      [HttpDelete]
        public JsonResult RemoveSurgeryDetails(DateTime surgeryDate)
        {
            bool status = false;
            try
            {
                status = hospital.RemoveSurgeryDetails(surgeryDate);
            }
            catch (Exception)
            {
                status = false;
            }
            return Json(status);
        }
        #endregion
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PolyclinicDALCrossPlatform;
using PolyclinicDALCrossPlatform.Models;

//using PolyclinicWebServices.Models;

namespace PolyclinicWebServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : Controller
    {
        //Create repository object
        PolyclinicRepository repository;

        public AdminController()//PolyclinicRepository repos
        {
            //Implement the logic here
            repository = new PolyclinicRepository();
        }
        [httpGet]
        public JsonResult GetAllPatientDetails()
        {
            //Implement the logic here
            List<Patients> patientList = new List<Patients>();
            try
            {
                patientList = repository.GetAllPatientDetails();
            }
            catch (global::System.Exception ex)
            {

                patientList = null;
            }
           return Json(patientList);
        }

        public JsonResult GetPatientDetails(string patientId)
        {
            //Implement the logic here
            return null;
        }

        public JsonResult AddNewPatientDetails(Models.Patient patient)
        {
            //Implement the logic here
            bool status = false;
            return Json(status);
        }

        public JsonResult UpdatePatientAge(string patientId, byte age)
        {
            //Implement the logic here
            bool status = false;
            return Json(status);
        }

        public JsonResult CancelAppointment(int appointmentNo)
        {
            //Implement the logic here
            int status = 0;
            return Json(status);
        }

    }
}

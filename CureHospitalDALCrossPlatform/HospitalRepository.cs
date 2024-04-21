using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using CureHospitalDALCrossPlatform.Models;

namespace CureHospitalDALCrossPlatForm
{
    public class HospitalRepository
    {
        #region Uncomment the below line
        CureHospitalDBContext context;
        #endregion

        #region Constructor - Do not modify the signature
        public HospitalRepository()
        {
            //To Do: Implement appropriate logic
            context = new CureHospitalDBContext();
        }
        #endregion

        #region FetchDoctorIDs - Do not modify the signature
        public List<int> FetchDoctorIDs(string specializationCode)
        {
            //To Do: Implement appropriate logic and change the return statement as per your logic
            List<int> docList = null;
            try
            {
                docList = (from s in context.DoctorSpecialization where s.SpecializationCode == specializationCode select s.DoctorId).ToList();
                //docList = context.DoctorSpecialization.Select(d =>d.DoctorId).Where(d => d.SpecializationCode == specializationCode).ToList();
            }
            catch (Exception)
            {
                docList = null;
                
            }

            return docList;
        }
        #endregion

        #region AddDoctorSpecialization - Do not modify the signature
        public bool AddDoctorSpecialization(int doctorId, string specializationCode, DateTime specializationDate)
        {
            //To Do: Implement appropriate logic and change the return statement as per your logic
            bool status = false;
            DoctorSpecialization docSp = new DoctorSpecialization();
            docSp.DoctorId = doctorId;
            docSp.SpecializationCode = specializationCode;
            docSp.SpecializationDate = specializationDate;

            try
            {
                context.DoctorSpecialization.Add(docSp);
                context.SaveChanges();
                status = true;
            }
            catch(Exception)
            {
                status = false;
            }
            return status;
            
        }
        #endregion

        #region UpdateSurgeryTime - Do not modify the signature
        public int UpdateSurgeryTime(int surgeryID, decimal newEndTime)
        {
            //To Do: Implement appropriate logic and change the return statement as per your logic
            int status = 0;
            Surgery sur = context.Surgery.Find(surgeryID);
            try
            {
                if (sur != null)
                {
                    sur.EndTime = newEndTime;
                    context.SaveChanges();
                    status = 1;
                }
                else
                {
                    status = 0;
                }
            }
            catch (Exception)
            {
                status = 0;
            }
            return status;
        }
        #endregion

        #region RemoveSurgeryDetails - Do not modify the signature
        public bool RemoveSurgeryDetails(DateTime surgeryDate)
        {
            //To Do: Implement appropriate logic and change the return statement as per your logic
            
            bool status = false;
            Surgery sur = null;
            try
            {
                //sur = context.Surgery.Find(surgeryDate);
                sur = context.Surgery.Where(p => p.SurgeryDate == surgeryDate).FirstOrDefault();
                if (sur != null)
                {
                    context.Surgery.Remove(sur);
                    context.SaveChanges();
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        #endregion

        #region AddSurgeryDetails - Do not modify the signature
        public int AddSurgeryDetails(int doctorID, DateTime surgeryDate, decimal startTime, decimal endTime, string surgeryCategory, out int surgeryID)
        {
            //To Do: Implement appropriate logic and change the return statement as per your logic
            surgeryID = 0;
            int noOfRowsAffected = 0;
            int returnResult = 0;

            SqlParameter prmDoctorId = new SqlParameter("@DoctorID", doctorID);
            SqlParameter prmSurgeryDate = new SqlParameter("@SurgeryDate", surgeryDate);
            SqlParameter prmStartTime = new SqlParameter("@StartTime", startTime);
            SqlParameter prmEndTime = new SqlParameter("@EndTime", endTime);
            SqlParameter prmSurgeryCategory = new SqlParameter("@SurgeryCategory", surgeryCategory);
            SqlParameter prmSurgeryID = new SqlParameter("@SurgeryID", System.Data.SqlDbType.Int);
            prmSurgeryID.Direction = System.Data.ParameterDirection.Output;
            SqlParameter prmReturnResult = new SqlParameter("@ReturnResult", System.Data.SqlDbType.Int);
            prmReturnResult.Direction = System.Data.ParameterDirection.Output;
            try
            {
                noOfRowsAffected = context.Database.ExecuteSqlRaw("EXEC @ReturnResult = usp_AddSurgeryDetails @DoctorID, @SurgeryDate, @StartTime, @EndTime, @SurgeryCategory, @SurgeryID OUT", prmReturnResult, prmDoctorId, prmSurgeryDate, prmStartTime, prmEndTime, prmSurgeryCategory, prmSurgeryID);
                returnResult = Convert.ToInt32(prmReturnResult.Value);
                surgeryID = Convert.ToInt32(prmSurgeryID.Value);
            }
            catch (Exception)
            {
                returnResult = 0;
                //noOfRowsAffected = -1;
                surgeryID = 0;
            }
            return returnResult;
        }
        #endregion

        //Uncomment below code to implement the functionality
        #region GetSurgeryDetails - Do not modify the signature
        public List<Surgery> GetSurgeryDetails(DateTime surgeryDate)
        {
            //To Do: Implement appropriate logic and change the return statement as per your logic
            List<Surgery> lstSurgery = null;
            try
            {
                SqlParameter prmSurgeryDate = new SqlParameter("@SurgeryDate", surgeryDate);
                lstSurgery = context.Surgery.FromSqlRaw("SELECT * FROM ufn_FetchSurgeryDetails(@SurgeryDate)",
                                                        prmSurgeryDate).ToList();

            }
            catch (Exception)
            {
                lstSurgery = null;
            }
            return lstSurgery;
        }
        #endregion

    }
}

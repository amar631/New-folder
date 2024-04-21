using System;
using System.Collections.Generic;
using System.Data;
using CureHospitalDALCrossPlatForm;

namespace CureHospitalTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestFetchDoctorIDs();  // STATUS - DONE
            //TestAddDoctorSpecialization();  // STATUS -DONE
            //TestUpdateSurgeryTime();   // STATUS -DONE
            //TestRemoveSurgeryDetails(); // STATUS - DONE
            //TestAddSurgeryDetails();  // STATUS - DONE
            //TestGetSurgeryDetails();  // STATUS - DONE
            Console.ReadLine();
        }


        static void TestFetchDoctorIDs()
        {
            HospitalRepository hospitalRepository = new HospitalRepository();
            List<int> doctorIDs = hospitalRepository.FetchDoctorIDs("CAR");
            if (doctorIDs.Count != 0)
            {
                foreach (var doctorID in doctorIDs)
                {
                    Console.WriteLine(doctorID);
                }
            }
            else
                Console.WriteLine("There are no doctors present for the given specialization!");
        }

        static void TestAddDoctorSpecialization()
        {
            HospitalRepository hospitalRepository = new HospitalRepository();
            if (hospitalRepository.AddDoctorSpecialization(1002, "GYN", DateTime.Today.Date))
                Console.WriteLine("The details have been added successfully!");
            else Console.WriteLine("The details could not be added!");
        }

        static void TestUpdateSurgeryTime()
        {
            HospitalRepository hospitalRepository = new HospitalRepository();
            int returnValue = hospitalRepository.UpdateSurgeryTime(5000, 11);
            if (returnValue == 1)
                Console.WriteLine("The End Time has been updated successfully!");
            else if (returnValue == -1)
                Console.WriteLine("The End Time is less than the Start Time!");
            else if (returnValue == -2)
                Console.WriteLine("The End Time could not be updated!");
        }

        static void TestRemoveSurgeryDetails()
        {
            HospitalRepository hospitalRepository = new HospitalRepository();
            if (hospitalRepository.RemoveSurgeryDetails(new DateTime(2015, 1, 1)))
                Console.WriteLine("The details have been removed successfully!");
            else
                Console.WriteLine("The details could not be removed!");
        }
 	    static void TestAddSurgeryDetails()
        {
            HospitalRepository hospitalRepository = new HospitalRepository();
            int surgeryId;
            int result = hospitalRepository.AddSurgeryDetails(1003,new DateTime(2021, 12, 30), 10, 12,"CAR", out surgeryId);
            if (result == 1)
            {
                Console.WriteLine("Surgery details added successfully! SurgeryId = {0}\n", surgeryId);
            }
            else if (result == -1)
            {
                Console.WriteLine("DoctorId is mandatory.\n");
            }
            else if (result == -2)
            {
                Console.WriteLine("DoctorSpecialization details already exists!\n");
            }
            else if (result == -3)
            {
                Console.WriteLine("Please provide correct Surgery Details.\n");
            }
            else
            {
                Console.WriteLine("Some error occurred! Please try again later.\n");
            }
            
        }
        //Uncomment to test this method
        static void TestGetSurgeryDetails()
        {
            HospitalRepository hospitalRepository = new HospitalRepository();
            DateTime surgeryDate = new DateTime(2021, 12, 30);
            var surgery = hospitalRepository.GetSurgeryDetails(surgeryDate);
            Console.WriteLine("{0, -20}{1, -25}{2, -30}{3, -20}{4, -20}", "SurgeryId", "DoctorId", "StartTime", "EndTime", "SurgeryCategory");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
            if (surgery.Count == 0)
            {
                Console.WriteLine("No surgery details available under the given date!");
            }
            else
            {
                foreach (var s in surgery)
                {
                    Console.WriteLine("{0, -20}{1, -25}{2, -30}{3, -20}{4, -20}", s.SurgeryId, s.DoctorId, s.StartTime, s.EndTime, s.SurgeryCategory);
                }
            }
        }
    }
}

﻿using Hospital_FinalP.DTOs.Apointment;
using Hospital_FinalP.DTOs.DocPhoto;
using Hospital_FinalP.DTOs.DoctorDetail;
using Hospital_FinalP.DTOs.ExaminationRooms;
using Hospital_FinalP.Entities;

namespace Hospital_FinalP.DTOs.Patients
{
    public class PatientGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string IdentityNumber { get; set; }//ID(FIN)
        public DateTime BirthDate { get; set; }

        public List<AppointmentGetDto> Appointments { get; set; }
    }
}

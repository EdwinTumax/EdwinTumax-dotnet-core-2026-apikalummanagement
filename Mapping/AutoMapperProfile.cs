using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiKalumManagement.DTOs.Requests;
using ApiKalumManagement.DTOs.Responses;
using ApiKalumManagement.Entities;
using AutoMapper;

namespace ApiKalumManagement.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Applicant, ApplicantListDTO>();
            CreateMap<ApplicantUpdateDTO, Applicant>();
            CreateMap<AdmissionExam, AdmissionExamListDTO>();
            CreateMap<Schedule, ScheduleListDTO>();
            CreateMap<TechnicalCareer, TechnicalCareerListDTO>();
            CreateMap<AdmissionExamCreatedDTO, AdmissionExam>();
            CreateMap<AdmissionExamUpdateDTO, AdmissionExam>();
            CreateMap<AdmissionExam, AdmissionExamListDTO>();
            CreateMap<AdmissionExamResultCreatedDTO, AdmissionExamResult>();
            CreateMap<AdmissionExamResultUpdateDTO, AdmissionExamResult>();
            CreateMap<AdmissionExamResult, AdmissionExamResultListDTO>();
            CreateMap<CareerInvestmentCreatedDTO, CareerInvestment>();
            CreateMap<CareerInvestmentUpdateDTO, CareerInvestment>();
            CreateMap<CareerInvestment, CareerInvestmentListDTO>();
            CreateMap<TechnicalCareerCreatedDTO, TechnicalCareer>();
            CreateMap<TechnicalCareerUpdateDTO, TechnicalCareer>();
            CreateMap<TechnicalCareer, TechnicalCareerListDTO>();
            CreateMap<ChargeCreatedDTO, Charge>();
            CreateMap<ChargeUpdateDTO, Charge>();
            CreateMap<Charge, ChargeListDTO>();
            CreateMap<EnrollmentCreatedDTO, Enrollment>();
            CreateMap<EnrollmentUpdateDTO, Enrollment>();
            CreateMap<Enrollment, EnrollmentListDTO>();
            CreateMap<EnrollmentPaymentCreatedDTO, EnrollmentPayment>();
            CreateMap<EnrollmentPaymentUpdateDTO, EnrollmentPayment>();
            CreateMap<EnrollmentPayment, EnrollmentPaymentListDTO>();
        }
    }
}
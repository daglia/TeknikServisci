using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TeknikServisci.BLL.Identity;
using TeknikServisci.Models.Entities;
using TeknikServisci.Models.ViewModels;

namespace TeknikServisci.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                CategoryMapping(cfg);
                FailureMapping(cfg);
                InvoiceMapping(cfg);
                RegisterMapping(cfg);
                SurveyMapping(cfg);
            });



        }

        private static void RegisterMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Operation, RegisterViewModel>().ReverseMap();
        }
        private static void InvoiceMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Operation, OperationViewModel>().ReverseMap();
        }
        private static void SurveyMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Survey, SurveyViewMdel>()
                .ForMember(dest => dest.SurveyId, opt => opt.MapFrom(x => x.Id))
                .ReverseMap();
        }

        private static void FailureMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Failure, FailureViewModel>()
                .ForMember(dest => dest.FailureId, opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.CreatedTime,
                    opt => opt.MapFrom((s, d) => s.CreatedDate == null ? DateTime.Now : s.CreatedDate))
                .ForMember(dest => dest.Operator,
                    opt => opt.MapFrom(
                        (s, d) => s.Operator == null ? "-" : (s.Operator.Name + " " + s.Operator.Surname)))
                .ForMember(dest => dest.Technician,
                    opt => opt.MapFrom((s, d) =>
                        s.Technician == null ? "-" : (s.Technician.Name + " " + s.Technician.Surname)))
                .ForMember(dest => dest.TechnicianStatus, opt => opt.MapFrom(x => x.Technician.TechnicianStatus))
                .ForMember(dest=>dest.PhotoPath,opt=>opt.MapFrom(x=>x.PhotoPath));

            // .ReverseMap() metodu çağrıldığında eğer bir entity başka bir entity ile ilişkiliyse, mapping oluşturulurken ilişkili olan entity'nin yeni bir instance'ı oluşturulur. Bunu istemeyiz. Bu yüzden tersine mapping ayrıca yapılmalıdır.

            cfg.CreateMap<FailureViewModel, Failure>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.FailureId));
        }

        private static void CategoryMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Category, CategoryViewModel>().ReverseMap();
        }
    }
}
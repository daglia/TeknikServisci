using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
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

        private static void FailureMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Failure, FailureViewModel>()
                .ForMember(dest=>dest.FailureId, opt => opt.MapFrom(x => x.Id))
                .ForMember(dest => dest.CreatedTime,opt=>opt.MapFrom(x=>x.CreatedDate))
                .ForMember(dest => dest.ClientName,opt=>opt.MapFrom(x=>x.Client.Name))
                .ForMember(dest => dest.ClientSurname, opt => opt.MapFrom(x => x.Client.Surname))
                .ForMember(dest => dest.Technician, opt => opt.MapFrom(x => (x.Technician.Name + " " + x.Technician.Surname)))
                .ForMember(dest => dest.Operator, opt => opt.MapFrom(x => (x.Operator.Name + " " + x.Operator.Surname)))
                .ReverseMap(); 
        }

        private static void CategoryMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Category, CategoryViewModel>().ReverseMap(); 
        }
    }
}
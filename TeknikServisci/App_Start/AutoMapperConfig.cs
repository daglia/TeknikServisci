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
            });

           

        }

        private static void InvoiceMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Operation, InvoiceViewModel>().ReverseMap(); 
        }

        private static void FailureMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Failure, FailureViewModel>().ReverseMap(); 
        }

        private static void CategoryMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Category, CategoryViewModel>().ReverseMap(); 
        }
    }
}
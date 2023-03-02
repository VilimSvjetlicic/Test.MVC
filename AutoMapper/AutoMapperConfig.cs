using AutoMapper;
using Microsoft.Ajax.Utilities;
using MonoTest.MVC.Models;
using MonoTest.MVC.Models.ViewModels;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonoTest.MVC.AutoMapper
{
    public class AutoMapperConfig
    {
        private static Mapper mapper;
        public static Mapper GetMapper()
        {   if (mapper == null) {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<VehicleMake, VehicleMakeVM>();
                    cfg.CreateMap<VehicleModel, VehicleModelVM>()
                        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.Abrv, opt => opt.MapFrom(src => src.Abrv))
                        .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.MakeId));
                });
                mapper = new Mapper(config);

                return mapper;
            }
            else 
            { 
                return mapper; 
            }
            
        }

    }
}
﻿using AutoMapper;
using ConsumerOne.Api.Models;

namespace ConsumerOne.Api.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationViewModel, AppUser>()
                .ForMember(au => au.UserName, map => map.MapFrom(vm => vm.CpfCnpj))
                .ForMember(au => au.Name, map => map.MapFrom(vm => vm.Name));

        }
    }
}
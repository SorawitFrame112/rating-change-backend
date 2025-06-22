using AutoMapper;

using Rtc.Domain.Dtos;
using Rtc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rtc.Appilcation.MappingProfile
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CurrencyDtos, Currency>();
            CreateMap<CurrencyDtos, Currency>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());  
            CreateMap<Currency, CurrencyDtos>();
        }
    }
}

using AutoMapper;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using System.Reflection;

namespace Ordering.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order,OrderDto>().ReverseMap();
        }

    }
}

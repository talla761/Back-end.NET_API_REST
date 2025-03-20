using AutoMapper;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.DTOs;

namespace P7CreateRestApi.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BidList, BidListDTO>().ReverseMap();

            CreateMap<CurvePoint, CurvePointDTO>().ReverseMap();

            CreateMap<Rating, RatingDTO>().ReverseMap();

            CreateMap<RuleName, RuleNameDTO>().ReverseMap();

            CreateMap<Trade, TradeDTO>().ReverseMap();
        }
    }
}

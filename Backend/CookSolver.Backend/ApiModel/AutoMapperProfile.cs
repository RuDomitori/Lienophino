using AutoMapper;
using CookSolver.Data.Entities;

namespace CookSolver.ApiModel;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Meal, ApiMeal>()
            .ForMember(dst => dst.MealTagIds, opt => opt.MapFrom(src => src.Meal2MealTags.Select(x => x.MealTagId)));
        CreateMap<Meal, ApiMeal.WithNavProps>()
            .ForMember(dst => dst.MealTags, opt => opt.MapFrom(src => src.Meal2MealTags.Select(x => x.MealTag)));
        
        CreateMap<MealHistoryItem, ApiMealHistoryItem>()
            .ForMember(dst => dst.Date, opt => opt.MapFrom(x => x.Date.ToDateTime(TimeOnly.MinValue)));

        CreateMap<ApiMealHistoryItem, MealHistoryItem>()
            .ForMember(dst => dst.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)));

        CreateMap<MealTag, ApiMealTag>();
    }
}
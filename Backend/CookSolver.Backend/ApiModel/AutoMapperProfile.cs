using AutoMapper;
using CookSolver.Data.Entities;

namespace CookSolver.ApiModel;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Meal, ApiMeal>();
        CreateMap<MealHistoryItem, ApiMealHistoryItem>()
            .ForMember(dst => dst.Date, opt => opt.MapFrom(x => x.Date.ToDateTime(TimeOnly.MinValue)));

        CreateMap<ApiMealHistoryItem, MealHistoryItem>()
            .ForMember(dst => dst.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)));
    }
}
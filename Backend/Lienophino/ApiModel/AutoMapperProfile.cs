using AutoMapper;
using Lienophino.Data.Entities;

namespace Lienophino.ApiModel;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Meal, ApiMeal>();
        CreateMap<Meal, ApiMeal.WithNavProps>()
            .ForMember(
                dst => dst.MealTags,
                opt =>
                {
                    opt.Condition(src => src.Meal2MealTags != null);
                    opt.MapFrom(src => src.Meal2MealTags.Select(x => x.MealTag));
                })
            .ForMember(
                dst => dst.Ingredients,
                opt =>
                {
                    opt.Condition(src => src.Meal2Ingredients != null);
                    opt.MapFrom(src => src.Meal2Ingredients.Select(x => x.Ingredient));
                });
        
        CreateMap<MealHistoryItem, ApiMealHistoryItem>()
            .ForMember(dst => dst.Date, opt => opt.MapFrom(x => x.Date.ToDateTime(TimeOnly.MinValue)));

        CreateMap<ApiMealHistoryItem, MealHistoryItem>()
            .ForMember(dst => dst.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)));

        CreateMap<MealTag, ApiMealTag>();
        CreateMap<Ingredient, ApiIngredient>();
    }
}
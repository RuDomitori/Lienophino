import {pickByKeys} from "../../utils";
import _ from "lodash";
import ApiMealTag, {IApiMealTag} from "./ApiMealTag";
import ApiIngredient, {IApiIngredient} from "./ApiIngredient";

export interface IApiMeal {
    id: string;
    name: string;
    description: string | null;

    mealTags: IApiMealTag[] | null;
    ingredients: IApiIngredient[] | null;
}

export default class ApiMeal implements IApiMeal {
    id!: string;
    name!: string;
    description!: string | null;

    mealTags!: ApiMealTag[] | null;
    ingredients!: ApiIngredient[] | null;

    constructor(iMeal: IApiMeal) {
        this.with(iMeal);
    }

    public with(iMeal: Partial<IApiMeal>): ApiMeal {
        const item = pickByKeys(iMeal, [
            "id",
            "name",
            "description",
            "mealTags",
            "ingredients"
        ]);

        return _.assign(this, item);
    }

    public static fromJson(json: string | object): ApiMeal {
        const item = typeof json === "string"
            ? JSON.parse(json)
            : json;

        item.mealTags = item.mealTags instanceof Array
            ? item.mealTags.map((x:any) => ApiMealTag.fromJson(x))
            : null;

        item.ingredients = item.ingredients instanceof Array
            ? item.ingredients.map((x:any) => ApiIngredient.fromJson(x))
            : null;

        return new ApiMeal(item as IApiMeal);
    }
}
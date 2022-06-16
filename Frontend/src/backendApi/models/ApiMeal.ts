import {pickByKeys} from "../../utils";
import _ from "lodash";
import ApiMealTag, {IApiMealTag} from "./ApiMealTag";

export interface IApiMeal {
    id: string;
    name: string;
    description: string | null;
    mealTagIds: string[];

    mealTags: IApiMealTag[] | null
}

export default class ApiMeal implements IApiMeal {
    id!: string;
    name!: string;
    description!: string | null;
    mealTagIds!: string[];

    mealTags!: ApiMealTag[] | null;

    constructor(iMeal: IApiMeal) {
        this.with(iMeal);
    }

    public with(iMeal: Partial<IApiMeal>): ApiMeal {
        const item = pickByKeys(iMeal, [
            "id",
            "name",
            "description",
            "mealTagIds",
            "mealTags"
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

        return new ApiMeal(item as IApiMeal);
    }
}
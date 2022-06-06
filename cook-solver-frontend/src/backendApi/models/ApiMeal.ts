import {pickByKeys} from "../../utils";
import _ from "lodash";

export interface IApiMeal {
    id: string;
    name: string;
    description: string | null;
}

export default class ApiMeal implements IApiMeal {
    id!: string;
    name!: string;
    description!: string | null;

    constructor(iMeal: IApiMeal) {
        this.with(iMeal);
    }

    public with(iMeal: Partial<IApiMeal>): ApiMeal {
        const item = pickByKeys(iMeal, [
            "id",
            "name",
            "description"
        ]);

        return _.assign(this, item);
    }

    public static fromJson(json: string | object): ApiMeal {
        const item = typeof json === "string"
            ? JSON.parse(json)
            : json;

        return new ApiMeal(item as IApiMeal);
    }
}
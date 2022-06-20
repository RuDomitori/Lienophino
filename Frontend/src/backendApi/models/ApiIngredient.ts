import {pickByKeys} from "../../utils";
import _ from "lodash";

export interface IApiIngredient {
    id: string;
    name: string;
}

export default class ApiIngredient implements IApiIngredient {
    id!: string;
    name!: string;

    constructor(ingredient: IApiIngredient) {
        this.with(ingredient);
    }

    public with(ingredient: Partial<IApiIngredient>): ApiIngredient {
        const item = pickByKeys(ingredient, [
            "id",
            "name"
        ]);

        return _.assign(this, item);
    }

    public static fromJson(json: string | object): ApiIngredient {
        const item = typeof json === "string"
            ? JSON.parse(json)
            : json;

        return new ApiIngredient(item as IApiIngredient);
    }
}
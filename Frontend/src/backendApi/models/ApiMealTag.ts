import {pickByKeys} from "../../utils";
import _ from "lodash";

export interface IApiMealTag {
    id: string,
    name: string
}

export default class ApiMealTag implements IApiMealTag {
    id!: string;
    name!: string;

    constructor(iMealTag: IApiMealTag) {
        this.with(iMealTag);
    }

    public with(iMealTag: Partial<IApiMealTag>): ApiMealTag {
        const item = pickByKeys(iMealTag, [
            "id",
            "name",
        ]);

        return _.assign(this, item);
    }

    public static fromJson(json: string | object): ApiMealTag {
        const item = typeof json === "string"
            ? JSON.parse(json)
            : json;

        return new ApiMealTag(item as IApiMealTag);
    }
}
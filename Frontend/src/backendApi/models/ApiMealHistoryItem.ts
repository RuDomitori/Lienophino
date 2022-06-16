import {DateTime} from "luxon";
import {pickByKeys} from "../../utils";
import _ from "lodash";

export interface IApiMealHistoryItem {
    mealId: string;
    date: DateTime;
}

export default class ApiMealHistoryItem implements IApiMealHistoryItem {
    mealId!: string;
    date!: DateTime;

    constructor(item: IApiMealHistoryItem) {
        this.with(item);
    }

    public with(iItem: Partial<IApiMealHistoryItem>): ApiMealHistoryItem {
        const item = pickByKeys(iItem, [
            "mealId",
            "date"
        ]);

        return _.assign(this, item);
    }

    public static fromJson(json: string | object): ApiMealHistoryItem {
        const item = typeof json === "string"
            ? JSON.parse(json)
            : json;

        item.date = DateTime.fromISO(item.date);
        return new ApiMealHistoryItem(item as IApiMealHistoryItem);
    }
}
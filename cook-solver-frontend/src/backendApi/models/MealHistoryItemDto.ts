import {DateTime} from "luxon";
import {pickByKeys} from "../../utils";
import _ from "lodash";

export interface IMealHistoryItemDto {
    mealId: string;
    date: DateTime;
}

export default class MealHistoryItemDto implements IMealHistoryItemDto{
    mealId!: string;
    date!: DateTime;

    constructor(dto: IMealHistoryItemDto) {
        this.with(dto);
    }

    public with(iMessage: Partial<IMealHistoryItemDto>): MealHistoryItemDto {
        const item = pickByKeys(iMessage, [
            "mealId",
            "date"
        ]);

        return _.assign(this, item);
    }

    public static fromJson(json: string | object): MealHistoryItemDto {
        const item = typeof json === "string"
            ? JSON.parse(json)
            : json;

        item.date = DateTime.fromISO(item.date);
        return new MealHistoryItemDto(item as IMealHistoryItemDto);
    }
}
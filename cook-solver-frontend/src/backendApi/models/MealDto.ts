import {pickByKeys} from "../../utils";
import _ from "lodash";

export interface IMealDto {
    id: string;
    name: string;
    description: string | null;
}

export default class MealDto implements IMealDto{
    id!: string;
    name!: string;
    description!: string | null;

    constructor(dto: IMealDto) {
        this.with(dto);
    }

    public with(iMessage: Partial<IMealDto>): MealDto {
        const item = pickByKeys(iMessage, [
            "id",
            "name",
            "description"
        ]);

        return _.assign(this, item);
    }

    public static fromJson(json: string | object): MealDto {
        const item = typeof json === "string"
            ? JSON.parse(json)
            : json;

        return new MealDto(item as IMealDto);
    }
}
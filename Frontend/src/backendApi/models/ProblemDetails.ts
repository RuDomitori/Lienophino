import _ from "lodash";
import {pickByKeys} from "../../utils";

export interface IProblemDetails {
    type?: string,
    title?: string,
    status?: number,
    detail?: string
}

export class ProblemDetails implements IProblemDetails {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;

    constructor(iDetails: IProblemDetails) {
        this.with(iDetails);
    }

    public with(iDetails: Partial<IProblemDetails>): ProblemDetails {
        const partDetails = pickByKeys(iDetails, ["title", "detail", "status", "type"]);
        return _.assign(this, partDetails) as ProblemDetails;
    }

    public static fromJson(json: string): ProblemDetails {
        return new ProblemDetails(JSON.parse(json) as IProblemDetails);
    }
}
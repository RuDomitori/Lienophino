import _ from "lodash";
import {ProblemDetails} from "../backendApi/models/ProblemDetails";

export function pickByKeys<T>(object: T | null | undefined, keys: (keyof T)[]) {
    return _.pick(object, keys);
}

export async function extractProblemDetails<T>(
    responsePromise: Promise<Response>,
    onOk: (response: Response) => T
): Promise<T | ProblemDetails> {
    try {
        const response = await responsePromise;
        if(response.ok) return onOk(response);

        return ProblemDetails.fromJson(await response.text());
    }
    catch (e: any) {
        const message = e instanceof Error
            ? e.message
            : e.toString() as string;
        console.error(`Ошибка отправки запроса: ${message}`, e);
        return new ProblemDetails({
            type: "",
            title: "Ошибка отпраки запроса",
            detail: message
        });
    }
}
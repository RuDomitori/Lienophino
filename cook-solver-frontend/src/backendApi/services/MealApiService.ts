import configuration from "../../configuration";
import {extractProblemDetails} from "../../utils";
import ApiMeal from "../models/ApiMeal";

interface GetArgs {

}

interface CreateArgs {
    name: string,
    description: string
}

interface DeleteArgs {
    id: string
}

export default abstract class MealApiService {
    public static async get(args?: GetArgs) {
        const url = new URL(`${configuration.hostApiUrl}/Meals`);

        const responsePromise = fetch(url.toString(), {
            method: "GET",
            headers: {
                'Accept': 'application/json'
            },
            credentials: "include"
        });

        return await extractProblemDetails(responsePromise, async response => {
            const json = await response.json() as Object[];
            return json.map(x => ApiMeal.fromJson(x));
        });
    }

    public static async create(args: CreateArgs) {
        const responsePromise = fetch(`${configuration.hostApiUrl}/Meals`, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(args),
            credentials: "include"
        });

        return await extractProblemDetails(responsePromise, async response => {
            const json = await response.json();
            return ApiMeal.fromJson(json);
        });
    }

    public static async delete(args: DeleteArgs) {
        const responsePromise = fetch(`${configuration.hostApiUrl}/Meals/${args.id}`, {
            method: "DELETE",
            headers: {
                'Accept': 'application/json',
            },
            credentials: "include"
        });

        return await extractProblemDetails(responsePromise, async response => {
            const json = await response.json();
            return ApiMeal.fromJson(json);
        });
    }
}
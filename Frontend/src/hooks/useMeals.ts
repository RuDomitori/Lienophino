import {useEffect, useState} from "react";
import ApiMeal from "../backendApi/models/ApiMeal";
import MealApiService from "../backendApi/services/MealApiService";
import {ProblemDetails} from "../backendApi/models/ProblemDetails";

interface UseMealsArgs {
    includeMealTags?: boolean
}

export default function useMeals(args?: UseMealsArgs) {
    const [state, setState] = useState({
        meals: [] as ApiMeal[],
        loading: false,
        error: null as (string | null)
    });

    function load() {
        if(state.loading) return;

        MealApiService.get({includeMealTags: args?.includeMealTags})
            .then(response => {
                if (response instanceof ProblemDetails) {
                    console.error(response);
                    setState({
                        meals: [],
                        loading: false,
                        error: response.title || response.detail || ""
                    });
                } else {
                    setState({meals: response, loading: false, error: null});
                }
            });
    }

    useEffect(load, []);

    return {
        values: state.meals,
        isLoading: state.loading,
        error: state.error,
        reload() {load();}
    };
}
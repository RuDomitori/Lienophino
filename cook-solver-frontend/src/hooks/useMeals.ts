import {useEffect, useState} from "react";
import MealDto from "../backendApi/models/MealDto";
import MealApiService from "../backendApi/services/MealApiService";
import {ProblemDetails} from "../backendApi/models/ProblemDetails";

export default function useMeals() {
    const [state, setState] = useState({
        meals: [] as MealDto[],
        loading: false,
        error: null as (string | null)
    });

    function load() {
        if(state.loading) return;

        MealApiService.get({})
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
import {FC, useCallback} from "react";
import useMeals from "../../hooks/useMeals";
import {useMealCreatingModal} from "./MealCreatingModal";
import MealCard from "./MealCard";
import MealDto from "../../backendApi/models/MealDto";
import MealApiService from "../../backendApi/services/MealApiService";
import {ProblemDetails} from "../../backendApi/models/ProblemDetails";

const MealsPage: FC = function () {
    const meals = useMeals();
    const handleMealEditing = useCallback((meal:MealDto) => console.log("Edit.", meal), []);
    const handleMealDeleting = useCallback((meal:MealDto) => {
        MealApiService.delete({id: meal.id})
            .then(response => {
                if(response instanceof ProblemDetails)
                    console.error(response);
                else {
                    meals.reload();
                }
            });
    }, [meals]);

    const modal = useMealCreatingModal(() => meals.reload());

    return (
        <div className="container my-3">
            <div className="row my-3">
                <div className="col-auto p-0">
                    <button className="btn btn-primary"
                            onClick={() => modal.show()}
                            type="button">
                        <i className="fa fa-plus" aria-hidden="true"></i> New
                    </button>
                </div>
            </div>
            <div className="g-3 row">
                {meals.values.map(x =>
                    <MealCard meal={x} key={x.id}
                              onEdit={handleMealEditing}
                              onDelete={handleMealDeleting} />
                )}
            </div>
            {modal.element}
        </div>
    );
};

export default MealsPage;
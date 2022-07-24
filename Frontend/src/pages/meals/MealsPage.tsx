import React, {FC, useCallback, useRef} from "react";
import useMeals from "../../hooks/useMeals";
import MealCreatingModal, {MealCreatingFormValues} from "./MealCreatingModal";
import MealCard from "./MealCard";
import ApiMeal from "../../backendApi/models/ApiMeal";
import MealApiService from "../../backendApi/services/MealApiService";
import {ProblemDetails} from "../../backendApi/models/ProblemDetails";
import Container from "@mui/material/Container";
import {Button, Grid} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";

const MealsPage: FC = function () {
    const meals = useMeals({includeMealTags: true});
    const handleMealEditing = useCallback((meal: ApiMeal) => console.log("Edit.", meal), []);
    const handleMealDeleting = useCallback((meal: ApiMeal) => {
        MealApiService.delete({id: meal.id})
            .then(response => {
                if (response instanceof ProblemDetails)
                    console.error(response);
                else {
                    meals.reload();
                }
            });
    }, [meals]);

    const handleMealSave = useCallback((formValues: MealCreatingFormValues) => {
        MealApiService.create(formValues)
            .then(response => {
                if (response instanceof ProblemDetails)
                    console.error(response);
                else {
                    meals.reload();
                }
            });
    }, [meals]);
    const mealCreatingModalRef = useRef<MealCreatingModal>(null);

    return (
        <Container sx={{my: 3}}>
            <Button color="primary" variant="contained" startIcon={<AddIcon/>} sx={{mb: 2}}
                    onClick={() => mealCreatingModalRef.current?.show()}>
                New
            </Button>
            <Grid container spacing={3}>
                {meals.values.map(x =>
                    <MealCard meal={x} key={x.id}
                              onEdit={handleMealEditing}
                              onDelete={handleMealDeleting}/>
                )}
            </Grid>

            <MealCreatingModal ref={mealCreatingModalRef} onSave={handleMealSave}/>
        </Container>
    );
};

export default MealsPage;
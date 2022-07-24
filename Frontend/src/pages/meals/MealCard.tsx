import ApiMeal from "../../backendApi/models/ApiMeal";
import {FC, useCallback} from "react";
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import {Card, CardActions, CardContent, Grid, IconButton} from "@mui/material";
import Typography from "@mui/material/Typography";

interface MealCardProps {
    meal: ApiMeal,
    onEdit: (meal: ApiMeal) => void,
    onDelete: (meal: ApiMeal) => void
}

const MealCard: FC<MealCardProps> = function ({meal, onEdit, onDelete}) {
    const handleEditClick = useCallback(() => onEdit(meal), [meal, onEdit]);
    const handleDeleteClick = useCallback(() => onDelete(meal), [meal, onDelete]);

    return (
        <Grid item xs={4}>
            <Card>
                <CardContent>
                    <Typography variant="h5" component="div">
                        {meal.name}
                    </Typography>
                    <Typography color="text.secondary">
                        {meal.mealTags?.map(x => x.name).join(", ")}
                    </Typography>
                    <Typography>
                        {meal.description}
                    </Typography>

                </CardContent>
                <CardActions>
                    <IconButton size="small" color="primary" onClick={handleEditClick}>
                        <EditIcon/>
                    </IconButton>
                    <IconButton size="small" color="error" onClick={handleDeleteClick}>
                        <DeleteIcon/>
                    </IconButton>
                </CardActions>
            </Card>
        </Grid>
    );
};

export default MealCard;

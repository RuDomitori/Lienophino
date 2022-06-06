import ApiMeal from "../../backendApi/models/ApiMeal";
import {FC, useCallback} from "react";

interface MealCardProps {
    meal: ApiMeal,
    onEdit: (meal: ApiMeal) => void,
    onDelete: (meal: ApiMeal) => void
}

const MealCard: FC<MealCardProps> = function({meal, onEdit, onDelete}) {
    const handleEditClick = useCallback(() => onEdit(meal), [meal, onEdit]);
    const handleDeleteClick = useCallback(() => onDelete(meal), [meal, onDelete]);

    return (
        <div className="card shadow-sm">
            <div className="card-body">
                <h5 className="card-title">{meal.name}</h5>
                <p className="card-text">{meal.description}</p>
                <div className="btn-group btn-group-sm me-2">
                    <button className="btn btn-outline-secondary" onClick={handleEditClick}>
                        <i className="fa fa-pencil" aria-hidden="true"></i>
                    </button>
                    <button className="btn btn-outline-danger" onClick={handleDeleteClick}>
                        <i className="fa fa-times" aria-hidden="true"></i>
                    </button>
                </div>
            </div>
        </div>
    );
};

export default MealCard;

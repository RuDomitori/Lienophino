import MealDto from "../../backendApi/models/MealDto";
import {FC, useCallback, useState} from "react";
import {Formik} from "formik";
import {object, string} from "yup";
import MealApiService from "../../backendApi/services/MealApiService";
import {ProblemDetails} from "../../backendApi/models/ProblemDetails";
import {Modal} from "react-bootstrap";
import classNames from "classnames";

interface NewMealCreatingModalProps {
    show: boolean,
    onHide: () => void,
    onSave: (meal: MealDto) => void
}

const MealCreatingModal: FC<NewMealCreatingModalProps> = function ({show, onHide, onSave}) {
    return (
        <Formik initialValues={{name:"", description:""}}
                validationSchema={object({
                    name: string().required("Name is required"),
                    description: string().required("Description is required")
                })}
                onSubmit={(values, formikHelpers) => {
                    MealApiService.create(values)
                        .then(response => {
                            if(response instanceof ProblemDetails)
                                console.error(response);
                            else {
                                onSave(response);
                            }

                            formikHelpers.setSubmitting(false);
                        });
                }}>
            {({
                  values,
                  errors, submitForm,
                  handleChange, handleSubmit,
                  handleBlur
              }) => (
                <Modal show={show}
                       onHide={onHide}
                       backdrop="static"
                       centered>
                    <Modal.Header closeButton>
                        <Modal.Title>New meal</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <form onSubmit={handleSubmit}>
                            <div className="col-12 mb-3">
                                <label className="form-label">Name</label>
                                <input className={classNames("form-control", {"is-invalid": errors?.name})}
                                       autoFocus type="text"
                                       value={values.name} name="name"
                                       onBlur={handleBlur} onChange={handleChange}/>
                                <div className="invalid-feedback">
                                    {errors?.name}
                                </div>
                            </div>

                            <div className="col-12 mb-3">
                                <label className="form-label">Description</label>
                                <input className={classNames("form-control", {"is-invalid": errors?.description})}
                                       type="text" autoFocus
                                       value={values.description} name="description"
                                       onBlur={handleBlur} onChange={handleChange}/>
                                <div className="invalid-feedback">
                                    {errors?.description}
                                </div>
                            </div>
                        </form>
                    </Modal.Body>
                    <Modal.Footer>
                        <button className="btn btn-outline-danger" onClick={onHide}>
                            Cancel
                        </button>
                        <button className="btn btn-primary" onClick={submitForm}>
                            Save
                        </button>
                    </Modal.Footer>
                </Modal>
            )}
        </Formik>
    );
};

export function useMealCreatingModal(onSave?: (meal: MealDto) => void) {
    const [showModal, setShowModal] = useState(false);
    const handleHiding = useCallback(() => setShowModal(false),[]);
    const handleSaving = useCallback((meal: MealDto) => {
        setShowModal(false);
        onSave?.(meal);
    }, [onSave]);

    return {
        element: <MealCreatingModal show={showModal} onHide={handleHiding} onSave={handleSaving} />,
        show() {setShowModal(true);},
        hide() {setShowModal(false);}
    };
}

export default MealCreatingModal;
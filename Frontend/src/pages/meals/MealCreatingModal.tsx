import React from "react";
import {Formik} from "formik";
import {object, string} from "yup";
import {Modal} from "react-bootstrap";
import classNames from "classnames";


interface MealCreatingModalProps {
    onHide?: () => void,
    onSave: (meal: MealCreatingFormValues) => void | Promise<void>
}

interface MealCreatingModalState {
    show: boolean
}

export interface MealCreatingFormValues {
    name: string,
    description: string
}

export default class MealCreatingModal extends React.Component<MealCreatingModalProps, MealCreatingModalState> {
    constructor(props: MealCreatingModalProps) {
        super(props);
        this.state = {
            show: false
        };

        this.hide = this.hide.bind(this);
        this.show = this.show.bind(this);
    }

    show() {
        this.setState({show: true});
    }

    hide() {
        this.setState({show: false});
        this.props.onHide?.();
    }

    private async save(values: MealCreatingFormValues): Promise<void> {
        const promise = this.props.onSave(values);
        if(promise) {
            await promise;
        }
        this.hide();
    }

    private static getFormInitValues() {
        return {name:"", description:""};
    }

    render() {
        return (
            <Formik initialValues={MealCreatingModal.getFormInitValues()}
                    validationSchema={object({
                        name: string().required("Name is required"),
                        description: string()
                    })}
                    onSubmit={(values, formikHelpers) => {
                        this.save(values).finally(() => formikHelpers.setSubmitting(false));
                    }}>
                {({
                      values,
                      errors, submitForm,
                      handleChange, handleSubmit,
                      handleBlur
                  }) => (
                    <Modal show={this.state.show}
                           onHide={this.hide}
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
                            <button className="btn btn-outline-danger" onClick={this.hide}>
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
    }
}
import React from "react";
import {Formik} from "formik";
import {object, string} from "yup";
import {Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField} from "@mui/material";
import Box from "@mui/material/Box";


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

const validationSchema = object({
    name: string().required("Name is required"),
    description: string()
});

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
        if (promise) {
            await promise;
        }
        this.hide();
    }

    private static getFormInitValues() {
        return {name: "", description: ""};
    }

    render() {
        return (
            <Formik initialValues={MealCreatingModal.getFormInitValues()}
                    validationSchema={validationSchema}
                    onSubmit={(values, formikHelpers) => {
                        this.save(values).finally(() => formikHelpers.setSubmitting(false));
                    }}>
                {({
                      values,
                      errors, submitForm,
                      handleChange, handleSubmit,
                      handleBlur
                  }) => (
                    <Dialog open={this.state.show} onClose={this.hide}>
                        <DialogTitle>New meal</DialogTitle>
                        <DialogContent>
                            <Box
                                component="form"
                                sx={{
                                    '& .MuiTextField-root': { m: 1 },
                                }}
                                onSubmit={handleSubmit}
                            >
                                <div>
                                    <TextField
                                               label="Name"
                                               autoFocus type="text"
                                               error={errors?.name !== undefined}
                                               helperText={errors?.name}
                                               value={values.name} name="name"
                                               onBlur={handleBlur} onChange={handleChange}
                                    />
                                </div>
                                <div>
                                    <TextField
                                        label="Description"
                                        autoFocus type="text"
                                        error={errors?.description !== undefined}
                                        helperText={errors?.description}
                                        value={values.description} name="description"
                                        onBlur={handleBlur} onChange={handleChange}
                                    />
                                </div>
                            </Box>
                        </DialogContent>
                        <DialogActions>
                            <Button color="error" onClick={this.hide}>Cancel</Button>
                            <Button color="primary" onClick={submitForm}>Save</Button>
                        </DialogActions>
                    </Dialog>
                )}
            </Formik>
        );
    }
}
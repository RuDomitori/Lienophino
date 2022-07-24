import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import configuration from "../configuration";
import {Button} from "@mui/material";
import {NavLink} from "react-router-dom";

const pages = [
    {
        title: 'Meals',
        path: "Meals"
    },
    {
        title: 'Meal tags',
        path: "MealTags"
    },
    {
        title: 'Ingredients',
        path: "Ingredients"
    }
];

const Header = () => {
    return (
        <AppBar position="static">
            <Container maxWidth={false}>
                <Toolbar disableGutters variant="dense">
                    <Typography
                        variant="h6"
                        noWrap
                        component="span"
                        sx={{
                            mr: 2,
                            display: 'flex',
                            fontFamily: 'monospace',
                            fontWeight: 700,
                            letterSpacing: '.3rem',
                        }}
                    >
                        {configuration.applicationTitle}
                    </Typography>

                    <Box sx={{flexGrow: 1, display: 'flex'}}>
                        {pages.map((page) => (
                            <Button component={NavLink} to={page.path} key={page.path}
                                    sx={{
                                        color: 'white', display: 'block',
                                        fontWeight: 600, fontFamily: 'monospace', textTransform: "none",
                                        "&.active": {backgroundColor: "info.main"},
                                        minWidth: 0
                                    }}
                            >
                                {page.title}
                            </Button>
                        ))}
                    </Box>
                </Toolbar>
            </Container>
        </AppBar>
    );
};
export default Header;

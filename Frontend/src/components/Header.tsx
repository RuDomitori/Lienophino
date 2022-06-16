import React, {FC} from "react";
import {NavLink} from "react-router-dom";
import configuration from "../configuration";

interface HeaderProps {
    className: string
}

const Header: FC<HeaderProps> = function (props) {
    const navLinkClassSelector = ({isActive}: { isActive: boolean }) => isActive
        ? "nav-link px-2 text-warning"
        : "nav-link px-2 text-white";

    return (
        <header className={"w-100 p-2 bg-dark text-white " + props.className}>
            <nav className="container">
                <div className="d-flex col-12 flex-wrap align-items-center justify-content-center">
                    <span className="navbar-brand">{configuration.applicationTitle}</span>
                    <ul className="nav me-auto mb-2 justify-content-center mb-md-0">
                        <li key="/Meals">
                            <NavLink to="/Meals" className={navLinkClassSelector}>Meals</NavLink>
                        </li>
                        {/*<li key="/MealHistory">*/}
                        {/*    <NavLink to="/MealHistory" className={navLinkClassSelector}>History</NavLink>*/}
                        {/*</li>*/}
                    </ul>
                </div>
            </nav>
        </header>
    );
};

export default Header;
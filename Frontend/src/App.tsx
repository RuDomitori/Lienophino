import React from 'react';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Header from "./components/Header";
import MealsPage from "./pages/meals/MealsPage";

function App() {
  return (
      <BrowserRouter>
        <div className="d-flex h-100 flex-column">
          <Header className="col-auto"/>
          <div className="h-100 w-100 overflow-auto">
            <Routes>
              <Route path="/Meals" element={<MealsPage/>}/>
            </Routes>
          </div>
        </div>
      </BrowserRouter>
  );
}

export default App;

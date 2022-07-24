import React from 'react';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import MealsPage from "./pages/meals/MealsPage";
import Header from "./components/Header";

function App() {
  return (
      <BrowserRouter>
          <Header/>
          <Routes>
              <Route path="/Meals" element={<MealsPage/>}/>
          </Routes>
      </BrowserRouter>
  );
}

export default App;

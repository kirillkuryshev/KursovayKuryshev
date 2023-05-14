import React from 'react';
import * as ReactDOMClient from 'react-dom/client';
import ReactDOM from 'react-dom';
import AppAdmin from './appAdmin.jsx';
import AppUser from './appUser.jsx';
import { Route, Routes } from "react-router-dom"
import Theme from "./theme"
import Login from "./login"
import Register from "./register"

const App = () => { // основное окно программы
    return (<React.Fragment>
        <Routes> { /*навигация*/}
            <Route path="/admin/*" element={<AppAdmin />} />
            <Route path='/user/*' element={<AppUser />} />
            <Route path='/register' element={<div> <Theme /> <Register /> </div>} />
            <Route path='/login' element={<div> <Theme /> <Login /> </div>} />
            <Route path='/*' element={<div> <Theme /> <Login /> </div>} />
        </Routes>
    </React.Fragment>);
}

export default App;
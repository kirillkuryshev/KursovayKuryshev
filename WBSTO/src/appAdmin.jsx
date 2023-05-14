import React from 'react';
import * as ReactDOMClient from 'react-dom/client';
import ReactDOM from 'react-dom';
import { InitializeAdminTheme, InitializeFooter } from './adminTheme.jsx';
import { InitializeHalts } from './halt.jsx';
import { InitializeCruises } from './cruise.jsx';
import { InitializeRoutes } from './route.jsx';
import { Route, Routes } from "react-router-dom"

const AppAdmin = () => { // навигация для администратора
    return (<React.Fragment>
        <InitializeAdminTheme/>
        <Routes>
            <Route path="/halts" element={<InitializeHalts/>} />
            <Route path='/cruises' element={<InitializeCruises />} />
            <Route path='/route' element={<InitializeRoutes />} />
            <Route path='/*' element={<InitializeHalts/>}/>
        </Routes>
    </React.Fragment>);
}

export default AppAdmin;
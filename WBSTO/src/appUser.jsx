import React from 'react';
import * as ReactDOMClient from 'react-dom/client';
import ReactDOM from 'react-dom';
import UserTheme from './userTheme'
import { Route, Routes } from "react-router-dom"
import Return from './return.jsx'
import Search from './search.jsx'

const AppUser = () => { // навигация для пользователя
    return (<React.Fragment>
        < UserTheme/>
        <Routes>
            <Route path='/search' element={<Search />} />
            <Route path='/return' element={<Return />} />
            <Route path='/*' element={<Search />} />
        </Routes>
    </React.Fragment>);
}

export default AppUser;
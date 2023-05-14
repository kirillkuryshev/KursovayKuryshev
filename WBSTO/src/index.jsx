import React from 'react';
import * as ReactDOMClient from 'react-dom/client';
import ReactDOM from 'react-dom';
import { BrowserRouter } from "react-router-dom";
import { InitializeAdminTheme, InitializeFooter } from './adminTheme.jsx';
import App from './app.jsx'
import ErrorBoundary from './errorBoundary'

const appRoot = ReactDOMClient.createRoot(document.getElementById('app')); // начальная страница

{
    appRoot.render(<BrowserRouter> {/*навигация*/}
        <ErrorBoundary> {/*обработка исключений*/}
            <App /><InitializeFooter />
        </ErrorBoundary>
    </BrowserRouter>);
}

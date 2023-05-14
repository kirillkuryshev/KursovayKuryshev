import React from 'react';
import * as ReactDOMClient from 'react-dom/client';
import ReactDOM from 'react-dom';
import Theme from "./theme"
import { InitializeAdminTheme, InitializeFooter } from './adminTheme.jsx';

class ErrorBoundary extends React.Component { // реализация Error Boundary
    constructor(props) {
        super(props);
        this.state = { error: "" };
    }

    componentDidCatch(error) {
        this.setState({ error: `${error.name}: ${error.message}` });
    }

    render() {
        const { error } = this.state;
        if (error) {
            return (
                <>
                    <Theme />
                    <div className="container">В ходе работы произошла ошибка:<br/>{error}</div>
                    <InitializeFooter/>
                </>
            );
        } else {
            return <>{this.props.children}</>;
        }
    }
}

export default ErrorBoundary;
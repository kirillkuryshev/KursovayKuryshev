import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import * as ReactDOMClient from 'react-dom/client';
import bookmark from './image/bookmark-check.svg';

const Theme = () => { // основная шапка без кнопок навигации
    return (
        <div className="px-3 py-2 bg-light text-black">
            <div className="container">
                <div className="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                    <a href="/" className="d-flex align-items-center my-2 my-lg-0 me-lg-auto text-white text-decoration-none">
                        <img className="bi me-2" src={bookmark} alt="Bootstrap" width="32" height="32" />
                        <span className="fs-4 text-black">With Bootstrap | Автовокзал онлайн</span>
                    </a>
                </div>
            </div>
        </div>);
}

export default Theme
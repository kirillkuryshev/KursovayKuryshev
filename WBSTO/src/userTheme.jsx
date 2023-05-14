import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import * as ReactDOMClient from 'react-dom/client';
import { useNavigate, NavLink } from "react-router-dom"
import calendar from './image/calendar.svg';
import search from './image/search.svg';
import retur from './image/return.svg';
import arrow from './image/box-arrow-right.svg';
import bookmark from './image/bookmark-check.svg';

import axios from 'axios';

const UserTheme = () => { // шапка для пользователя
    let navigate = useNavigate();

    const Exit = () => {
        axios.post('http://localhost:5000/api/Account/' + 'LogOff', {}, { withCredentials: true })
            .then((response) => {
                if (response.status == 200) {
                    navigate("../login", { replace: true });
                }
            })
            .catch((response) => {
                throw new Error("Ошибка выхода из аккаунта");
            });
    }

    return (
        <div className="px-3 py-2 bg-light text-black">
            <div className="container">
                <div className="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                    <a href="/" className="d-flex align-items-center my-2 my-lg-0 me-lg-auto text-white text-decoration-none">
                        <img className="bi me-2" src={bookmark} alt="Bootstrap" width="32" height="32" />
                        <span className="fs-4 text-black">With Bootstrap | Автовокзал онлайн</span>
                    </a>
                    <ul className="nav col-12 col-lg-auto my-2 justify-content-center my-md-0 text-small">
                        <li>
                            <NavLink to={'/user/search'} className="nav-link text-black">
                                <img className="bi d-block mx-auto mb-1" src={search} alt="" width="24" height="24" />
                                Поиск
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to={'/user/return'} className="nav-link text-black">
                                <img className="bi d-block mx-auto mb-1" src={retur} alt="" width="24" height="24" />
                                Возврат
                            </NavLink>
                        </li>
                        <li>
                            <a className="nav-link text-black">
                                <img className="bi d-block mx-auto mb-1" src={arrow} alt="" width="24" height="24" onClick={Exit} />
                                Выход
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>);
}

export default UserTheme;
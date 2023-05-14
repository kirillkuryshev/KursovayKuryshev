import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import * as ReactDOMClient from 'react-dom/client';
import { useNavigate, NavLink } from "react-router-dom"
import calendar from './image/calendar-plus.svg';
import bookmark from './image/bookmark-check.svg';
import activity from './image/activity.svg';
import building from './image/building.svg';
import arrow from './image/box-arrow-right.svg';

import axios from 'axios';

const Initialize = () => // header для администратора с навигацией по доступным страницам
{
    let navigate = useNavigate();

    const Exit = () => { // выход из аккаунта
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
                            <NavLink to={'/admin/route'} className="nav-link text-black">
                                <img className="bi d-block mx-auto mb-1" src={calendar} alt="" width="24" height="24" />
                                Маршруты
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to={'/admin/cruises'} className="nav-link text-black">
                                <img className="bi d-block mx-auto mb-1" src={activity} alt="" width="24" height="24" />
                                Рейсы
                            </NavLink>
                        </li>
                        <li>
                            <NavLink to={'/admin/halts'} className="nav-link text-black">
                                <img className="bi d-block mx-auto mb-1" src={building} alt="" width="24" height="24" />
                                Остановки
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

function Footer() { // общий для всех footer
    return (<div className="container">
        <footer className="d-flex flex-wrap justify-content-between align-items-center py-3 my-4 border-top">
            <div className="col-md-4 d-flex align-items-center">
                <span className="text-muted">© 2022 Сизяков Иван 3-42</span>
            </div>
        </footer>
    </div>);
}

export function InitializeAdminTheme() { return Initialize(); }
export function InitializeFooter() { return Footer(); }
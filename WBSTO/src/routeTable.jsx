import React, { useEffect, useState } from 'react';
import { Table, Space, TimePicker } from 'antd'
import 'antd/dist/antd.css';
import { HiddenToString, SetOption } from './helpful'
import axios from 'axios';
import { useNavigate, NavLink } from 'react-router-dom';
const uri = "http://localhost:5000/api/RouteHalt/";

const RouteTable = ({ halts, setHalts }) => { // отображение таблицы остановок маршрута с
    // возможностью редактирования
    let navigate = useNavigate();

    const removeHalt = (removeId) => { // скрытие остановки маршрута
        let bufHalts = Object.assign([], halts);
        bufHalts.filter(item => item.routeHaltId == removeId)[0].hidden = bufHalts.filter(item =>
            item.routeHaltId == removeId)[0].hidden == 0 ? 1 : 0; // меняет статус на противоположный
        setHalts(bufHalts);
    };

    const deleteItem = (haltId) => { // отправка delete запроса, получает id элемента для удаления
        axios.delete(uri + haltId)
            .then((response) => {
                response.status = 204 ? removeHalt(haltId) : null;
            })
            .catch((response) => {
                if (response.status === 401) {
                    navigate("../../login", { replace: true });
                }
                else {
                    throw new Error("Ошибка смены видимости остановки");
                }
            });
    };

    const updateHalts = (halt) => { // обновление рейса, получает рейс с примененными изменениями
        let bufHalts = Object.assign([], cruises);
        bufHalts.filter(item => item.cruiseId === halt.cruiseId)[0].time = halt.time;
        bufHalts.filter(item => item.cruiseId === halt.cruiseId)[0].cost = halt.cost;
        setCruises(bufHalts);
    };

    const handleChangeTime = event => { // изменение состояния поля ввода
        //соответствующего поля ввода в таблице
        let bufHalts = Object.assign([], halts);
        bufHalts.filter(item => item.routeHaltId == event.target.form.id)[0].time = event.target.value;
        setHalts(bufHalts);
    }

    const handleChangeCost = event => { // изменение состояния поля ввода
        //соответствующего поля ввода в таблице
        let bufHalts = Object.assign([], halts);
        bufHalts.filter(item => item.routeHaltId == event.target.form.id)[0].cost = event.target.value;
        setHalts(bufHalts);
    }


    const handleSubmit = (e) => { // обновление остановки
        e.preventDefault();
        const time = Number(e.target.elements.time.value);
        const cost = Number(e.target.elements.cost.value);
        const id = Number(e.target.id);
        var halt = halts.filter(i => i.routeHaltId === id)[0];
        halt.time = time;
        halt.cost = cost;
        axios.put(uri + id, halt, { withCredentials: true }) // отправка запроса
            .then((response) => {
                response.status = 200 ? updateHalt(halt) : null;
            })
            .catch((response) => {
                if (response.status === 401) {
                    navigate("../../login", { replace: true });
                }
                else {
                    throw new Error("Ошибка обновления рейса");
                }
            });
    };

    const columns = [
        {
            title: 'Остановка',
            dataIndex: 'halt',
            key: 'halt',
            render: (halt) => <label>{halt.locality_model.locality_name} - {halt.adress}</label>
        },
        {
            title: 'Стоимость',
            dataIndex: 'cost',
            key: 'cost',
            render: (cost, record) => <form id={record.routeHaltId} onSubmit={handleSubmit}>
                <input name="cost" form={record.routeHaltId} value={cost}
                    onChange={handleChangeCost} /></form>
        },
        {
            title: 'Время проезда от начала (мин.)',
            dataIndex: 'time',
            key: 'time',
            render: (time, record) => <input name="time" form={record.routeHaltId} value={time}
                onChange={handleChangeTime} />
        },
        {
            title: 'Статус',
            dataIndex: 'hidden',
            key: 'hidden',
            onFilter: (value, record) => record.hidden == value,
            render: hidden => <HiddenToString hidden={hidden} />
        },
        {
            title: 'Действия',
            key: 'action',
            render: (text, record) => (
                <Space size="middle">
                    <button form={record.routeHaltId} type="submit" name="submit" className="btn btn-warning">
                        <i className='fa fa-edit'></i></button> {/* обновление*/}
                    <button className='btn btn-danger' onClick={(e) => deleteItem(record.routeHaltId)}>
                        <i className='fa fa-trash-alt'></i></button> {/* скрытие*/}
                </Space>
            ),
        },
    ];

    return (
        <React.Fragment>
            <Table dataSource={halts} columns={columns} />
        </React.Fragment>
    );
}

export default RouteTable;
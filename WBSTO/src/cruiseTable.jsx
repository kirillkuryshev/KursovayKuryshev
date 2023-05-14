import React, { useEffect, useState } from 'react';
import { Table, Space, TimePicker } from 'antd'
import 'antd/dist/antd.css';
import { HiddenToString, SetOption } from './helpful'
import moment from 'moment';
import axios from 'axios';
import { useNavigate, NavLink } from 'react-router-dom';
const uri = "http://localhost:5000/api/Cruise/";

const CruiseTable = ({ cruises, setCruises, days }) => { // отображение таблицы рейсов с
    // возможностью редактирования
    let navigate = useNavigate();
    let daysFilter = [];
    for (let i = 0; i < days.length; i++) {
        daysFilter[i] = {
            text: days[i].day,
            value: days[i].id,
        }
    }

    const removeHalt = (removeId) => { // "удаление" рейса
        let bufHalts = Object.assign([], cruises);
        bufHalts.filter(item => item.cruiseId === removeId)[0].hidden = bufHalts.filter(item =>
            item.cruiseId === removeId)[0].hidden == 0 ? 1 : 0; // меняет статус на противоположный
        setCruises(bufHalts);
    };

    const deleteItem = (cruiseId) => { // отправка delete запроса, получает id элемента для удаления
        axios.delete(uri + cruiseId)
            .then((response) => {
                response.status = 204 ? removeHalt(cruiseId) : null;
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

    const updateCruise = (cruise) => { // обновление рейса, получает рейс с примененными изменениями
        let bufHalts = Object.assign([], cruises);
        bufHalts.filter(item => item.cruiseId === cruise.cruiseId)[0].day = cruise.day;
        setCruises(bufHalts);
    };

    const handleChange = event => { // изменение состояния поля количество мест у элемента при изменении 
        //соответствующего поля ввода в таблице
        let bufHalts = Object.assign([], cruises);
        bufHalts.filter(item => item.cruiseId == event.target.form.id)[0].places = event.target.value;
        setCruises(bufHalts);
    }

    const handleChangeTime = event => { // изменение состояния поля время отправки при изменении 
        //соответствующего поля ввода в таблице
        let bufHalts = Object.assign([], cruises);
        bufHalts.filter(item => item.cruiseId == event.target.form.id)[0].time = event.target.value;
        setCruises(bufHalts);
    }

    const handleSubmit = (e) => { // обновление строки с рейсом
        e.preventDefault();
        const day = e.target.elements.day.value;
        const cruiseId = e.target.id;
        const time = e.target.elements.time.value;
        const places = e.target.elements.places.value;
        let cruise = cruises.filter(i => i.cruiseId == cruiseId)[0];
        cruise.day = days.filter(i => i.id = day)[0];
        cruise.time = time;
        cruise.places = Number(places);
        axios.put(uri + cruiseId, cruise) // отправка запроса
            .then((response) => {
                response.status = 200 ? updateCruise(cruise) : null;
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
            title: 'Номер',
            dataIndex: 'cruiseId',
            key: 'cruiseId',
            sorter: (a, b) => a.cruiseId - b.cruiseId,
        },
        {
            title: 'День недели',
            dataIndex: 'day',
            key: 'day',
            filters: daysFilter,
            onFilter: (value, record) => record.day.id == value,
            sorter: (a, b) => a.day.id - b.day.id,
            render: (day, record) =>
                <select form={record.cruiseId} name="day">
                    {daysFilter.map(({ text, value }) => (
                        <SetOption value={value} item={text} sItem={day.id} />
                    ))}
                </select>,
        },
        {
            title: 'Мест',
            dataIndex: 'places',
            key: 'places',
            render: (places, record) => <form id={record.cruiseId} onSubmit={handleSubmit}>
                <input name="places" form={record.cruiseId} value={places} onChange={handleChange} /></form>
        },
        {
            title: 'Время отправки',
            dataIndex: 'time',
            key: 'time',
            render: (time, record) => <input type="time" name="time" form={record.cruiseId} value={time} onChange={handleChangeTime} />
        },
        {
            title: 'Статус',
            dataIndex: 'hidden',
            key: 'hidden',
            filters: [
                {
                    text: 'Виден',
                    value: 0,
                },
                {
                    text: 'Скрыт',
                    value: 1,
                },
            ],
            onFilter: (value, record) => record.hidden == value,
            render: hidden => <HiddenToString hidden={hidden} />
        },
        {
            title: 'Действия',
            key: 'action',
            render: (text, record) => (
                <Space size="middle">
                    <button form={record.cruiseId} type="submit" name="submit" className="btn btn-warning">
                        <i className='fa fa-edit'></i></button> {/* обновление*/}
                    <button className='btn btn-danger' onClick={(e) => deleteItem(record.cruiseId)}>
                        <i className='fa fa-trash-alt'></i></button> {/* удаление*/}
                </Space>
            ),
        },
    ];

    return (
        <React.Fragment>
            <Table dataSource={cruises} columns={columns} />
        </React.Fragment>
    );
}

export default CruiseTable;
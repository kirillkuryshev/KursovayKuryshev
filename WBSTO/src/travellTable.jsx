import React, { useEffect, useState } from 'react';
import { Table, Space } from 'antd'
import 'antd/dist/antd.css';
import { HiddenToString, SetTablesLocalities } from './helpful'
import { useNavigate, NavLink } from 'react-router-dom';
import axios from 'axios';
import Buy from './buy'

// таблица для отображения подходящих рейсов
const TravellTable = ({ travells, getTravells }) => {

    const columns = [ // столбцы таблицы
        {
            title: 'Номер маршрута',
            dataIndex: 'routeId',
            key: 'routeId',
            sorter: (a, b) => a.routeId - b.routeId,
        },
        {
            title: 'Время отправки',
            dataIndex: 'time',
            key: 'time',
            sorter: (a, b) => a.time.localeCompare(b.time),
        },
        {
            title: 'Стоимость',
            dataIndex: 'cost',
            key: 'cost',
            sorter: (a, b) => a.cost - b.cost,
        },
        {
            title: 'Свободно мест',
            dataIndex: 'places',
            key: 'places',
            sorter: (a, b) => a.places.length - b.places.length,
            onFilter: (value, record) => record.hidden == value,
            render: places => <div>{
                places.length
            }</div>
        },
        {
            title: 'Действия',
            key: 'action',
            render: (text, record) => (
                <Buy travell={record} getTravells={getTravells} />
            ),
        },
    ];

    return (
        <React.Fragment>
            <Table dataSource={travells} columns={columns} />
        </React.Fragment>
    );
}

export default TravellTable;
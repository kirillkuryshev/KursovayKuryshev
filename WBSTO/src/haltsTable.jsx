import React, { useEffect, useState } from 'react';
import { Table, Space } from 'antd'
import 'antd/dist/antd.css';
import { HiddenToString, SetTablesLocalities } from './helpful'
import { useNavigate, NavLink } from 'react-router-dom';
import axios from 'axios';

const uri = "http://localhost:5000/api/Halt/";

// получает массив населенных пунктов и остановок, useState для него, отображает таблицу остановок с возможностью
// редактирования
const HaltTable = ({ halts, setHalts, localities }) => {
    let navigate = useNavigate();

    const removeHalt = (removeId) => { // "удаление" остановки
        let bufHalts = Object.assign([], halts);
        bufHalts.filter(item => item.halt_id === removeId)[0].hidden = bufHalts.filter(item =>
            item.halt_id === removeId)[0].hidden == 0 ? 1 : 0; // меняет статус на противоположный
        setHalts(bufHalts);
    };

    const deleteItem = (haltId) => { // отправка delete запроса, получает id элемента для удаления
        axios.delete(uri + haltId, {}, { withCredentials: true })
            .then((response) => {
                response.status = 204 ? removeHalt(haltId) : null;
            })
            .catch((response) => {
                if (response.status === 401) {
                    navigate("../../login", { replace: true });
                }
                else {
                    throw new Error("Ошибка удаления остановки");
                }
            });
    };

    const updateHalt = (halt) => { // обновление остановки, изменяется адрес и населенный пункт, 
        //получает остановку с примененными изменениями
        let bufHalts = Object.assign([], halts);
        bufHalts.filter(item => item.halt_id === halt.halt_id)[0].locality = halt.locality;
        setHalts(bufHalts);
    };

    const handleChange = event => { // изменение состояния поля адрес у элемента при изменении 
        //соответствующего поля ввода в таблице
        let bufHalts = Object.assign([], halts);
        bufHalts.filter(item => item.halt_id == event.target.form.id)[0].adress = event.target.value;
        setHalts(bufHalts);
    }

    const handleSubmit = (e) => { // обновление строки
        e.preventDefault();
        const adress = e.target.elements.adress.value;
        const halt_id = e.target.id;
        const locality = e.target.elements.locality.value;
        let halt = {
            halt_id: Number(halt_id), hidden: halts.filter(item =>
                item.halt_id == halt_id)[0].hidden, adress: adress, locality_model: localities.filter(
                    item => item.locality_id == locality)[0]
        }; // обновляемый элемент
        axios.put(uri + halt_id, halt, { withCredentials: true }) // отправка запроса
            .then((response) => {
                response.status = 200 ? updateHalt(halt) : null;
            })
            .catch((response) => {
                if (response.status === 401) {
                    navigate("../../login", { replace: true });
                }
                else {
                    throw new Error("Ошибка обновления остановки");
                }
            });
    };

    const columns = [ // столбцы таблицы
        {
            title: 'Номер',
            dataIndex: 'halt_id',
            key: 'halt_id',
            sorter: (a, b) => a.halt_id - b.halt_id,
        },
        {
            title: 'Название',
            dataIndex: 'adress',
            key: 'adress',
            sorter: (a, b) => a.adress.localeCompare(b.adress),
            render: (adress, record) => <form id={record.halt_id} onSubmit={handleSubmit}>
                <input name="adress" form={record.halt_id} value={adress} onChange={handleChange}/></form>
        },
        {
            title: 'Адрес',
            dataIndex: 'locality_model',
            key: 'locality_name',
            sorter: (a, b) => a.locality_model.locality_name.localeCompare(b.locality_model.locality_name),
            render: (value, record) =>
                <select form={record.halt_id} name="locality">
                    <SetTablesLocalities //получение всех населенных пунктов как option
                        locality_model={value}
                        localities={localities} />
                </select>,
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
                    <button form={record.halt_id} type="submit" name="submit" className="btn btn-warning">
                        <i className='fa fa-edit'></i></button> {/* обновление*/}
                    <button className='btn btn-danger' onClick={(e) => deleteItem(record.halt_id)}>
                        <i className='fa fa-trash-alt'></i></button> {/* удаление*/}
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

export default HaltTable;
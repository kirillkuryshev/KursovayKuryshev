import React, { useEffect, useState } from 'react';
import 'antd/dist/antd.css';
import { Modal, Form, Input, Select, TimePicker } from 'antd';
import './cruise'
import axios from 'axios';
import moment from 'moment';

import { useNavigate, NavLink } from 'react-router-dom';
const uri = "http://localhost:5000/api/Cruise/";
const format = 'HH:mm'; // формат времени

const AddCruise = ({ cruises, setCruises, days, routeId }) => { // модальное окно для добавления
    // рейса
    let navigate = useNavigate();
    axios.defaults.withCredentials = true; // передача cookie по умолчанию
    const [isModalVisible, setIsModalVisible] = useState(false);  // видимость модального окна
    const [form] = Form.useForm();

    const showModal = () => { // показать модальное окно
        setIsModalVisible(true);
    };

    const handleOk = () => { // кнопка ок модального окна
        setIsModalVisible(false);
    };

    const handleCancel = () => { // кнопка отмена модального окна
        setIsModalVisible(false);
    };

    const addCruise = (cruise) => setCruises([...cruises, cruise]); // добавление созданного 
    // элемента

    const handleSubmit = (e) => { // отправка новой остановки на сервер и закрытие окна
        const day = days.filter(i => i.id == e.day)[0];
        const time = String(e.time._d).substring(16, 21);
        const places = e.places;
        handleOk();
        const cruise = {
            routeId: routeId, day: day, time: time, places: places, hidden: 0
        };
        axios.post(uri, cruise)
            .then((response) => {
                response.status = 201 ? addCruise(response.data) : null;
            })
            .catch((response) => {
                if (response.status === 401) {
                    navigate("../../login", { replace: true });
                }
                else {
                    throw new Error("Ошибка добавления рейса");
                }
            });
    };

    return (
        <div>
            <br />
            <button onClick={showModal} className="btn btn-success mb-1">
                <i className="fa fa-plus"></i></button>
            <Modal title="Добавить рейс" visible={isModalVisible} onOk={form.submit}
                onCancel={handleCancel}>
                <Form form={form} onFinish={handleSubmit}>
                    <Form.Item
                        label="День недели"
                        name="day"
                        rules={[
                            {
                                required: true,
                                message: 'Выберите день!',
                            },
                        ]}
                    >
                        <Select>
                            {days.map(({ id, day }) => (
                                <Option value={id}>{day}</Option>
                            ))}
                        </Select>
                    </Form.Item>
                    <Form.Item
                        name="time"
                        label="Время отправки"
                        rules={[
                            {
                                required: true,
                                message: 'Выберите время отправки!',
                            },
                        ]}
                    >
                        <TimePicker initialValues={moment('00:00', format)} format={format}/>
                    </Form.Item>
                    <Form.Item
                        label="Количество мест"
                        name="places"
                        rules={[
                            {
                                required: true,
                                message: 'Введите количество мест!',
                            },
                        ]}
                    >
                        <Input/>
                    </Form.Item>
                </Form>
            </Modal>
        </div>
    );
};

export default AddCruise;
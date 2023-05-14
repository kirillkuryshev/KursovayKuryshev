import React, { useEffect, useState } from 'react';
import 'antd/dist/antd.css';
import { Modal, Form, Input, Select } from 'antd';
import './halt'
import axios from 'axios';
import { useNavigate, NavLink } from 'react-router-dom';

const uri = "http://localhost:5000/api/Halt/";

const AddHalt = ({ halts, setHalts, localities }) => { // модальное окно для добавления остановки
    axios.defaults.withCredentials = true; // передача куки по-умолчанию
    const [isModalVisible, setIsModalVisible] = useState(false);  // видимость модального окна
    const [form] = Form.useForm();
    let navigate = useNavigate();
    const showModal = () => { // показать модальное окно
        setIsModalVisible(true);
    };

    const handleOk = () => { // кнопка ок модального окна
        setIsModalVisible(false);
    };

    const handleCancel = () => { // кнопка отмена модального окна
        setIsModalVisible(false);
    };

    const addHalt = (halt) => setHalts([...halts, halt]); // добавление созданного элемента

    const handleSubmit = (e) => { // отправка новой остановки на сервер и закрытие окна
        const adress = e.adress;
        const locality = e.locality;
        handleOk();
        const halt = {
            adress: adress, locality_model: localities.filter(item =>
                item.locality_id == locality)[0], hidden: 0
        };
        axios.post(uri, halt)
            .then((response) => {
                response.status = 201 ? addHalt(response.data) : null;
            })
            .catch((response) => {
                if (response.status === 401) {
                    navigate("../../login", { replace: true });
                }
                else {
                    throw new Error("Ошибка добавления остановки");
                }
            });
    };

    return (
        <div>
            <br/>
            <button onClick={showModal} className="btn btn-success mb-1">
                <i className="fa fa-plus"></i></button>
            <Modal title="Добавить остановку" visible={isModalVisible} onOk={form.submit}
                onCancel={handleCancel}>
                <Form form={form} onFinish={handleSubmit}>
                    <Form.Item
                        label="Название остановки"
                        name="adress"
                        rules={[
                            {
                                required: true,
                                message: 'Введите название остановки!',
                            },
                        ]}
                    >
                        <Input/>
                    </Form.Item>
                    <Form.Item
                        name="locality"
                        label="Населенный пункт"
                        rules={[
                            {
                                required: true,
                                message: 'Выберите населенный пункт!',
                            },
                        ]}
                    >
                        <Select>
                            {localities.map(({ locality_id, locality_name }) => (
                                <Option value={locality_id}>{locality_name}</Option>
                            ))}
                        </Select>
                    </Form.Item>
                </Form>
            </Modal>
        </div>
    );
};

export default AddHalt;
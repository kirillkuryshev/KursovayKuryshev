import React, { useEffect, useState } from 'react';
import 'antd/dist/antd.css';
import { Modal, Form, Input, Select } from 'antd';
import './halt'
import axios from 'axios';
import { useNavigate, NavLink } from 'react-router-dom';

const uri = "http://localhost:5000/api/Ticket/Buy/";

const Buy = ({ travell, getTravells }) => { // модальное окно покупки билета
    axios.defaults.withCredentials = true;
    const [msg, setMsg] = useState(''); // сообщение о результате покупки
    const [isModalVisible, setIsModalVisible] = useState(false);  // видимость модального окна
    const [form] = Form.useForm();
    let navigate = useNavigate();
    const showModal = () => { // показать модальное окно
        setIsModalVisible(true);
    };

    const handleCancel = () => { // кнопка отмена модального окна
        Reload();
        setIsModalVisible(false);
    };

    const Reload = () => { // перезагрузка данных
        getTravells();
    }

    const handleSubmit = (e) => { // отправка запроса на покупку
            axios.post(uri + String(e.places), travell, { withCredentials:true })
                .then((response) => {
                    setMsg(response.data.message);
                })
                .catch((response) => {
                    if (response.status === 401) {
                        navigate("../../login", { replace: true });
                    }
                    else {
                        throw new Error("Ошибка покупки билета");
                    }
                });
    };

    return (
        <div>
            <button className='btn btn-info' onClick={showModal}>
                Купить</button>
            <Modal title="Добавить остановку" visible={isModalVisible} onOk={form.submit}
                onCancel={handleCancel}>
                Выберите место
                <Form form={form} onFinish={handleSubmit}>
                    <Form.Item
                        name="places"
                        label="Место"
                        rules={[
                            {
                                required: true,
                                message: 'Выберите место!',
                            },
                        ]}
                    >
                        <Select>
                            {travell.places.map((record) => (
                                <Select.Option value={record}>{record}</Select.Option>
                            ))}
                        </Select>
                    </Form.Item>
                </Form>
                <div>{msg}</div>
            </Modal>
        </div>
    );
};

export default Buy;
import React, { useEffect, useState } from 'react';
import 'antd/dist/antd.css';
import { Form, Input, Button, Checkbox } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useNavigate, NavLink } from 'react-router-dom';
import axios from 'axios';

const uri = 'http://localhost:5000/api/Ticket/';
const Return = () => { // компонент для оформления возврата билета
    let navigate = useNavigate();

    axios.defaults.withCredentials = true;
    axios.post('http://localhost:5000/api/Account/' + 'checkRole')
        .then((response) => {
            if (response.status == 200) {
                if (response.data.role !== "user") {
                    navigate("../../login", { replace: true });
                }
            }
        })
        .catch((response) => {
            throw new Error("Ошибка проверки роли");
        });

    const [status, setStatus] = useState(""); // статус билета с введенным  номером
    const [cost, setCost] = useState(0);  // сумма к возврату для выбранного билета

    const onFinish = (values) => { // отправка запроса на возврат
        if (cost > 0) {
            axios.post(uri + 'Return/' + values.id, { cost: cost })
                .then((response) => {
                    if (response.status == 200) {
                        setStatus("Возврат оформлен");
                    }
                })
                .catch((response) => {
                    throw new Error("Ошибка оформления возврата");
                });
        }
    };

    const handleChange = event => { // получение информации о возможности возврата билета с введенным номером
        axios.get(uri + 'ReturnInfo/' + event.target.value)
            .then((response) => {
                if (response.status == 200) {
                    setStatus(response.data.status);
                    setCost(response.data.cost);
                }
            })
            .catch((response) => {
                throw new Error("Ошибка получения информации о возможности возврата билета");
            });
    };

    return (
        <div className='container'>
            <h2>Возврат билета:</h2>
            <Form
                name="normal_return"
                className="return-form"
                onFinish={onFinish}
            >
                <Form.Item
                    name="id"
                    rules={[
                        {
                            required: true,
                            message: 'Введите номер билета!',
                        },
                    ]}
                >
                    <Input onChange={handleChange} placeholder="Номер билета" />
                </Form.Item>
                <div className="row">
                    <label>Статус: {status}</label>
                </div>
                <div className="row">
                    <label>К возврату: {cost}</label>
                </div>
                <Form.Item>
                    <Button type="primary" htmlType="submit" className="login-form-button">
                        Возврат
                    </Button>
                </Form.Item>
            </Form>
        </div>
    );
};

export default Return
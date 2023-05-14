import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ReactDOM from 'react-dom';
import RouteTable from './routeTable'
import { Select } from 'antd'
import { useNavigate, NavLink } from 'react-router-dom';

export function InitializeRoutes() { return App(); }

const App = () => { // основной компонент для маршрутов

    let navigate = useNavigate();
    axios.defaults.withCredentials = true;
    axios.post('http://localhost:5000/api/Account/' + 'checkRole') // проверка роли
        .then((response) => {
            if (response.status == 200) {
                if (response.data.role !== "admin") {
                    navigate("../../login", { replace: true });
                }
            }
        })
        .catch((response) => {
            throw new Error("Ошибка проверки роли");
        });

    const [halts, setHalts] = useState([]); // список остановок выбранного маршрута
    const [routes, setRoutes] = useState([]); // список маршрутов
    const [selectedRoute, setSelected] = useState(-1); // выбранный маршрут

    useEffect(() => { // получение списка маршрутов
        axios({
            "method": "GET",
            "url": "http://localhost:5000/api/Route/",
            "mode": 'no-cors',
            "headers": {
                "content-type": "application/json",
            }
        })
            .then((response) => {
                setRoutes(response.data);
            })
            .catch((response) => {
                throw new Error("Ошибка получения списка маршрутов");
            });
    }, []);

    function getHalts(value) { // получение выбранного маршрута
        setSelected(value);
        var cr = routes.filter(i => i.routeId === value)[0].routeHalts;
        setHalts(cr);
    }

    return (
        <div className="container">
            <br />
            <Select style={{ width: "10%" }} onChange={getHalts}>
                {routes.map(({ routeId }) => (
                    <Select.Option value={routeId}>{routeId}</Select.Option>
                ))}
            </Select>
            <RouteTable
                halts={halts}
                setHalts={setHalts}/>
        </div>
    );
};
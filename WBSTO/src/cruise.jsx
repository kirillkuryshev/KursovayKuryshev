import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ReactDOM from 'react-dom';
import CruiseTable from './cruiseTable'
import AddCruise from './addCruise'
import { Select } from 'antd'
import { useNavigate, NavLink } from 'react-router-dom';

const uri = "http://localhost:5000/api/Cruise/";

export function InitializeCruises() { return App(); }

const App = () => { // основной компонент для отображения рейсов

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

    const [cruises, setCruises] = useState([]); // список рейсов
    const [routes, setRoutes] = useState([]); // список маршрутов
    const [days, setDays] = useState([]); // список дней
    const [selectedRoute, setSelected] = useState(-1); // выбранный рейс

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
    }, [setCruises]);

    useEffect(() => { // получение списка дней
        axios({
            "method": "GET",
            "url": "http://localhost:5000/api/Day/",
            "mode": 'no-cors',
            "headers": {
                "content-type": "application/json",
            }
        })
            .then((response) => {
                setDays(response.data);
            })
            .catch((response) => {
                throw new Error("Ошибка получения списка дней");
            });
    }, [setDays]);

    function getCruises(value) { // получение выбранного рейса
        setSelected(value);
        var cr = routes.filter(i => i.routeId === value)[0].cruises;
        setCruises(cr);
    }

    return (
        <div className="container">
            <br/>
            <Select style={{ width: "10%" }} onChange={getCruises}>
            {routes.map(({ routeId }) => (
                <Select.Option value={routeId}>{routeId}</Select.Option>
            ))}
            </Select>
                <AddCruise
                cruises={cruises}
                setCruises={setCruises}
                days={days}
                routeId={selectedRoute} />
            <CruiseTable
                cruises={cruises}
                setCruises={setCruises}
                days={days}/>
        </div>
    );
};
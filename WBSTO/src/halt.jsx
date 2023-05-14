import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ReactDOM from 'react-dom';
import HaltTable from './haltsTable'
import AddHalt from './addHalt'
import { useNavigate, NavLink } from 'react-router-dom';

const uri = "http://localhost:5000/api/Halt/";

export function InitializeHalts() { return App(); }

const App = () => { // основной компонент для работы с остановками
    let navigate = useNavigate();

    const [halts, setHalts] = useState([]); // список остановок
    const [localities, setLocalities] = useState([]); // список населенных пунктов

    axios.post('http://localhost:5000/api/Account/' + 'checkRole', {}, { withCredentials:true }) // проверка роли
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

    useEffect(() => { // получение списка населенных пунктов
        axios({
            "method": "GET",
            "url": "http://localhost:5000/api/Locality/",
            "mode": 'no-cors',
            "headers": {
                "content-type": "application/json",
            }
        })
            .then((response) => {
                setLocalities(response.data);
            })
            .catch((response) => {
                throw new Error("Ошибка получения населенных пунктов");
            });
    }, []);

    useEffect(() => { // получение списка остановок
        axios({
            "method": "GET",
            "url": uri,
            "mode": 'no-cors',
            "headers": {
                "content-type": "application/json",
            }
        })
            .then((response) => {
                setHalts(response.data);
            })
            .catch((response) => {
                throw new Error("Ошибка получения списка остановок");
            });
    }, []);

    return (
        <div className="container">
            <AddHalt
                halts={halts}
                setHalts={setHalts}
                localities={localities}
            />
            <HaltTable
                halts={halts}
                setHalts={setHalts}
                localities={localities} />
        </div>
    );
};
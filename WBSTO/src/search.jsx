import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { DatePicker, Space } from 'antd';
import ReactDOM from 'react-dom';
import { useNavigate, NavLink } from 'react-router-dom';
import moment from 'moment';
import { MomentToDate } from './helpful'
import TravellTable from './travellTable'

const uri = "http://localhost:5000/api/Ticket/";
const dateFormat = 'DD/MM/YYYY';

export function InitializeHalts() { return App(); }

const Search = () => { // поиск подходящих рейсов 
    let navigate = useNavigate();

    const [halts, setHalts] = useState([]); // список остановок
    const [shalts, setSHalts] = useState([]); // список остановок для начального населенного пункта
    const [ehalts, setEHalts] = useState([]); // список остановок для конечного населенного пункта
    const [localities, setLocalities] = useState([]); // список населенных пунктов
    const [startHalt, setStartHalt] = useState(-1); // остановка отправления
    const [endHalt, setEndHalt] = useState(-1); // конечная остановка
    const [date, setDate] = useState(MomentToDate(moment())); // выбранная дата
    const [travells, setTravells] = useState([]); // список подходящих рейсов с дополнительной информацией
    const [loading, setLoading] = useState(false); // индикатор загрузки

    const showLoading = () => (loading ? <div>Загрузка...</div> : <TravellTable travells={travells}
        getTravells={getTravells} />);
    axios.post('http://localhost:5000/api/Account/' + 'checkRole', {}, { withCredentials: true })
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

    const setStart = (id) => { // установка начального значения начальной остановки при смене населенного
        // пункта
        const buf = halts.filter(i => i.locality_model.locality_id == id); // список остановок населенного
        // пункта
        if (buf.length > 0) {
            setStartHalt(buf[0].halt_id);
        }
        setSHalts(buf);
    }

    const setEnd = (id) => {// установка начального значения конечной остановки при смене населенного
        // пункта
        const buf = halts.filter(i => i.locality_model.locality_id == id); // список остановок населенного
        // пункта
        if (buf.length > 0) {
            setEndHalt(buf[0].halt_id);
        }
        setEHalts(buf);
    }

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
                throw new Error("Ошибка получения списка населенных пунктов");
            });
    }, []);

    useEffect(() => { // получение списка остановок
    axios({
        "method": "GET",
        "url": 'http://localhost:5000/api/Halt/',
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

    useEffect(() => { // установка начальных начальной и конечной остановок после получения списка
        // населенных пунктов
        if (localities.length > 0 && halts.length > 0) {
            setStart(localities[0].locality_id);
            setEnd(localities[0].locality_id);
        }
    }, [localities]);

    useEffect(() => { // установка начальных начальной и конечной остановок после получения списка
        // остановок пунктов
        if (localities.length > 0 && halts.length > 0) {
            setStart(localities[0].locality_id);
            setEnd(localities[0].locality_id);
        }
    }, [halts]);

    const startLChange = event => { // смена начального населенного пункта
        setStart(event.target.value);
    }

    const endLChange = event => { // смена конечного населенного пункта
        setEnd(event.target.value);
    }

    const startHChange = event => { // смена начальной остановки
        setStartHalt(event.target.value);
    }

    const endHChange = event => { // смена конечной остановки
        setEndHalt(event.target.value);
    }

    const changeDate = event => { // смена даты
        if (moment() < event && event.diff(moment(), 'days') < 15) { // дата выходит за допустимый диапазон
            setDate(MomentToDate(event)); // сброс даты
        }
        else {
            setDate(MomentToDate(moment()));
        }
    }

    const getTravells = () => { // запрос на получение подходящих рейсов
        setLoading(true);
        setTravells([]);
        axios.post(uri + 'Travell', { date: date, start: startHalt, end: endHalt },
            { withCredentials: true })
            .then((response) => {
                setLoading(false);
                setTravells(response.data);
            })
            .catch((response) => {
                throw new Error("Ошибка получения рейсов");
            });
    }

    useEffect(() => { // загрузка новых рейсов при смене даты
        if (startHalt != -1 && endHalt != -1) {
            getTravells();
        }
    }, [date]);

    useEffect(() => { // загрузка новых рейсов при смене начальной остановки
        if (startHalt != -1 && endHalt != -1) {
            getTravells();
        }
    }, [startHalt]);

    useEffect(() => { // загрузка новых рейсов при смене конечной остановки
        if (startHalt != -1 && endHalt != -1) {
            getTravells();
        }
    }, [endHalt]);

    return (
        <div className="container">
            <div className="row">
                <label>Дата отправки: </label>
                <DatePicker value={moment(date)} format={dateFormat} onChange={changeDate} />
                    <small>*Не более чем на 2 недели вперед</small>
            </div>
            <div className="row">
                <div className="col-6">
                    <label>Населенный пункт отправки:&ensp;</label>
                    <select onChange={startLChange}>{localities.map(({ locality_id, locality_name }) => (
                        <option value={locality_id}>{locality_name}</option>
                    ))}</select>
                </div>
                <div className="col-6">
                    <label>Остановка отправки:&ensp;</label>
                    <select style={{ width: "40%" }} onChange={startHChange}>{shalts.map(({ halt_id, adress }) => (
                        <option value={halt_id}>{adress}</option>
                    ))}</select>
                </div>
            </div>
            <p />
            <div className="row">
                <div className="col-6">
                    <label>Населенный пункт прибытия:&ensp;</label>
                    <select onChange={endLChange}>{localities.map(({ locality_id, locality_name }) => (
                        <option value={locality_id}>{locality_name}</option>
                    ))}</select>
                </div>
                <div className="col-6">
                    <label>Остановка прибытия:&ensp;</label>
                    <select style={{ width: "40%" }} onChange={endHChange}>{ehalts.map(({ halt_id, adress }) => (
                        <option value={halt_id}>{adress}</option>
                    ))}</select>
                </div>
            </div>
            {showLoading()}
        </div>
    );
};

export default Search;
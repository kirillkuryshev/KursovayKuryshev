import React, { useEffect, useState } from 'react';
import axios from 'axios';
import ReactDOM from 'react-dom';
import moment from 'moment';

const hiddenToString = (hidden) => { // преобразование числового значения hidden в текстовое
    return hidden.hidden == 0 ? <label>Виден</label> : <label>Скрыт</label>
}

const momentToDate = (cmoment) => { // перевод даты из moment в гггг-мм-дд
    cmoment = cmoment._d;
    return String(cmoment.getYear() + 1900) + "-" + String(cmoment.getMonth() + 1) + "-" + String(cmoment.getDate());
}

const setTablesLocalities = ({ locality_model, localities }) => { // установка option для select - item - список
    // населенных пунктов
    let x = [];
    for (let l = 0; l < localities.length; l++) {
        if (localities[l].locality_id == locality_model.locality_id) {
            x.push(<option key={localities[l].locality_id} value={localities[l].locality_id} selected>
                {localities[l].locality_name}</option>);
        }
        else {
            x.push(<option key={localities[l].locality_id} value={localities[l].locality_id}>
                {localities[l].locality_name}</option>);
        }
    }
    return x;
}

const setOption = ({ value, item, sItem }) => { // установка option для select с учетом выбранного
    if (value === sItem) {
        return <option key={value} value={value} selected>{item}</option>
    }
    else {
        return <option key={value} value={value}>{item}</option>
    }
}

export function MomentToDate(cmoment) { return momentToDate(cmoment); }
export function HiddenToString(hidden) { return hiddenToString(hidden); }
export function SetOption(value, item, sItem) { return setOption(value, item, sItem); }
export function SetTablesLocalities(locality_model, localities) {
    return setTablesLocalities(locality_model,
        localities);
}
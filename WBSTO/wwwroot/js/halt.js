const uri = "api/Halt";
let halts;
let localities;

function getData() {
    getHalts();
    getLocalities();
}

async function getHalts() {
    var result;
    result = await fetch(uri);
    halts = await result.json();
    var haltTable = "";
    for (i in halts) {
        haltTable += "<tr>";
        haltTable += "<td>" + halts[i].halt_id + "</td>";
        haltTable += "<td>" + halts[i].adress + "</td>";
        haltTable += "<td>" + halts[i].locality_model.locality_name + "</td>";
        if (halts[i].hidden == 0) {
            haltTable += "<td>Виден</td>";
        }
        else {
            haltTable += "<td>Скрыт</td>";
        }
        haltTable += "<td><button class='btn btn-warning' data-bs-toggle='modal' data-bs-target='#create'><i class='fa fa-edit'></i> <button style='margin-left : 8px' class='btn btn-danger' data-bs-toggle='modal' data-bs-target='#create'><i class='fa fa-trash-alt'></i></button></td>";
        haltTable += "</tr>";
    }
    document.getElementById("haltTable").innerHTML = haltTable;
}

async function getLocalities() {
    var result;
    result = await fetch("api/Locality");
    localities = await result.json();
}
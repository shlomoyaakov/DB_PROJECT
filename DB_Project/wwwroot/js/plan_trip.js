function showAccommodations() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            // find all countries
            for (i in jsonResponse) {
                var li = document.createElement("li");
                li.appendChild(document.createTextNode(jsonResponse[i].name.replaceAll("\"","")));
                accommodationsList.appendChild(li);
            }
        }
        else {
            alert('Error occure while getting accommodations from server');
        }
    };
    // ask the server for countries & cities list
    xhttp.open("GET", "/api/Accommodation");
    xhttp.send();
}

function showRestaurants() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            // find all countries
            for (i in jsonResponse) {
                var li = document.createElement("li");
                li.appendChild(document.createTextNode(jsonResponse[i].name.replaceAll("\"", "")));
                restaurantsList.appendChild(li);
            }
        }
        else {
            alert('Error occure while getting restaurants from server');
        }
    };
    // ask the server for countries & cities list
    xhttp.open("GET", "/api/Restaurants");
    xhttp.send();
}

function showAttractions() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            // find all countries
            for (i in jsonResponse) {
                var li = document.createElement("li");
                li.appendChild(document.createTextNode(jsonResponse[i].name.replaceAll("\"", "")));
                attractionsList.appendChild(li);
            }
        }
        else {
            alert('Error occure while getting attractions from server');
        }
    };
    // ask the server for countries & cities list
    xhttp.open("GET", "/api/Attractions");
    xhttp.send();
}



var username = localStorage.getItem("user");;
var country = localStorage.getItem("country");
var city = localStorage.getItem("city");
var accommodationsList = document.getElementById("accommodationsList");
var restaurantsList = document.getElementById("restaurantsList");
var attractionsList = document.getElementById("attractionsList");
window.onload = function () {
    showAccommodations();
    showRestaurants();
    showAttractions();
}

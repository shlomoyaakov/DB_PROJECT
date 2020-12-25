


function showAccommodations() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            if (jsonResponse.length == 0) {
                document.getElementById("emptyAccommodations").classList.remove("d-none")
                document.getElementById("accoNav").remove();
            }
            else {
                for (i in jsonResponse) {
                    var li = document.createElement("li");
                    li.appendChild(document.createTextNode(jsonResponse[i].name.replaceAll("\"", "")));
                    accommodationsList.appendChild(li);
                }
            }
        }
        else {
            document.getElementById("emptyAccommodations").classList.remove("d-none")
            document.getElementById("accoNav").remove();
        }
        if (responses < 2) {
            responses++
        } else {
            loadDiv.remove()
            mainDiv.classList.remove("d-none")
        }
    };
    // ask the server for countries & cities list
    let request = "/api/Accommodation/location?country=" + country + "&city=" + city
    xhttp.open("GET", request);
    xhttp.send();
}

function showRestaurants() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            if (jsonResponse.length == 0) {
                document.getElementById("emptyRestaurants").classList.remove("d-none")
                document.getElementById("restNav").remove();
            }
            else {
                for (i in jsonResponse) {
                    var li = document.createElement("li");
                    li.appendChild(document.createTextNode(jsonResponse[i].name.replaceAll("\"", "")));
                    restaurantsList.appendChild(li);
                }
            }
        }
        else {
            document.getElementById("emptyRestaurants").textContent = 'Error occure while getting restaurants from server'
            document.getElementById("restNav").remove();
        }
        if (responses < 2) {
            responses++
        } else {
            loadDiv.remove()
            mainDiv.classList.remove("d-none")
        }
    };
    // ask the server for countries & cities list
    // bug that need to be fixed!
     // xhttp.open("GET", "/api/Restaurants/location?country=" + country + "&city=" + city);
    //xhttp.send();
      if (responses < 2) {
            responses++
        } else {
            loadDiv.remove()
            mainDiv.classList.remove("d-none")
        }
}

function showAttractions() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            if (jsonResponse.length == 0) {
                document.getElementById("emptyAttractions").textContent = "Sorry, no attractions was found in Database";
                document.getElementById("AttracNav").remove();
            }
            else {
                for (i in jsonResponse) {
                    var li = document.createElement("li");
                    li.appendChild(document.createTextNode(jsonResponse[i].name.replaceAll("\"", "")));
                    attractionsList.appendChild(li);
                }
            }
        }
        else {
            document.getElementById("emptyAttractions").textContent = 'Error occure while getting attractions from server';
            document.getElementById("AttracNav").remove();
        }
        if (responses < 2) {
            responses++
        } else {
            loadDiv.remove()
            mainDiv.classList.remove("d-none")
        }
    };
    // ask the server for countries & cities list
    xhttp.open("GET", "/api/Attractions/location?country=" + country + "&city=" + city);
    xhttp.send();
}


var responses = 0
var mainDiv = document.getElementById("main")
var loadDiv = document.getElementById("loader")
var username = localStorage.getItem("user");
var country = localStorage.getItem("country");
var city = localStorage.getItem("city");
document.getElementById("travelId").textContent = city + " , " + country;
var accommodationsList = document.getElementById("accommodationsList");
var restaurantsList = document.getElementById("restaurantsList");
var attractionsList = document.getElementById("attractionsList");


window.onload = function () {
    showAccommodations();
    showRestaurants();
    showAttractions();
}

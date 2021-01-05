function goBack() {
    window.history.back();
}

function showAccommodations() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            if (jsonResponse.length == 0) {
                document.getElementById("emptyAccommodations").classList.remove("d-none")
                accommodationsList.remove();
            }
            else {
                for (i in jsonResponse) {
                    let name = jsonResponse[i].name
                    var option = document.createElement("option");
                    option.text = name
                    option.value = JSON.stringify(jsonResponse[i])
                    accommodationsList.add(option, accommodationsList[0]);
                }
            }
        }
        else {
            document.getElementById("emptyAccommodations").classList.remove("d-none")
            accommodationsList.remove();
        }
        if (responses < 2) {
            responses++
        } else {
            pageLoaded()
        }
    };
    // ask the server for countries & cities list
    if (showNewPlacesOnly === "true") {
        xhttp.open("GET", "/api/Accommodation/region_and_user?country=" + country + "&city=" + city + "&user_name=" + username);
    } else {
        xhttp.open("GET", "/api/Accommodation/region?country=" + country + "&city=" + city);
    }
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
                restaurantsList.remove();
            }
            else {
                for (i in jsonResponse) {
                    let name = jsonResponse[i].name.replaceAll("\"", "")
                    var option = document.createElement("option");
                    option.text = name
                    option.value = JSON.stringify(jsonResponse[i])
                    restaurantsList.add(option, restaurantsList[0]);
                }
            }
        }
        else {
            document.getElementById("emptyRestaurants").textContent = 'Error occure while getting restaurants from server'
            restaurantsList.remove();
        }
        if (responses < 2) {
            responses++
        } else {
            pageLoaded()
        }
    };
    // ask the server for countries & cities list
    if (showNewPlacesOnly === "true") {
        xhttp.open("GET", "/api/Restaurants/region_and_user?country=" + country + "&city=" + city + "&user_name=" + username);

    } else {
        xhttp.open("GET", "/api/Restaurants/region?country=" + country + "&city=" + city);
    }
    xhttp.send();

}

function showAttractions() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            if (jsonResponse.length == 0) {
                document.getElementById("emptyAttractions").classList.remove("d-none")
                attractionsList.remove();
            }
            else {
                for (i in jsonResponse) {
                    let name = jsonResponse[i].name.replaceAll("\"", "")
                    var option = document.createElement("option");
                    option.text = name
                    option.value = JSON.stringify(jsonResponse[i])
                    attractionsList.add(option, attractionsList[0]);
                }
            }
        }
        else {
            document.getElementById("emptyAttractions").textContent = 'Error occure while getting attractions from server';
            attractionsList.remove();
        }
        if (responses < 2) {
            responses++
        } else {
            pageLoaded()
        }
    };
    if (showNewPlacesOnly === "true") {
        xhttp.open("GET", "/api/Attractions/region_and_user?country=" + country + "&city=" + city + "&user_name=" + username);
    } else {
        xhttp.open("GET", "/api/Attractions/region?country=" + country + "&city=" + city);
    }
    xhttp.send();
}


let pageLoaded = function () {
    loadDiv.remove()
    mainDiv.classList.remove("d-none")
    $(".selection-2").select2({
        minimumResultsForSearch: 20,
        dropdownParent: $('#dropDownSelect1')
    });
       

    $('option').mousedown(function (e) {
        e.preventDefault();
        valueJSON = JSON.parse(this.value)
        let location = valueJSON.location.coordinates
        let phone = valueJSON.phone
        switch (this.parentElement.id) {
            case 'accommodationsList':
                let internet = valueJSON.internet
                let type = valueJSON.type
                detailsString = "Internet: " + internet + "<br>Location: lat " + location.latitude + ", lng " + location.longitude + "<br>Phone: " + phone + "<br>Type: " + type
                break;
            case 'restaurantsList':
                let cuisine = valueJSON.cuisine.replaceAll(";", ", ")
                detailsString = "Cuisine: " + cuisine + "<br>Location: lat " + location.latitude + ", lng " + location.longitude + "<br>Phone: " + phone 
                break;
            case 'attractionsList':
                detailsString = "Location: lat " + location.latitude + ", lng " + location.longitude + "<br>Phone: " + phone
                break;
            default:
                break;
        }
        $(this).prop('selected', !$(this).prop('selected'));
        var coordinets = getConstLocation(detailsString)
        setMarker(coordinets)
        $('#hoverlabel').text($(this).prop('label'))
        document.getElementById("hovervalue").innerHTML = detailsString
        return false;
    });
}


function initMap() {
    let options = {
        zoom: 3,
        center: { lat: 32.3232919, lng: 34.85538661 },
    }
    map = new google.maps.Map(document.getElementById("map"), options);
}
function getConstLocation(value) {
   //"Internet: N/A<br>Location: 48.8824615, 2.3498469<br>Phone: N/A<br>Type: hotel"
    var latitude = value.match("lat (.*),")[1];
    var longitude = value.match("lng (.*)<br>P")[1];

    const myLatLng = { lat: parseFloat(latitude), lng: parseFloat(longitude) };
    return myLatLng
}
function setMarker(location) {
    if (marker !== "") {
        marker.setMap(null)
    }
    marker = new google.maps.Marker({
        position: location,
        map,
        title: "Hello World!",
    });
    map.setCenter(location)
    map.setZoom(13)
}

// Get the modal
var modal = document.getElementById("myModal");

// Get the button that opens the modal
var btn = document.getElementById("saveBtn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks the button, open the modal 
btn.onclick = function () {
    var details = document.getElementById("tripDetails")
    var acclist= ""
    var reslist = ""
    var attlist = ""

    for (var i = 0, len = accommodationsList.options.length; i < len; i++) {
        opt = accommodationsList.options[i];
        if (opt.selected) {
            acclist += opt.text+", "
        }
    }
    acclist = acclist.substring(0, acclist.length-2)

    for (var i = 0, len = restaurantsList.options.length; i < len; i++) {
        opt = restaurantsList.options[i];
        if (opt.selected) {
            reslist += opt.text + ", "
        }
    }
    reslist = reslist.substring(0, reslist.length - 2)

    for (var i = 0, len = attractionsList.options.length; i < len; i++) {
        opt = attractionsList.options[i];
        if (opt.selected) {
            attlist += opt.text + ", "
        }
    }
    attlist = attlist.substring(0, attlist.length - 2)


    details.innerHTML = "<br><strong>Accommodations: </strong >" + acclist + "<br><strong>Restaurants: </strong>" + reslist + "<br><strong>Attractions: </strong>" + attlist
    modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    closeModal()
}
var closeModal = function () {
    modal.style.display = "none";}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

var saveTrip = function () {
    time = new Date,
        dformat = [time.getFullYear(),
        time.getMonth()+1,
        time.getDate()].join('-') + ' ' +
        [time.getHours(),
        time.getMinutes(),
        time.getSeconds()].join(':');
    var trip = { User_Name: localStorage.getItem("user"), Time: dformat, Attractions: [], Restaurants: [], Accommodation: [] }
    if (document.querySelector("#attractionsList") != null) {
        Array.from(document.querySelector("#attractionsList").options).forEach(function (option_element) {
            if (option_element.selected === true) {
                optionJSON = JSON.parse(option_element.value)
                trip.Attractions.push(optionJSON)
            }
        })
    }
    if (document.querySelector("#restaurantsList") != null) {
        Array.from(document.querySelector("#restaurantsList").options).forEach(function (option_element) {
            if (option_element.selected === true) {
                optionJSON = JSON.parse(option_element.value)
                trip.Restaurants.push(optionJSON)
            }
        })
    }
    if (document.querySelector("#accommodationsList") != null) {
        Array.from(document.querySelector("#accommodationsList").options).forEach(function (option_element) {
            if (option_element.selected === true) {
                optionJSON = JSON.parse(option_element.value)
                trip.Accommodation.push(optionJSON)
            }
        })
    }
    sendtrip(JSON.stringify(trip))
    closeModal()
}

function sendtrip(trip) {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            alert("Trip successfully saved");
        }
        else {
            alert(this.response);
        }
    };
    // generate and send the request to the server of register new account
    xhttp.open("POST", "/api/Trips");
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.send(trip);
}

var responses = 0
var mainDiv = document.getElementById("main")
var loadDiv = document.getElementById("loader")
var username = localStorage.getItem("user");
var country = localStorage.getItem("country");
var city = localStorage.getItem("city");
var showNewPlacesOnly = localStorage.getItem("newPlaces");
var accommodationsList = document.getElementById("accommodationsList");
var restaurantsList = document.getElementById("restaurantsList");
var attractionsList = document.getElementById("attractionsList");
var map
var marker = ""
document.getElementById("travelId").textContent += city + ", " + country;

window.onload = function () {
    showAccommodations();
    showRestaurants();
    showAttractions();
    initMap()
}
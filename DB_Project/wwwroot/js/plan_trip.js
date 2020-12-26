

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
                    let internet = jsonResponse[i].internet
                    let location = jsonResponse[i].location.coordinates
                    let phone = jsonResponse[i].phone
                    let type = jsonResponse[i].type

                    var option = document.createElement("option");
                    option.text = name
                    option.value = "Internet: " + internet + "<br>Location: lat " + location.latitude + ", lng " + location.longitude + "<br>Phone: " + phone + "<br>Type: " + type
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
                restaurantsList.remove();
            }
            else {
                for (i in jsonResponse) {
                    let name = jsonResponse[i].name.replaceAll("\"", "")
                    let cuisine = jsonResponse[i].cuisine.replaceAll(";",", ")
                    let location = jsonResponse[i].location.coordinates
                    let phone = jsonResponse[i].phone
      
                    var option = document.createElement("option");
                    option.text = name
                    option.value = "Cuisine: " + cuisine + "<br>Location: lat " + location.latitude + ", lng " + location.longitude  + "<br>Phone: " + phone 
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
      xhttp.open("GET", "/api/Restaurants/location?country=" + country + "&city=" + city);
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
                    let location = jsonResponse[i].location.coordinates
                    let phone = jsonResponse[i].phone


                    var option = document.createElement("option");
                    option.text = name
                    option.value = "Location: lat " + location.latitude + ", lng " + location.longitude  + "<br>Phone: " + phone
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
document.getElementById("travelId").textContent += city + ", " + country;
var accommodationsList = document.getElementById("accommodationsList");
var restaurantsList = document.getElementById("restaurantsList");
var attractionsList = document.getElementById("attractionsList");
var map
var marker = ""

window.onload = function () {
    showAccommodations();
    showRestaurants();
    showAttractions();
    initMap()
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
        $(this).prop('selected', !$(this).prop('selected'));
        var location = getConstLocation($(this).prop('value'))
        setMarker(location)
        return false;
    });

    $('option').mouseover(function (e) {
        e.preventDefault();
        $('#hoverlabel').text($(this).prop('label'))

        document.getElementById("hovervalue").innerHTML = $(this).prop('value')
        return false;
    });

    $('select').mouseleave(function (e) {
        e.preventDefault();
        $('#hoverlabel').text("")
        $('#hovervalue').text("")
        return false;
    });}
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
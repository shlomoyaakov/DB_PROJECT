
// when the help sign near the attractions section is clicked
function attHelpClicked() {
    if (attTravelsMap.size === 0) {
        return
    }
    // find the attraction with most visitors and set her as selected
    let maxVisitorsAttId = ([...attTravelsMap.entries()].reduce((a, e) => e[1] > a[1] ? e : a))[0]
    let maxVisitorsAttName = attIdNameMap.get(maxVisitorsAttId)
    let maxVisitorsAttOption = document.getElementById(maxVisitorsAttName)
    maxVisitorsAttOption.selected = "selected";
    showDetails(maxVisitorsAttOption.label, JSON.parse(maxVisitorsAttOption.value), "attractionsList")
}

// when the help sign near the restaurants section is clicked
function resHelpClicked() {
    if (resTravelsMap === 0) {
        return
    }
    // find the restaurant with most visitors and set her as selected
    let maxVisitorsResId = ([...resTravelsMap.entries()].reduce((a, e) => e[1] > a[1] ? e : a))[0]
    let maxVisitorsResName = resIdNameMap.get(maxVisitorsResId)
    let maxVisitorsResOption = document.getElementById(maxVisitorsResName)
    maxVisitorsResOption.selected = "selected";
    showDetails(maxVisitorsResOption.label, JSON.parse(maxVisitorsResOption.value), "restaurantsList")
}

// when the help sign near the accommodations section is clicked
function accHelpClicked() {
    if (accTravelsMap === 0) {
        return
    }
    // find the accommodation with most visitors and set her as selected
    let maxVisitorsAccId = ([...accTravelsMap.entries()].reduce((a, e) => e[1] > a[1] ? e : a))[0]
    let maxVisitorsAccName = accIdNameMap.get(maxVisitorsAccId)
    let maxVisitorsAccOption = document.getElementById(maxVisitorsAccName)
    maxVisitorsAccOption.selected = "selected";
    showDetails(maxVisitorsAccOption.label, JSON.parse(maxVisitorsAccOption.value), "accommodationsList")
}

function goBack() {
    window.history.back();
}

// load accommodations from DB into the select element
function showAccommodations() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            if (jsonResponse.length == 0) {
                document.getElementById("emptyAccommodations").classList.remove("d-none")
                document.getElementById("helpAccId").remove()
                accommodationsList.remove();
            }
            else {
                for (i in jsonResponse) {
                    let name = jsonResponse[i].name
                    var option = document.createElement("option");
                    option.text = name
                    option.setAttribute("id", name)
                    option.value = JSON.stringify(jsonResponse[i])
                    accommodationsList.add(option, accommodationsList[0]);
                    accIdNameMap.set(jsonResponse[i].id, name)
                }
            }
        }
        else {
            document.getElementById("emptyAccommodations").classList.remove("d-none")
            document.getElementById("helpAccId").remove()
            accommodationsList.remove();
        }
        if (responses < 5) {
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

// load restaurants from DB into the select element
function showRestaurants() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            if (jsonResponse.length == 0) {
                document.getElementById("emptyRestaurants").classList.remove("d-none")
                document.getElementById("helpResId").remove()
                restaurantsList.remove();
            }
            else {
                for (i in jsonResponse) {
                    let name = jsonResponse[i].name.replaceAll("\"", "")
                    var option = document.createElement("option");
                    option.text = name
                    option.setAttribute("id", name)
                    option.value = JSON.stringify(jsonResponse[i])
                    restaurantsList.add(option, restaurantsList[0]);
                    resIdNameMap.set(jsonResponse[i].id, name)
                }
            }
        }
        else {
            document.getElementById("emptyRestaurants").textContent = 'Error occure while getting restaurants from server'
            document.getElementById("helpResId").remove()
            restaurantsList.remove();
        }
        if (responses < 5) {
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

// load attractions from DB into the select element
function showAttractions() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            if (jsonResponse.length == 0) {
                document.getElementById("emptyAttractions").classList.remove("d-none")
                document.getElementById("helpAttId").remove()
                attractionsList.remove();
            }
            else {
                for (i in jsonResponse) {
                    let name = jsonResponse[i].name.replaceAll("\"", "")
                    var option = document.createElement("option");
                    option.text = name
                    option.setAttribute("id", name)
                    option.value = JSON.stringify(jsonResponse[i])
                    attractionsList.add(option, attractionsList[0]);
                    attIdNameMap.set(jsonResponse[i].id, name)
                }
            }
        }
        else {
            document.getElementById("emptyAttractions").textContent = 'Error occure while getting attractions from server';
            document.getElementById("helpAttId").remove()
            attractionsList.remove();
        }
        if (responses < 5) {
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
    if (loadedTrip !== "") { // new TODO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        selectLoaded()
    }
    // check if the user is an Admin and change the page accordingly
    checkIfAdmin(localStorage.getItem("user"), localStorage.getItem("pass"))
    // remove the loading screen
    loadDiv.remove()
    // show mainDiv
    mainDiv.classList.remove("d-none")
    // TODO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    $(".selection-2").select2({
        minimumResultsForSearch: 20,
        dropdownParent: $('#dropDownSelect1')
    });

    // allow multi selection without holding ctrl down and show details and map location of the clicked option
    $('select[multiple="multiple"] option').mousedown(function (e) {
        e.preventDefault();
        selectedPlace = valueJSON = JSON.parse(this.value)
        showDetails($(this).prop('label'), valueJSON, this.parentElement.id)
        var st = this.parentElement.scrollTop
        $(this).prop('selected', !$(this).prop('selected'));
        setTimeout(() => this.parentElement.scrollTop = st, 0);
        var coordinets = getConstLocation(detailsString)
        setMarker(coordinets)
        $('#updatePlaceButton, #deletePlaceButton').prop('disabled', false)
        return false;
    });

    // prevent focus for multi selection select (for esthetics)
    $('select[multiple="multiple"]').mousedown(function (e) {
        this.blur();
        e.preventDefault();
    });

    // when place type is selected show relevant form
    $('#newPlaceType').change(function (e) {
        $('.form-phone').removeClass('d-none')
        switch (this.value) {
            case 'accommodation':
                $('.form-accommodation').removeClass('d-none')
                $('.form-restaurant').addClass('d-none')
                $('.form-attraction').addClass('d-none')
                break;
            case 'restaurant':
                $('.form-accommodation').addClass('d-none')
                $('.form-restaurant').removeClass('d-none')
                $('.form-attraction').addClass('d-none')
                break;
            case 'attraction':
                $('.form-accommodation').addClass('d-none')
                $('.form-restaurant').addClass('d-none')
                $('.form-attraction').removeClass('d-none')
                break;
        }
    });
}

// writes a selected option to the details section
function showDetails(title, valueJSON, parentElement) {
    $('#hoverlabel').text(title)
    let location = valueJSON.location.coordinates
    let phone = valueJSON.phone
    let visitors
    switch (parentElement) {
        case 'accommodationsList':
            selectedPlaceType = 'accommodation'
            let internet = valueJSON.internet
            let type = valueJSON.type
            detailsString = "<b>Internet:</b> " + internet + "<br><b>Location:</b> lat " + location.latitude + ", lng " + location.longitude + "<br><b>Phone:</b> " + phone + "<br><b>Type:</b> " + type
            visitors = accTravelsMap.get(valueJSON.id)
            break;
        case 'restaurantsList':
            selectedPlaceType = 'restaurant'
            let cuisine = valueJSON.cuisine.replaceAll(";", ", ")
            detailsString = "<b>Cuisine:</b> " + cuisine + "<br><b>Location:</b> lat " + location.latitude + ", lng " + location.longitude + "<br><b>Phone:</b> " + phone
            visitors = resTravelsMap.get(valueJSON.id)

            break;
        case 'attractionsList':
            selectedPlaceType = 'attraction'
            detailsString = "<b>Location:</b> lat " + location.latitude + ", lng " + location.longitude + "<br><b>Phone:</b> " + phone
            visitors = attTravelsMap.get(valueJSON.id)

            break;
        default:
            break;
    }
    // show number of visitors if defined else show 0
    if (visitors !== undefined) {
        detailsString += "<br><b>Visitors:</b> " + visitors
    } else {
        detailsString += "<br><b>Visitors:</b> 0"
    }
    document.getElementById("hovervalue").innerHTML = detailsString
}

function initMap() {
    let options = {
        zoom: 3,
        center: { lat: 32.3232919, lng: 34.85538661 },
    }
    map = new google.maps.Map(document.getElementById("map"), options);
}

// gets a json representing an option and returns his location
function getConstLocation(value) {
    //"Internet: N/A<br>Location: 48.8824615, 2.3498469<br>Phone: N/A<br>Type: hotel"
    var latitude = value.match("lat (.*),")[1];
    var longitude = value.match("lng (.*)<br><b>P")[1];

    const myLatLng = { lat: parseFloat(latitude), lng: parseFloat(longitude) };
    return myLatLng
}

// moves the map to a location
function setMarker(location) {
    if (marker !== "") {
        marker.setMap(null)
    }
    marker = new google.maps.Marker({
        position: location,
        map,
    });
    map.setCenter(location)
    map.setZoom(18)
}

// Get the save-trip modal
var modal = document.getElementById("myModal"); // TODO rename modal to "saveTripModal" or something !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

// Get the button that opens the save-trip modal
var btn = document.getElementById("saveBtn");

// Get the <span> element that closes the save-trip modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks the save button, load data into the save-trip modal and show him
btn.onclick = function () {
    var details = document.getElementById("tripDetails")
    var acclist = ""
    var reslist = ""
    var attlist = ""

    for (var i = 0, len = accommodationsList.options.length; i < len; i++) {
        opt = accommodationsList.options[i];
        if (opt.selected) {
            acclist += opt.text + ", "
        }
    }
    acclist = acclist.substring(0, acclist.length - 2)

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
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

// build an object describing the trip and sand him to the DB
var saveTrip = function () {
    time = new Date,
        dformat = [time.getFullYear(),
        time.getMonth() + 1,
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
    if (trip.Attractions.length === 0 && trip.Restaurants.length === 0 && trip.Accommodation.length === 0) { // new
        alert("please choose some places before saving the trip")
    } else {
        sendtrip(JSON.stringify(trip))
    }
    closeModal()
}

// sends the trip to the DB
function sendtrip(trip) {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            alert("Trip successfully saved");
            window.location.assign("plan_trip.html");
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

// if loaded trip  select the places that are in the trip
function selectLoaded() {
    acco = loadedTrip.accommodation
    for (i in acco) {
        let element = document.getElementById(acco[i].name);
        if (element !== null) {
            element.setAttribute('selected', 'selected');
        }
    }
    rest = loadedTrip.restaurants
    for (i in rest) {
        let element = document.getElementById(rest[i].name);
        if (element !== null) {
            element.setAttribute('selected', 'selected');
        }
    }
    att = loadedTrip.attractions
    for (i in att) {
        let element = document.getElementById(att[i].name);
        if (element !== null) {
            element.setAttribute('selected', 'selected');
        }
    }
}

// get from DB how many visitors each Accommodation had
function askForAccTravelsMap() {
    let xhttp = new XMLHttpRequest();
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            for (i in jsonResponse) {
                let key = jsonResponse[i].Key
                let value = jsonResponse[i].Value
                accTravelsMap.set(key, value)
            }
        }
        else {
            alert(this.response);
        }
        if (responses < 5) {
            responses++
        } else {
            pageLoaded()
        }
    };
    xhttp.open("GET", "/api/Accommodation/travelers_by_region?country=" + country + "&city=" + city);
    xhttp.send();
}

// get from DB how many visitors each Restaurants had
function askForResTravelsMap() {
    let xhttp = new XMLHttpRequest();
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            for (i in jsonResponse) {
                let key = jsonResponse[i].Key
                let value = jsonResponse[i].Value
                resTravelsMap.set(key, value)
            }
        }
        else {
            alert(this.response);
        }
        if (responses < 5) {
            responses++
        } else {
            pageLoaded()
        }
    };
    xhttp.open("GET", "/api/Restaurants/travelers_by_region?country=" + country + "&city=" + city);
    xhttp.send();
}

// get from DB how many visitors each Attractions had
function askForAttTravelsMap() {
    let xhttp = new XMLHttpRequest();
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            for (i in jsonResponse) {
                let key = jsonResponse[i].Key
                let value = jsonResponse[i].Value
                attTravelsMap.set(key, value)
            }
        }
        else {
            alert(this.response);
        }
        if (responses < 5) {
            responses++
        } else {
            pageLoaded()
        }
    };
    xhttp.open("GET", "/api/Attractions/travelers_by_region?country=" + country + "&city=" + city);
    xhttp.send();
}

// check if the user is an Admin and change the page accordingly
function checkIfAdmin(username, password) {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            if (this.response === "true") {
                admin()
            }
        }
        else {
            alert(this.response);
        }
    };
    // generate and send the request to the server of register new account
    let url = "/api/Users/admin?" + "username=" + username + "&password=" + password;
    xhttp.open("GET", url);
    xhttp.send();
}

// add Admin-Only elements to the site
function admin() {
    $(".admin-only").removeClass("d-none");
}

function hideNewPlaceModal() {
    $("#screen-disabler").addClass("d-none");
    $("#new-place-form").addClass("d-none");
}

function showNewPlaceModal() {
    $('#newPlaceForm small').hide()
    $("#screen-disabler").removeClass("d-none");
    $("#new-place-form").removeClass("d-none");
}

// submit a place to the DB as an update or a new one (determined with the "editing" variable)
function submitPlace() {
    // clear the comments from the form
    $('#newPlaceForm small').hide()
    if (!formIsValid())
        return
    // show the "please wait" screen
    $('.wait-screen').removeClass('d-none');
    let place = {
        "Location":
        {
            "General_Location": {
                "Country": localStorage.getItem("country"),
                "City": localStorage.getItem("city")
            },
            "Coordinates": {
                "Latitude": parseFloat($('#newPlaceLatitude').val()),
                "Longitude": parseFloat($('#newPlaceLongitude').val())
            }
        },
        "Name": $('#newPlaceName').val(),
        "Phone": $('#newPlacePhone').val(),
    };
    let apiPath
    switch ($('#newPlaceType').val()) {
        case 'accommodation':
            apiPath = "/api/Accommodation"
            place.Internet = $('#newAccommodationInternet').val()
            place.Type = $('#newAccommodationType').val()
            break;
        case 'restaurant':
            apiPath = "/api/Restaurants"
            place.Cuisine = $('#newRestaurantCuisine').val()
            break;
        case 'attraction':
            apiPath = "/api/Attractions"
            break;
    }
    let jsonMsg
    if (editing === true) {
        apiPath = apiPath + "/update"
        place.id = selectedPlace.id
        temp = [selectedPlace, place]
        jsonMsg = JSON.stringify(temp)
    } else {
        jsonMsg = JSON.stringify(place)
    }
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            if (editing === true) {
                $('.wait-screen').addClass('d-none');
                alert("Place updated successfuly");
            } else {
                $('.wait-screen').addClass('d-none');
                alert("New place added successfuly");
            }
            window.location.assign("plan_trip.html");
        }
        else {
            $('.wait-screen').addClass('d-none');
            alert(this.response);
        }
    };
    // generate and send the request to the server of register new account
    xhttp.open("POST", apiPath);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.send(jsonMsg);
}

// sets "editing" variable to true and show the new-place modal
function updatePlace() {
    editing = true
    showNewPlaceModal()
}

// send a delete request of the selected place to the DB
function deletePlace() {
    let apiPath
    switch (selectedPlaceType) {
        case 'accommodation':
            apiPath = '/api/Accommodation'
            break;
        case 'restaurant':
            apiPath = '/api/Restaurants'
            break;
        case 'attraction':
            apiPath = '/api/Attractions'
            break;
    }
    let jsonMsg = JSON.stringify(selectedPlace)
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            alert("Place deleted successfuly");
            window.location.assign("plan_trip.html");
        }
        else {
            alert(this.response);
        }
    };
    // generate and send the request to the server of register new account
    xhttp.open("DELETE", apiPath);
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.send(jsonMsg);
}

// clear the new-place form, usually followed by showNewPlaceModal()
function clearNewPlaceForm() {
    editing = false
    $('#newPlaceName, #newPlaceLatitude, #newPlaceLongitude, #newPlaceType, #newPlacePhone, #newRestaurantCuisine #newAccommodationInternet, #newAccommodationType').val('')
    $('#newPlaceType').prop('disabled', false)
    $('.form-phone').addClass('d-none')
    $('.form-accommodation').addClass('d-none')
    $('.form-restaurant').addClass('d-none')
    $('.form-attraction').addClass('d-none')
}

// load into the new-place form the selected place, usually followed by showNewPlaceModal()
function loadNewPlaceForm() {
    editing = true
    $('#newPlaceName').val(selectedPlace.name)
    $('#newPlaceLatitude').val(selectedPlace.location.coordinates.latitude)
    $('#newPlaceLongitude').val(selectedPlace.location.coordinates.longitude)
    switch (selectedPlaceType) {
        case 'accommodation':
            $('#newPlaceType').val('accommodation')
            $('.form-accommodation').removeClass('d-none')
            $('.form-restaurant').addClass('d-none')
            $('.form-attraction').addClass('d-none')
            $('#newAccommodationInternet').val(selectedPlace.internet)
            $('#newAccommodationType').val(selectedPlace.type)
            break;
        case 'restaurant':
            $('#newPlaceType').val('restaurant')
            $('.form-accommodation').addClass('d-none')
            $('.form-restaurant').removeClass('d-none')
            $('.form-attraction').addClass('d-none')
            $('#newRestaurantCuisine').val(selectedPlace.cuisine)
            break;
        case 'attraction':
            $('#newPlaceType').val('attraction')
            $('.form-accommodation').addClass('d-none')
            $('.form-restaurant').addClass('d-none')
            $('.form-attraction').removeClass('d-none')
            break;
    }
    $('#newPlaceType').prop('disabled', true)
    $('.form-phone').removeClass('d-none')
    $('#newPlacePhone').val(selectedPlace.phone)
}

// show the "please wait" screen
function showWaitScreen() {
    $('.wait-screen').removeClass('d-none')
}

// hide the "please wait" screen
function hideWaitScreen() {
    $('.wait-screen').addClass('d-none')
}

// check if values in the form are valid before sending to DB, show a help section for invalid values
function formIsValid() {
    valid = true
    placeNameRegex = /^[^'"\t\n\r]{3,}$/g
    isLatitude = /^-?\d+\.?\d*$/g
    isLongitude = /^-?\d+\.?\d*$/g
    if (!placeNameRegex.test($('#newPlaceName').val())) {
        valid = false;
        $('#newPlaceNameHelp').show()
    }
    if (!(isLatitude.test($('#newPlaceLatitude').val()) && isLongitude.test($('#newPlaceLongitude').val()))) {
        valid = false;
        $('#newPlaceCoordinatesHelp').show()
    }
    if ($('#newPlaceType').val() === null) {
        valid = false;
        $('#newPlaceTypeHelp').show()
    }
    return valid
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
var loadedTrip = localStorage.getItem("loadedTrip")
if (loadedTrip !== "") {
    loadedTrip = JSON.parse(loadedTrip)
}
var selectedPlace = null
var selectedPlaceType = null
var editing = false
var attTravelsMap = new Map()
var resTravelsMap = new Map()
var accTravelsMap = new Map()
var attIdNameMap = new Map()
var resIdNameMap = new Map()
var accIdNameMap = new Map()
window.onload = function () {
    showAccommodations();
    showRestaurants();
    showAttractions();
    askForAccTravelsMap();
    askForResTravelsMap();
    askForAttTravelsMap();
    initMap()
}
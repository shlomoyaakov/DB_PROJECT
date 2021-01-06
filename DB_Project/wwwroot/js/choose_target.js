function loadChosenTrip() {
    if (selectedCountry === "" || selectedCity === "") {
        alert("Please select a trip")
        return
    }
    localStorage.setItem("country", selectedCountry);
    localStorage.setItem("city", selectedCity);
    window.location.assign("plan_trip.html");
}

function tripHistoryClicked() {
    if (!historyLoaded) {
        askForTripHistory(user_name)
        historyLoaded = true;
    }
}

function tripHistoryLoaded() {
    for (let item of userTrips) {
        let country = item.country
        let city = item.city
        let time = item.time
        var option = document.createElement("option")
        option.text = "country: "+country + ", city: " + city+", time: "+time
        option.value = JSON.stringify(item)
        tripsList.add(option, tripsList[0]);
        option.style ="font-size:30px"
        option.onclick = function (){
            valueJSON = JSON.parse(this.value)
            //document.getElementById("tripDetails").innerHTML = valueJSON
            showDetails(valueJSON)
        }
    }
}

function showDetails(valueJSON) {
    localStorage.setItem("loadedTrip", JSON.stringify(valueJSON));
    selectedCountry = valueJSON.country
    selectedCity = valueJSON.city
    tripDetails = "<b>country:</b> " + selectedCountry + ", <b>city:</b> " + selectedCity + ", <b>time:</b> " + valueJSON.time
    tripDetails += "<br><br><b>Accommodations: </b>"
    acco = valueJSON.accommodation
    accos = ""
    for (i in acco) {
        accos += acco[i].name + ", "
    }
    if (accos.length !== 0) {
        accos = accos.slice(0, -2)
    }
    tripDetails += accos
    tripDetails += "<br><br><b>Restaurants: </b>"
    rest = valueJSON.restaurants
    rests = ""
    for (i in rest) {
        rests += rest[i].name + ", "
    }
    if (rests.length !== 0) {
        rests = rests.slice(0, -2)
    }
    tripDetails += rests
    tripDetails += "<br><br><b>Attractions: </b> "
    att = valueJSON.attractions
    atts = ""
    for (i in att) {
        atts += att[i].name + ", "
    }
    if (atts.length !== 0) {
        atts = atts.slice(0, -2)
    }
    tripDetails += atts
    document.getElementById("tripDetails").innerHTML = tripDetails
}

function askForTripHistory(username) {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse =  JSON.parse(this.response);
            for (i in jsonResponse) {
                userTrips.add(jsonResponse[i])
            }
            tripHistoryLoaded()
        }
        else {
            alert(this.response);
        }
    };
    // ask the server for countries & cities list
    xhttp.open("GET", "/api/Trips/user?user_name=" + username);
    xhttp.send();
}

function hideModal(){
    modal.style.display = "none";
}
function loadCountries() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            // find all countries
            for (i in jsonResponse) {
                let country = jsonResponse[i].country;
                countriesSet.add(country)
            }
            initCountriesComboBox()
            askForCities()
            document.getElementById("mainDiv").classList.remove("d-none")
            document.getElementById("loader").remove()
        }
        else {
            alert('Error occure while getting regions from server');
        }
    };
    // ask the server for countries 
    xhttp.open("GET", "/api/Region");
    xhttp.send();
}

function initCountriesComboBox() {
    // init the countries combo box
    if (countriesComboBox === null) {
        return;
    }
    let countriesArr = Array.from(countriesSet)
    for (let i = 0; i < countriesArr.length; i++) {
        let country = countriesArr[i];
        countriesComboBox.options[countriesComboBox.options.length] = new Option(country, 0);
    }
}
function askForCities() {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            citiesSet = new Set()
            jsonResponse = JSON.parse(this.response);
            // find all countries
            for (i in jsonResponse) {
                let country = jsonResponse[i].city;
                citiesSet.add(country)
            }
            initCitiesComboBox(citiesSet)
        }
        else {
            alert(this.response);
        }
    };
    // ask the server for countries 
    selectedCountry = countriesComboBox.options[countriesComboBox.selectedIndex].text
    xhttp.open("GET", "/api/Region/country?country=" + selectedCountry);
    xhttp.send();
}

function initCitiesComboBox(citiesSet) {
    $("#citiesId").empty();
    for (let item of citiesSet) {
        citiesComboBox.options[citiesComboBox.options.length] = new Option(item, 0);
    }
}

function planTripClicked() {
    localStorage.setItem("country", countriesComboBox.options[countriesComboBox.selectedIndex].text);
    localStorage.setItem("city", citiesComboBox.options[citiesComboBox.selectedIndex].text);
    localStorage.setItem("newPlaces", document.getElementById('newPlacesID').checked)
    localStorage.setItem("loadedTrip", "");
    window.location.assign("plan_trip.html");
}

var userTrips = new Set()
var historyLoaded = false
var loginUser = "";
var countriesComboBox;
var citiesComboBox;
var selectedCountry = ""
var selectedCity = ""

var countriesSet = new Set();
var user_name = localStorage.getItem("user");

countriesComboBox = document.getElementById('countriesId');
citiesComboBox = document.getElementById('citiesId');
var modal = document.getElementById("myModal");
var tripsList = document.getElementById("historyTrips");

// Get the button that opens the modal
var btn = document.getElementById("historyBtn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

// When the user clicks on the button, open the modal
btn.onclick = function () {
    modal.style.display = "block";
    tripHistoryClicked()
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    hideModal()
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
window.onload = function () {
    document.getElementById('helloId').innerHTML = "Hello " + user_name;
    loadCountries();
}



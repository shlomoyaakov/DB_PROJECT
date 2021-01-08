function infoClicked() {
    // info icon clicked, get the selected city statistic and show it to user on infoModal
    var option = citiesComboBox.options[citiesComboBox.selectedIndex]
    var stats = JSON.parse(option.value)
    var country = countriesComboBox.options[countriesComboBox.selectedIndex].text
    var city = citiesComboBox.options[citiesComboBox.selectedIndex].text
    var acc = stats.Accommodation
    var att = stats.Attractions
    var res = stats.Restaurants
    var trips = stats.Trips
    var info = city + ", " + country + "<br><br>Trips Planned: " + trips + "<br>Accommodations: " + acc + "<br>Restaurants: " + res + "<br>Attrations: " + att
    document.getElementById("infoHeader").innerHTML = info
    document.getElementById("infoModal").style.display = "block";
}

function hideInfoModal() {
    document.getElementById("infoModal").style.display = "none";
}

function loadChosenTrip() {
    // user choose to load saved trip, save selected country and city and move to plan_trip page
    if (selectedCountry === "" || selectedCity === "") {
        alert("Please select a trip")
        return
    }
    localStorage.setItem("country", selectedCountry);
    localStorage.setItem("city", selectedCity);
    window.location.assign("plan_trip.html");
}

function tripHistoryClicked() {
    // user click on "View trip history", if its the first time, ask trip history data of this user from server
    if (!historyLoaded) {
        askForTripHistory(user_name)
        historyLoaded = true;
    }
}

function askForTripDelete() {
    if (selectedTripId !== "") {
        let xhttp = new XMLHttpRequest();
        // the user ask to delete specific trip
        xhttp.open("DELETE", "/api/Trips/id?id=" + selectedTripId);
        // on server response
    
        xhttp.onloadend = function () {
            if (this.readyState == 4 && this.status == 200) {
                document.getElementById(selectedTripId).remove()
                document.getElementById("tripDetails").innerHTML = ""
            }
            else {
                alert(this.response);
            }
        };
        xhttp.send();
    }
}

function askForClearTrips() {
    // the user ask to delete all of his trips
    let xhttp = new XMLHttpRequest();
    // create json object to send to server
    var user = {};
    user.User_Name = user_name
    user.Password = password
    user.Admin = isAdmin
    var json = JSON.stringify(user);
    user = JSON.stringify(user)
    xhttp.open("DELETE", "/api/Trips", true);
    xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');

    //on server response     
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            $("#historyTrips").empty();
            document.getElementById("tripDetails").innerHTML = ""
        }
        else {
            alert("fail to delete trip history");
        }
    };

    xhttp.send(json);
}



function tripHistoryLoaded() {
    // for each trip in userTrips, add it to tripsList, with click event what show trip details when clicked
    for (let item of userTrips) {
        let country = item.country
        let city = item.city
        let time = item.time
        var option = document.createElement("option")
        option.text = time + ", country: " + country + ", city: " + city
        option.value = JSON.stringify(item)
        tripsList.add(option, tripsList[0]);
        option.setAttribute("id", item.id)
        option.style = "font-size:15px"
        option.onclick = function () {
            valueJSON = JSON.parse(this.value)
            showDetails(valueJSON)
        }
    }
}

function showDetails(valueJSON) {
    // when trip is clicked in the history trip list, get its details and build 'tripDetails' string, and show it to user in the right side
    localStorage.setItem("loadedTrip", JSON.stringify(valueJSON));
    selectedTripId = valueJSON.id
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
    // ask the server for this user trips history
    xhttp.open("GET", "/api/Trips/user?user_name=" + username);
    // on server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            // add each trip to userTrips
            for (i in jsonResponse) {
                userTrips.add(jsonResponse[i])
            }
            tripHistoryLoaded()
        }
        else {
            alert("fail to get trips history from server");
        }
    };
    xhttp.send();
}

function hideModal() {
    modal.style.display = "none";
}

function loadCountries() {
    // ask the server for the list of countries
    let xhttp = new XMLHttpRequest();
    xhttp.open("GET", "/api/Region");

    // on server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            // add all countries to countriesSet
            for (i in jsonResponse) {
                let country = jsonResponse[i].country;
                countriesSet.add(country)
            }
            // set CountriesComboBox and ask for the selected country list of cities stats (names and info about each city)
            initCountriesComboBox()
            askForCities()
            // remove 'please wait modal' and show the main page
            document.getElementById("mainDiv").classList.remove("d-none")
            document.getElementById("loader").remove()
        }
        else {
            // request failed
            alert('Error occure while getting regions from server');
        }
    };

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
    // show pleaseWaitModal until cities are loaded
    document.getElementById("pleaseWaitModal").style.display = "block";
    let xhttp = new XMLHttpRequest();

    // get the selected country
    selectedCountry = countriesComboBox.options[countriesComboBox.selectedIndex].text
    // ask for cities stats for this country
    xhttp.open("GET", "/api/Region/stats_per_region?country=" + selectedCountry);

    // on server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            citiesSet = new Set()
            jsonResponse = JSON.parse(this.response);
            // add each city to citiesSet
            for (i in jsonResponse) {
                var stats = {}
                stats.city = jsonResponse[i].general_Location.city;
                stats.data = jsonResponse[i].data;
                citiesSet.add(stats)
            }
            // init CitiesComboBox
            initCitiesComboBox(citiesSet)
        }
        else {
            alert(this.response);
        }
    };

    xhttp.send();
}

function initCitiesComboBox(citiesSet) {
    // clear cities combo box and init it with the new citiesSet data
    $("#citiesId").empty();
    for (let item of citiesSet) {
        option = new Option(item.city, 0)
        option.value = JSON.stringify(item.data)
        citiesComboBox.options[citiesComboBox.options.length] = option;
    }
    // remove pleaseWaitModal
    document.getElementById("pleaseWaitModal").style.display = "none";
}

function planTripClicked() {
    // user clicked on plan trip, save country,city, and newPlaces checkbox and move to plan_trip page
    localStorage.setItem("country", countriesComboBox.options[countriesComboBox.selectedIndex].text);
    localStorage.setItem("city", citiesComboBox.options[citiesComboBox.selectedIndex].text);
    localStorage.setItem("newPlaces", document.getElementById('newPlacesID').checked)
    localStorage.setItem("loadedTrip", ""); // new trip and no old trip editing
    window.location.assign("plan_trip.html");
}

function checkIfAdmin() {
    // check if the user is admin by asking the server to check it 
    let xhttp = new XMLHttpRequest();
    xhttp.open("GET", "/api/Users/admin?username=" + user_name + "&password=" + password);

    // on server response 
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            isAdmin = (this.response === "true")
        }
        else {
            alert("isAdmin request failed, default isAdmin set to false");
        }
        loadCountries();
    };

    xhttp.send();
}


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

var userTrips = new Set()
var historyLoaded = false
var loginUser = "";
var isAdmin = false
var selectedCountry = ""
var selectedCity = ""
var selectedTripId = ""
var countriesSet = new Set();
var user_name = localStorage.getItem("user");
var password = localStorage.getItem("pass");
var countriesComboBox = document.getElementById('countriesId');
var citiesComboBox = document.getElementById('citiesId');
var modal = document.getElementById("myModal");
var tripsList = document.getElementById("historyTrips");
window.onload = function () {
    // when the page loaded, ask the server if the user is admin and continue to loadCountries()
    document.getElementById('helloId').innerHTML = "Hello " + user_name;
    checkIfAdmin()
}

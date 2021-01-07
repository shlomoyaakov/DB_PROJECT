function infoClicked() {
    var option = citiesComboBox.options[citiesComboBox.selectedIndex]
    var stats = JSON.parse(option.value)
    var country = countriesComboBox.options[countriesComboBox.selectedIndex].text
    var city = citiesComboBox.options[citiesComboBox.selectedIndex].text
    var acc = stats.Accommodation
    var att = stats.Attractions
    var res = stats.Restaurants
    var info = city + ", " + country + "<br>Accommodations: " + acc + "<br>Restaurants: " + res + "<br>Attrations: " + att
    document.getElementById("infoHeader").innerHTML = info
    document.getElementById("infoModal").style.display = "block";
}
function hideInfoModal() {
    document.getElementById("infoModal").style.display = "none";
}
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
function askForTripDelete() {
    let xhttp = new XMLHttpRequest();
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById(selectedTripId).remove()
            document.getElementById("tripDetails").innerHTML=""
        }
        else {
            alert(this.response);
        }
    };
    if (selectedTripId !== "") {
        xhttp.open("DELETE", "/api/Trips/id?id=" + selectedTripId);
        xhttp.send();
    }
}

function askForClearTrips() {
    let xhttp = new XMLHttpRequest();
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            $("#historyTrips").empty();
            document.getElementById("tripDetails").innerHTML = ""
        }
        else {
            alert(this.response);
        }
    };


    var user = {};
    user.User_Name = user_name
    user.Password = password
    user.Admin = isAdmin
    var json = JSON.stringify(user);

    user = JSON.stringify(user)
    xhttp.open("DELETE", "/api/Trips", true);
    xhttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
    xhttp.send(json);
}



function tripHistoryLoaded() {
    for (let item of userTrips) {
        let country = item.country
        let city = item.city
        let time = item.time
        var option = document.createElement("option")
        option.text = time+", country: "+country + ", city: " + city
        option.value = JSON.stringify(item)
        tripsList.add(option, tripsList[0]);
        option.setAttribute("id", item.id)
        option.style ="font-size:15px"
        option.onclick = function (){
            valueJSON = JSON.parse(this.value)
            //document.getElementById("tripDetails").innerHTML = valueJSON
            showDetails(valueJSON)
        }
    }
}

function showDetails(valueJSON) {
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
    document.getElementById("pleaseWaitModal").style.display = "block";
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            citiesSet = new Set()
            jsonResponse = JSON.parse(this.response);
            // find all countries
            for (i in jsonResponse) {
                var stats = {}
                stats.city = jsonResponse[i].general_Location.city;
                stats.data = jsonResponse[i].data;
                citiesSet.add(stats)
            }
            initCitiesComboBox(citiesSet)
        }   
        else {
            alert(this.response);
        }
    };
    // ask the server for countries 
    selectedCountry = countriesComboBox.options[countriesComboBox.selectedIndex].text
    xhttp.open("GET", "/api/Region/stats_per_region?country=" + selectedCountry);
    xhttp.send();
}

function initCitiesComboBox(citiesSet) {
    $("#citiesId").empty();
    for (let item of citiesSet) {
        option = new Option(item.city, 0)
        option.value = JSON.stringify(item.data)
        citiesComboBox.options[citiesComboBox.options.length] = option;

    }
    document.getElementById("pleaseWaitModal").style.display = "none";
}

function planTripClicked() {
    localStorage.setItem("country", countriesComboBox.options[countriesComboBox.selectedIndex].text);
    localStorage.setItem("city", citiesComboBox.options[citiesComboBox.selectedIndex].text);
    localStorage.setItem("newPlaces", document.getElementById('newPlacesID').checked)
    localStorage.setItem("loadedTrip", "");
    window.location.assign("plan_trip.html");
}
function checkIfAdmin() {
    let xhttp = new XMLHttpRequest();
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            isAdmin = (this.response === "true")
        }
        else {
            alert(this.response);
        }
    };
    xhttp.open("GET", "/api/Users/admin?username=" + user_name + "&password=" + password);
    xhttp.send();
}
var userTrips = new Set()
var historyLoaded = false
var loginUser = "";
var isAdmin= false
var countriesComboBox;
var citiesComboBox;
var selectedCountry = ""
var selectedCity = ""
var selectedTripId =""
var countriesSet = new Set();
var user_name = localStorage.getItem("user");
var password = localStorage.getItem("pass");
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
    checkIfAdmin()
    loadCountries();
}




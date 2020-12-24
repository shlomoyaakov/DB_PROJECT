function loadCountriesAndCities() {
    countriesComboBox = document.getElementById('countriesId');
    citiesComboBox = document.getElementById('citiesId');

    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            // find all countries
            for (i in jsonResponse) {
                let country = jsonResponse[i].country;
                let city = jsonResponse[i].city;
                countriesCitiesSet.add([city, country])
                countriesSet.add(country);
            }
            initCountriesComboBox();
            initCitiesComboBox();
        }
        else {
            alert('Error occure while getting regions from server');
        }
    };
    // ask the server for countries & cities list
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

function initCitiesComboBox() {
    if (countriesComboBox === null) {
        return;
    }
    $("#citiesId").empty();
    let selectedCountry = countriesComboBox.options[countriesComboBox.selectedIndex].text;
    let countriesCitiesArr = Array.from(countriesCitiesSet)
    for (let i = 0; i < countriesCitiesArr.length; i++) {
        let currCity = countriesCitiesArr[i][0];
        let currCountry = countriesCitiesArr[i][1];
        if (currCountry === selectedCountry) {
            citiesComboBox.options[citiesComboBox.options.length] = new Option(currCity, 0);
        }
    }
}

function planTripClicked() {
    localStorage.setItem("country", countriesComboBox.options[countriesComboBox.selectedIndex].text);
    localStorage.setItem("city", citiesComboBox.options[citiesComboBox.selectedIndex].text);
    window.location.replace("plan_trip.html");
}



var loginUser = "";
var countriesComboBox;
var citiesComboBox;
var countriesSet = new Set();
var countriesCitiesSet = new Set();

window.onload = function () {
    document.getElementById('helloId').innerHTML = "Hello " + localStorage.getItem("user");
    loadCountriesAndCities();
}
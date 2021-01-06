//function newPlacesClicked() {
//    if (document.getElementById('newPlacesID').checked) {
//        var selectedCountry = countriesComboBox.options[countriesComboBox.selectedIndex].text

//        for (var i = 0; i < citiesComboBox.length; i++) {
//            var currCity = citiesComboBox.options[i].text
//            if (visitedPlaces.has(selectedCountry+","+currCity)) {
//                citiesComboBox.options[i].disabled = true
//                citiesComboBox.options[i].style.backgroundColor = 'gray'
//            }
//        }
//    } else {
//        for (var i = 0; i < citiesComboBox.length; i++) {
//            citiesComboBox.options[i].disabled = false
//            citiesComboBox.options[i].style.backgroundColor = 'white'
//        }
//    }
//}

//function loadPrevTrips() {
//    let xhttp = new XMLHttpRequest();
//    // server respose
//    xhttp.onloadend = function () {
//        if (this.readyState == 4 && this.status == 200) {
//            jsonResponse = JSON.parse(this.response);
//            for (i in jsonResponse) {
//                item = jsonResponse[i]
//                // should be country and city !@@@@@@@@@@@@@@@@@@@@@@@@@
//               // visitedPlaces.add(item.country+","+item.city)
//                visitedPlaces.add("France,Abbaretz")
//                visitedPlaces.add("France,Paris")
//            }
//        }
//        else {
//            alert(this.response);
//        }
//    };
//    // ask the server for prev trips that the user already plan
//    xhttp.open("GET", "/api/Trips/user?user_name=" + user_name);
//    xhttp.send();
//}

function tripHistoryClicked() {
    if (!historyLoaded) {
        let userTrips = askForTripHistory(user_name)
        historyLoaded = true;
    }


}
function askForTripHistory(username) {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            jsonResponse = JSON.parse(this.response);
            // find all countries
            for (i in jsonResponse) {
                //let country = jsonResponse[i].country;
                //let city = jsonResponse[i].city;
                //countriesCitiesSet.add([city, country])
                //countriesSet.add(country);
            }
            //initCountriesComboBox();
            //initCitiesComboBox();
            //document.getElementById("mainDiv").classList.remove("d-none")
            //document.getElementById("loader").remove()
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
            document.getElementById("mainDiv").classList.remove("d-none")
            document.getElementById("loader").remove()
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
    //if (document.getElementById('newPlacesID').checked) {
    //    newPlacesClicked()
    //}
}

function planTripClicked() {
    localStorage.setItem("country", countriesComboBox.options[countriesComboBox.selectedIndex].text);
    localStorage.setItem("city", citiesComboBox.options[citiesComboBox.selectedIndex].text);
    localStorage.setItem("newPlaces", document.getElementById('newPlacesID').checked)
    window.location.assign("plan_trip.html");
}


var historyLoaded = false
var loginUser = "";
var countriesComboBox;
var citiesComboBox;
//var visitedPlaces = new Set();
var countriesSet = new Set();
var countriesCitiesSet = new Set();
var user_name = localStorage.getItem("user");
// Get the modal
var modal = document.getElementById("myModal");

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
    loadCountriesAndCities();

}
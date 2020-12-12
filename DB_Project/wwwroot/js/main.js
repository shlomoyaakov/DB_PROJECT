function signUpClicked() {
    let username = document.getElementById("usernameId").value;
    let password = document.getElementById("passwordId").value;
    let repeatPassword = document.getElementById("repeatPasswordId").value;
    let error = isValidUserName(username);
    if (error !== "") {
        alert(error);
        return;
    }

    if (!(password === repeatPassword)) {
        alert("password do not match");
        return;
    }
    if (!isValidPassword(password)) {
        alert("invalid password, please enter at least 6 digits password");
        return;
    }
    // pass all sign up checks, open create account request from server
    createAccountRequest(username, password);
}

function createAccountRequest(username, password) {
    let jsonMsg = createJson(username, password);
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            alert("account created successfully");
            window.location.replace("sign_in.html");
        }
        else {
            alert(this.response);
        }
    };
    // generate and send the request to the server of register new account
    xhttp.open("POST", "/api/Users");
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhttp.send(jsonMsg);
}

function createJson(username, password) {
    const json = {
        "User_Name": username,
        "Password": password
    };
    return JSON.stringify(json)
}





function isValidUserName(username) {
    let error = "";
    let illegalChars = /\W/; // allow letters, numbers, and underscores

    if (username === "") {
        error = "Please enter Username";
    } else if ((username.length < 5) || (username.length > 15)) {
        error = "Username must have 5-15 characters";
    } else if (illegalChars.test(username)) {
        error = "Please enter valid Username. Use only numbers and alphabets";
    } else {
        error = "";
    }
    return error;
}


function isValidPassword(password) {
    return password.length >= 6;
}



function signInClicked() {
    let username = document.getElementById("usernameId").value;
    let password = document.getElementById("passwordId").value;
    let error = isValidUserName(username);
    if (error !== "") {
        alert("Invalid user name");
        return;
    }
    if (!isValidPassword(password)) {
        alert("Invalid password");
        return;
    }
    // pass all sign up checks, go to login
    tryToLogin(username, password);
}

function tryToLogin(username, password) {
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            if (this.response === "true") {
                loginUser = username;
                window.location.replace("choose_target.html");
            } else {
                alert("username or password is incorrect");
            }
        }
        else {
            alert(this.response);
        }
    };
    // generate and send the request to the server of register new account
    let url = "/api/Users?" + "username=" + username + "&password=" + password;
    xhttp.open("GET", url);
    xhttp.send();
}

function loadCountriesAndCities() {
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
    let countriesArr = Array.from(countriesSet)
    for (let i = 0; i < countriesArr.length; i++) {
        let country = countriesArr[i];
        countriesComboBox.options[countriesComboBox.options.length] = new Option(country, 0);
    }
}

function initCitiesComboBox() {
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


let loginUser = "";
let countriesComboBox = document.getElementById('countriesId');
let citiesComboBox = document.getElementById('citiesId');

let countriesSet = new Set();
let countriesCitiesSet = new Set();
loadCountriesAndCities();
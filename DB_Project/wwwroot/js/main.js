let loginUser="";


function sendPassFunction() {
    // if the mail is valid, send email to user with his password
    var status = document.getElementById("statusParagraph");
    status.innerHTML = "good\\bad";
}

function signUpClicked() {
    var username = document.getElementById("usernameId").value;
    var email = document.getElementById("emailId").value;
    var password = document.getElementById("passwordId").value;
    var repeatPassword = document.getElementById("repeatPasswordId").value;
    var error = isValidUserName(username);
    if (error !== "") {
        alert(error);
        return;
    }
    if (!isValidEmail(email)) {
        alert("invalid email");
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
    createAccountRequest(username, email, password);
}

function createAccountRequest(username, email, password) {
    let jsonMsg = createJson(username, email, password);
    let xhttp = new XMLHttpRequest();
    // server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            alert("Account creation succeeded");
            window.location.replace("index.html");
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

function createJson(username, email, password) {
    const json = {
        "User_Name": username ,
        "Password":  password ,
        "Email":  email 
    };
    return JSON.stringify(json)
}





function isValidUserName(username) {
    var error = "";
    var illegalChars = /\W/; // allow letters, numbers, and underscores

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

function isValidEmail(email) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

function isValidPassword(password) {
    return password.length >= 6;
}



function signInClicked() {
    var username = document.getElementById("usernameId").value;
    var password = document.getElementById("passwordId").value;
    var error = isValidUserName(username);
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
                window.location.replace("main_page.html");
            } else {
                alert("username or password is incorrect");
            }
        }
        else {
            alert(this.response);
        }
    };
    // generate and send the request to the server of register new account
    var url = "/api/Users?" + "username=" + username + "&password=" + password;
    xhttp.open("GET", url);
    xhttp.send();
}
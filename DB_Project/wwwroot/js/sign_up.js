function signUpClicked() {
    let username = document.getElementById("usernameId").value;
    let password = document.getElementById("passwordId").value;
    let repeatPassword = document.getElementById("repeatPasswordId").value;
    // check input validation
    if (isValidUserName(username) !== "") {
        alert(error);
        return;
    }

    if (!(password === repeatPassword)) {
        alert("password do not match");
        return;
    }
    if (!isValidPassword(password)) {
        alert("invalid password, password lenght should be between 6 to 15 characters");
        return;
    }
    // pass all sign up checks, open a create account request from server
    createAccountRequest(username, password);
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
    return password.length >= 6 && password.length <= 15;
}


function createAccountRequest(username, password) {
    // create json object from input parameters
    let jsonMsg = createJson(username, password);
    let xhttp = new XMLHttpRequest();

    // generate and send the request to the server of register new account
    xhttp.open("POST", "/api/Users");
    xhttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    // on server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            // account created successfully, move back to sign in page
            alert("account created successfully");
            window.location.assign("sign_in.html");
        }
        else {
            // account created failed, show message to user
            if (this.response.includes("Duplicate")) {
                alert("The username already exists. Please use a different username")
            } else {
                alert(this.response);
            }
        }
    };
    xhttp.send(jsonMsg);
}

// create json object from input parameters
function createJson(username, password) {
    const json = {
        "User_Name": username,
        "Password": password
    };
    return JSON.stringify(json)
}




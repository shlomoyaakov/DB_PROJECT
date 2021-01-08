function signInClicked() {
    let username = document.getElementById("usernameId").value;
    let password = document.getElementById("passwordId").value;

    if (isValidUserName(username) !== "") {
        alert("Invalid user name");
        return;
    }

    if (!isValidPassword(password)) {
        alert("Invalid password");
        return;
    }
    // pass all sanity checks, try to log in
    tryToLogin(username, password);
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
    return password.length >= 6 && password.length <= 15
}

function tryToLogin(username, password) {
    let xhttp = new XMLHttpRequest();
    // generate and send sign in request to the server
    let url = "/api/Users?" + "username=" + username + "&password=" + password;
    xhttp.open("GET", url);

    // on server respose
    xhttp.onloadend = function () {
        if (this.readyState == 4 && this.status == 200) {
            if (this.response === "true") {
                // login successful, save the username and password for next screens logic and move to next screen
                localStorage.setItem("user", username);
                localStorage.setItem("pass", password);
                window.location.assign("choose_target.html");
            } else {
                // login failed
                alert("username or password is incorrect");
            }
        }
        else {
            alert(this.response);
        }
    };

    xhttp.send();
}






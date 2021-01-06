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
                localStorage.setItem("user", username);
                localStorage.setItem("pass", password);
                window.location.assign("choose_target.html");
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






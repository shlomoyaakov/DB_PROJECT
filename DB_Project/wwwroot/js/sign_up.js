
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
                window.location.assign("sign_in.html");
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





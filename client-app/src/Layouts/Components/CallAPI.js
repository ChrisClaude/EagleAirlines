import React, { useState } from "react";
import { UserManager } from "oidc-client";

function CallAPI() {
    const [results, setResults] = useState("");

    const config = {
        authority: "https://localhost:5001",
        client_id: "js",
        redirect_uri: "http://localhost:3000/callback",
        response_type: "code",
        scope: "openid profile BookingAPI",
        post_logout_redirect_uri: "http://localhost:3000/"
    };

    const mgr = new UserManager(config);

    mgr.getUser().then(function(user) {
        if (user) {
            console.log("User logged in", user.profile);
        } else {
            console.log("User not logged in");
        }
    });

    const login = () => {
        mgr.signinRedirect();
        console.log("login");
    };

    const api = () => {
        console.log("api");
        mgr.getUser().then(function(user) {
            let url = "https://localhost:6001/identity";

            let xhr = new XMLHttpRequest();
            xhr.open("GET", url);
            xhr.onload = function() {
                console.log(xhr.status, JSON.parse(xhr.responseText));
            };
            xhr.setRequestHeader(
                "Authorization",
                "Bearer " + user.access_token
            );
            xhr.send();
        });
    };

    const logout = () => {
        console.log("logout");
        mgr.signoutRedirect();
    };

    return (
        <div>
            <button onClick={login}>Login</button>
            <button onClick={api}>Call API</button>
            <button onClick={logout}>Logout</button>

            <p>{results}</p>
        </div>
    );
}

export default CallAPI;

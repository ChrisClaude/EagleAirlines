import React from "react";
import { UserManager } from "oidc-client";

const CallBack = () => {
    new UserManager({ response_mode: "query" })
        .signinRedirectCallback()
        .then(function() {
            window.location = "/";
        })
        .catch(function(e) {
            console.error(e);
        });

    return <div>CallBack Page</div>;
};

export default CallBack;

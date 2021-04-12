import React from "react";
import "./App.css";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

import Home from "./Layouts/Home";
import CallBack from "./Layouts/CallBack";

function App() {
    return (
        <Router>
            <Switch>
                <Route exact path="/" component={Home} />
                <Route exact path="/callback" component={CallBack} />
                <Route exact path="/about" render={() => <div>About</div>} />
            </Switch>
        </Router>
    );
}

export default App;

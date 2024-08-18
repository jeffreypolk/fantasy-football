import React, { useEffect } from "react";
import { Switch, BrowserRouter as Router } from "react-router-dom";
import { routes } from "./routes/";
import AppRoute from "./routes/AppRoute";
import Layout from "./components/common/layout/Layout";

function App() {
  const toggleCss = (toggle, css) => {
    if (toggle) {
      document.body.classList.add(css);
    } else {
      document.body.classList.remove(css);
    }
  };
  useEffect(() => {
    window.setTimeout(() => {
      toggleCss(false, "app-loading");
    }, 1000);
  });

  return (
    <Router>
      <Switch>
        {routes.map((route, idx) => (
          <AppRoute
            path={route.path}
            layout={route.layout ? route.layout : Layout}
            component={route.component}
            key={idx}
            isPublic={route.isPublic}
            exact={route.exact}
          />
        ))}
      </Switch>
    </Router>
  );
}

export default App;

import React from "react";
import { Redirect } from "react-router-dom";
import loadable from "@loadable/component";
import pMinDelay from "p-min-delay";
import Loader from "../components/common/Loader/Loader";
import BlankLayout from "../components/common/layout/BlankLayout";

const lazy = (cb) =>
  loadable(() => pMinDelay(cb(), 200), { fallback: <Loader /> });

const routes = [
  {
    path: "/home",
    component: lazy(() => import("../components/home/Home")),
    layout: BlankLayout,
  },
  {
    path: "/stats",
    component: lazy(() => import("../components/stats/Stats")),
    layout: BlankLayout,
  },
  {
    path: "/draft/admin",
    component: lazy(() => import("../components/draft/admin/DraftAdmin")),
    layout: BlankLayout,
  },
  {
    path: "/draft",
    component: lazy(() => import("../components/draft/Draft")),
    layout: BlankLayout,
  },
];

routes.push(
  // this route will catch the base url
  // use redirect here in case the user is not logged in
  {
    path: "/",
    exact: true,
    // eslint-disable-next-line react/display-name
    component: () => <Redirect to="/home" />,
  },
  // this will catch all unmatched routes
  {
    path: "/",
    component: lazy(() => import("../components/common/NotFound/NotFound")),
  }
);
export { routes };

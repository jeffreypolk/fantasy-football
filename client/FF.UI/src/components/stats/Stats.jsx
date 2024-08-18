import React from "react";
import { Tabs, Tab } from "react-bootstrap";
import LeagueStats from "./League";
import PlayerStats from "./Players";
import TeamStats from "./Teams";
import "./Stats.scss";

const Stats = () => {
  return (
    <div className="cmp-stats">
      <Tabs
        defaultActiveKey="league"
        id="uncontrolled-tab-example"
        className="mb-3 mt-3"
      >
        <Tab eventKey="league" title="League">
          <LeagueStats />
        </Tab>
        <Tab eventKey="teams" title="Teams">
          <TeamStats />
        </Tab>
        <Tab eventKey="players" title="Players">
          <PlayerStats />
        </Tab>
      </Tabs>
    </div>
  );
};

export default Stats;

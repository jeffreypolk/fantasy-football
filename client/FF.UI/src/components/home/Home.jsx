import React, { useEffect, useState } from "react";
import api from "../../services/api/index";
import Anchor from "../common/Anchor/Anchor";
import "./Home.scss";

const Home = () => {
  const [leagues, setLeagues] = useState([]);

  useEffect(() => {
    api.getLeagues().then((result) => {
      setLeagues(result.data);
    });
  }, []);

  return (
    <div className="page-content cmp-home">
      <div className="card-deck">
        {leagues.map((league) => {
          return (
            <div className="card text-center" key={league.id}>
              <div className="card-body">
                <h2 className="card-title">{league.name}</h2>
                <div className="card-text">
                  <div className="alert alert-success est">
                    Est. {league.established}
                  </div>
                </div>
              </div>
              <div className="card-footer text-muted">
                <Anchor href={"/stats?leagueid=" + league.id} text="Stats" />
                <Anchor href={"/draft?leagueid=" + league.id} text="Drafts" />
                {league.bylawsUrl && (
                  <Anchor
                    href={league.bylawsUrl}
                    text="Bylaws"
                    target="_blank"
                  />
                )}
                {league.votingUrl && (
                  <Anchor
                    href={league.votingUrl}
                    text="Voting Log"
                    target="_blank"
                  />
                )}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default Home;

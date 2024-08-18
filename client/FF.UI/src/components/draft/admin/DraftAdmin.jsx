import React, { useEffect, useState, useRef } from "react";
import { withRouter } from "react-router-dom";
import { Nav, Navbar, NavDropdown } from "react-bootstrap";
import api from "../../../services/api/index";
import "./DraftAdmin.scss";

const DraftAdmin = ({ history }) => {
  const [leagues, setLeagues] = useState([]);
  const [league, setLeague] = useState(null);
  const [teams, setTeams] = useState([]);
  const [team, setTeam] = useState(null);
  const [year, setYear] = useState(new Date().getFullYear());
  const [years, setYears] = useState([]);
  const [players, setPlayers] = useState([]);
  const [playerFilter, setPlayerFilter] = useState("");
  const [positions, setPositions] = useState([]);
  const playerFilterInput = useRef(null);

  const home = (event) => {
    event.preventDefault();
    history.push("/");
  };

  const onChangeLeague = (event) => {
    const newLeague = leagues.find(
      (l) => l.id === parseInt(event.target.dataset.leagueid)
    );
    if (newLeague) {
      setLeague(newLeague);
    }
  };

  const onChangeYear = (event) => {
    setYear(parseInt(event.target.dataset.year));
  };

  const onChangeTeam = (event) => {
    const newTeam = teams.find(
      (t) => t.id === parseInt(event.target.dataset.teamid)
    );
    if (newTeam) {
      setTeam(newTeam);
    }
  };

  const onChangePlayerFilter = (event) => {
    var value = event.target.value;
    setPlayerFilter(value);
  };

  const makePick = (event) => {
    const playerId = parseInt(event.target.dataset.playerid);
    const player = players.find((player) => {
      return player.id === playerId;
    });
    const teamPosition = team.draftOrder;
    const round = team.players.length + 1;
    const positionInfo = positions.find((pos) => {
      return pos.teamPosition === teamPosition && pos.round === round;
    });

    // copy the team
    const newTeam = { ...team };
    // copy the player
    const newPlayer = { ...player };
    // copy the players
    const newPlayers = [...players];

    // set up new mapping object
    const newPlayerTeam = {
      id: 0,
      isKeeper: false,
      overall: positionInfo.overall,
      player: newPlayer,
      playerId: newPlayer.id,
      round: round,
      teamId: newTeam.id,
    };
    // add mapping
    newTeam.players.push(newPlayerTeam);

    // update team state
    setTeam(newTeam);

    // remove this player
    newPlayers.splice(
      newPlayers.indexOf(
        newPlayers.find((player) => {
          return player.id === newPlayer.id;
        })
      ),
      1
    );

    // update players state
    setPlayers(newPlayers);

    // clear the player filter
    setPlayerFilter("");
    playerFilterInput.current.focus();

    // send to api
    api.postDraftPick({
      isKeeper: false,
      overall: newPlayerTeam.overall,
      playerId: newPlayerTeam.playerId,
      round: newPlayerTeam.round,
      teamId: newPlayerTeam.teamId,
    });
  };

  useEffect(() => {
    api.getLeagues().then((result) => {
      setLeagues(result.data);
      setLeague(result.data[0]);
    });
  }, []);

  useEffect(() => {
    if (league) {
      const newYears = [];
      for (var y = new Date().getFullYear(); y >= league.established; y--) {
        newYears.push(y);
      }
      setYears(newYears);
    }
  }, [league]);

  useEffect(() => {
    if (league) {
      api.getDraftTeams(league.id, year).then((result) => {
        setTeams(result.data);
        setTeam(result.data[0]);
      });

      api.getDraftPlayers(league.id, year).then((result) => {
        setPlayers(result.data);
      });

      api.getDraftPositions(league.id, year).then((result) => {
        setPositions(result.data);
      });
    }
  }, [league, year]);

  return (
    <div className="page-content cmp-draftadmin">
      <Navbar className="toolbar">
        <Nav className="me-auto">
          <Nav.Link onClick={home} className="nav-home">
            <i className="fas fa-home"></i>
          </Nav.Link>
          <NavDropdown title={league ? league.name : "League"}>
            {leagues.map((l) => {
              return (
                <NavDropdown.Item
                  onClick={onChangeLeague}
                  data-leagueid={l.id}
                  key={l.id}
                >
                  {l.name}
                </NavDropdown.Item>
              );
            })}
          </NavDropdown>
          <NavDropdown title={year}>
            {years.map((y) => {
              return (
                <NavDropdown.Item key={y} onClick={onChangeYear} data-year={y}>
                  {y}
                </NavDropdown.Item>
              );
            })}
          </NavDropdown>
          <NavDropdown title={team ? team.name : "Team"}>
            {teams.map((t, index) => {
              return (
                <NavDropdown.Item
                  key={index}
                  onClick={onChangeTeam}
                  data-teamid={t.id}
                >
                  {t.name}
                </NavDropdown.Item>
              );
            })}
          </NavDropdown>
        </Nav>
      </Navbar>

      <div className="row">
        <div className="col-md-6">
          <div className="card card-picks">
            <div className="card-body">
              <h5 className="card-title">Picks</h5>
              <div className="card-text">
                <table className="table table-sm">
                  <thead className="thead-light">
                    <tr>
                      <th>Round</th>
                      <th>Name</th>
                      <th>Pos</th>
                      <th>NFL Team</th>
                    </tr>
                  </thead>
                  <tbody>
                    {team &&
                      team.players.map((player, index) => {
                        return (
                          <tr key={index}>
                            <td>{player.round}</td>
                            <td
                              className={
                                "keeper-" +
                                player.isKeeper.toString().toLowerCase()
                              }
                            >
                              {player.player.name}
                              <span className="keeper-indicator">
                                &nbsp;(K)
                              </span>
                            </td>
                            <td>{player.player.position}</td>
                            <td>{player.player.nflTeam}</td>
                          </tr>
                        );
                      })}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-6">
          <div className="card card-players">
            <div className="card-body">
              <h5 className="card-title">
                <span>Available Players</span>
                <span>
                  <input
                    type="text"
                    className="form-control"
                    onChange={onChangePlayerFilter}
                    value={playerFilter}
                    ref={playerFilterInput}
                  ></input>
                </span>
              </h5>
              <div className="card-text">
                <table className="table table-sm">
                  <thead className="thead-light">
                    <tr>
                      <th>Name</th>
                      <th>Pos</th>
                      <th>NFL Team</th>
                    </tr>
                  </thead>
                  <tbody>
                    {players
                      .filter((player) => {
                        if (playerFilter) {
                          return player.name
                            .toLowerCase()
                            .includes(playerFilter.toLowerCase());
                        } else {
                          return true;
                        }
                      })
                      .map((player, index) => {
                        return (
                          <tr key={index}>
                            <td>
                              <button
                                data-playerid={player.id}
                                onClick={makePick}
                              >
                                {player.name}
                              </button>
                            </td>
                            <td>{player.position}</td>
                            <td>{player.nflTeam}</td>
                          </tr>
                        );
                      })}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default withRouter(DraftAdmin);

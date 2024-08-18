import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Nav, Navbar, NavDropdown } from "react-bootstrap";
import api from "../../services/api/index";
import qstring from "../../services/qstring/index";
import "./Draft.scss";
import Pick from "./Pick";

const Draft = ({ history }) => {
  const [leagues, setLeagues] = useState([]);
  const [league, setLeague] = useState(null);
  const [year, setYear] = useState(new Date().getFullYear());
  const [years, setYears] = useState([]);
  const [draft, setDraft] = useState(null);
  const [filter, setFilter] = useState("All");

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

  const onChangeFilter = (event) => {
    var value = event.target.dataset.filter;
    setFilter(value);
  };

  const displayPick = (pick, filter) => {
    if (filter == null || filter === "All") {
      return true;
    } else {
      if (pick.isKeeper) {
        return false;
      } else {
        if (pick.player.adp === 0 || pick.player.adp === 999) {
          return false;
        } else {
          if (filter === "Good") {
            return pick.overall - pick.player.adp > 10;
          } else {
            return pick.overall - pick.player.adp < -20;
          }
        }
      }
    }
  };
  const renderPick = (pick, filter) => {
    return <>{displayPick(pick, filter) && <Pick pick={pick} />}</>;
  };
  useEffect(() => {
    let initLeagueId = qstring.get("leagueid");
    api.getLeagues().then((result) => {
      //console.log(result);
      if (initLeagueId) {
        const initLeague = result.data.find(
          (l) => l.id === parseInt(initLeagueId)
        );
        if (initLeague) {
          setLeague(initLeague);
        } else {
          setLeague(result.data[0]);
        }
      } else {
        setLeague(result.data[0]);
      }
      setLeagues(result.data);
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
      api
        .getDraft(league.id, year)
        .then((result) => {
          setDraft(result.data);
          //console.log(result.data);
        })
        .catch((result) => {
          setDraft(null);
        });
    }
  }, [league, year]);

  return (
    <div className="page-content cmp-draft">
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
          <NavDropdown title={filter}>
            <NavDropdown.Item onClick={onChangeFilter} data-filter="All">
              All Picks
            </NavDropdown.Item>
            <NavDropdown.Item onClick={onChangeFilter} data-filter="Good">
              Good Picks
            </NavDropdown.Item>
            <NavDropdown.Item onClick={onChangeFilter} data-filter="Bad">
              Bad Picks
            </NavDropdown.Item>
            <NavDropdown.Item onClick={onChangeFilter} data-filter="Analysis">
              Draft Analysis
            </NavDropdown.Item>
          </NavDropdown>
          <Navbar.Text className="draft-summary">
            <span>
              {draft !== null && (
                <span>
                  {draft.bestPick && (
                    <span>
                      <i className="far fa-thumbs-up best"></i>
                      {draft.bestPick}
                    </span>
                  )}
                  {draft.worstPick && (
                    <span>
                      <i className="far fa-thumbs-down worst"></i>
                      {draft.worstPick}
                    </span>
                  )}
                </span>
              )}
            </span>
          </Navbar.Text>
        </Nav>
      </Navbar>

      <div>
        {draft === null && <div>No draft found</div>}
        {draft !== null && (
          <div>
            {filter === "Analysis" && <div>Coming soon!</div>}
            {filter !== "Analysis" && (
              <table className="board">
                <thead>
                  <tr>
                    <th></th>
                    {draft.teams.map((team) => {
                      return (
                        <th key={team.id}>
                          <div className="cell-inner team">
                            {team.name}
                            {team.finish > 0 ? " (" + team.finish + ")" : ""}
                          </div>
                        </th>
                      );
                    })}
                  </tr>
                </thead>
                <tbody>
                  {draft.layout !== null &&
                    draft.layout.map((row, rowIndex) => {
                      return (
                        <tr key={rowIndex}>
                          <td className="round">{rowIndex + 1}</td>
                          {row.map((overallPick) => {
                            return (
                              <td key={overallPick}>
                                <div className="cell-inner">
                                  {renderPick(
                                    draft.picks[overallPick - 1],
                                    filter
                                  )}
                                </div>
                              </td>
                            );
                          })}
                        </tr>
                      );
                    })}
                </tbody>
              </table>
            )}
          </div>
        )}
      </div>
    </div>
  );
};

export default withRouter(Draft);

import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Nav, Navbar, NavDropdown } from "react-bootstrap";
import NumberFormat from "react-number-format";
import api from "../../services/api/index";
import qstring from "../../services/qstring/index";
import "./League.scss";
import classnames from "classnames";

const League = ({ history }) => {
  const [leagues, setLeagues] = useState([]);
  const [league, setLeague] = useState(null);
  const [year, setYear] = useState("LAST5");
  const [years, setYears] = useState([]);
  const [managerType, setManagerType] = useState("ACTIVE");
  const [stats, setStats] = useState(null);

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
    setYear(event.target.dataset.year);
  };

  const onChangeManagerType = (event) => {
    setManagerType(event.target.dataset.managertype);
  };
  const getYearText = () => {
    if (year === "LAST5") {
      return "Last 5 Years";
    } else if (year === "LAST10") {
      return "Last 10 Years";
    } else if (year === "ALL") {
      return "All Time";
    } else {
      return year;
    }
  };
  const getManagerText = () => {
    if (managerType === "ACTIVE") {
      return "Active";
    } else {
      return "All";
    }
  };

  useEffect(() => {
    let initLeagueId = qstring.get("leagueid");
    api.getLeagues().then((result) => {
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
    if (league && year && managerType) {
      api
        .getStats(league.id, year, managerType)
        .then((result) => {
          setStats(result.data);
          //console.log(result.data);
        })
        .catch((result) => {
          setStats(null);
        });
    }
  }, [league, year, managerType]);

  return (
    <>
      <div className="cmp-stats-league p-2">
        <Navbar className="toolbar">
          <Nav className="me-auto">
            <Nav.Link onClick={home} className="nav-home">
              <i className="fas fa-home"></i>
            </Nav.Link>
            <NavDropdown
              className="leagues"
              title={league ? league.name : "League"}
            >
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
            <NavDropdown title={getYearText()}>
              <NavDropdown.Item onClick={onChangeYear} data-year="LAST5">
                Last 5 Years
              </NavDropdown.Item>
              <NavDropdown.Item onClick={onChangeYear} data-year="LAST10">
                Last 10 Years
              </NavDropdown.Item>
              <NavDropdown.Item onClick={onChangeYear} data-year="ALL">
                All Time
              </NavDropdown.Item>
              {years.map((y) => {
                return (
                  <NavDropdown.Item
                    onClick={onChangeYear}
                    data-year={y}
                    key={y}
                  >
                    {y}
                  </NavDropdown.Item>
                );
              })}
            </NavDropdown>
            <NavDropdown title={getManagerText()}>
              <NavDropdown.Item
                onClick={onChangeManagerType}
                data-managertype="ACTIVE"
              >
                Managers - Active
              </NavDropdown.Item>
              <NavDropdown.Item
                onClick={onChangeManagerType}
                data-managertype="ALL"
              >
                Managers - All
              </NavDropdown.Item>
            </NavDropdown>
          </Nav>
        </Navbar>
        {stats && (
          <div>
            <div className="card-columns">
              <div className="card card-rings">
                <div className="card-body">
                  <h5 className="card-title">Rings</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Years</th>
                          <th className="col-num">Rings</th>
                          <th className="col-num">Pct</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.rings.map((ring, index) => {
                          return (
                            <tr key={index}>
                              <td>{ring.name}</td>
                              <td className="col-num">{ring.years}</td>
                              <td className="col-num">{ring.rings}</td>
                              <td className="col-num">
                                <NumberFormat
                                  value={ring.ringPercentage}
                                  displayType={"text"}
                                  allowLeadingZeros={false}
                                  decimalScale={2}
                                  fixedDecimalScale={true}
                                />
                              </td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div className="card card-money">
                <div className="card-body">
                  <h5 className="card-title">Money</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Years</th>
                          <th className="col-num">Winnings</th>
                          <th className="col-num">Buy In</th>
                          <th className="col-num">Delta</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.money.map((money, index) => {
                          return (
                            <tr key={index}>
                              <td>{money.name}</td>
                              <td className="col-num">{money.years}</td>
                              <td className="col-num">
                                <NumberFormat
                                  value={money.moneyWon}
                                  displayType={"text"}
                                  thousandSeparator={true}
                                  prefix={"$"}
                                />
                              </td>
                              <td className="col-num">
                                <NumberFormat
                                  value={money.buyIn}
                                  displayType={"text"}
                                  thousandSeparator={true}
                                  prefix={"$"}
                                />
                              </td>
                              <td
                                className={classnames(
                                  "col-num",
                                  "col-num-color",
                                  {
                                    "col-num-positive":
                                      money.moneyWon > money.buyIn,
                                    "col-num-negative":
                                      money.moneyWon < money.buyIn,
                                  }
                                )}
                              >
                                <NumberFormat
                                  value={money.moneyWon - money.buyIn}
                                  displayType={"text"}
                                  thousandSeparator={true}
                                  prefix={"$"}
                                />
                              </td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div className="card card-record">
                <div className="card-body">
                  <h5 className="card-title">Record</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Years</th>
                          <th className="col-num">Wins</th>
                          <th className="col-num">Losses</th>
                          <th className="col-num">Ties</th>
                          <th className="col-num">Finish</th>
                          <th className="col-num">Pct</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.records.map((record, index) => {
                          return (
                            <tr key={index}>
                              <td>{record.name}</td>
                              <td className="col-num">{record.years}</td>
                              <td className="col-num">{record.wins}</td>
                              <td className="col-num">{record.losses}</td>
                              <td className="col-num">{record.ties}</td>
                              <td className="col-num">{record.finish}</td>
                              <td className="col-num">
                                <NumberFormat
                                  value={record.winPercentage}
                                  displayType={"text"}
                                  allowLeadingZeros={false}
                                  decimalScale={3}
                                  fixedDecimalScale={true}
                                />
                              </td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                  {parseInt(year) ? (
                    <div></div>
                  ) : (
                    <div className="card-text">
                      <small className="text-muted">Finish is an average</small>
                    </div>
                  )}
                </div>
              </div>

              <div className="card card-playoffs">
                <div className="card-body">
                  <h5 className="card-title">Playoffs</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Years</th>
                          <th className="col-num">Playoffs</th>
                          <th className="col-num">Pct</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.playoffs.map((playoff, index) => {
                          return (
                            <tr key={index}>
                              <td>{playoff.name}</td>
                              <td className="col-num">{playoff.years}</td>
                              <td className="col-num">{playoff.playoffs}</td>
                              <td className="col-num">
                                <NumberFormat
                                  value={playoff.playoffPercentage}
                                  displayType={"text"}
                                  allowLeadingZeros={false}
                                  decimalScale={3}
                                  fixedDecimalScale={true}
                                />
                              </td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div className="card card-playoffmisses">
                <div className="card-body">
                  <h5 className="card-title">Playoff Misses</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Years</th>
                          <th className="col-num">Misses</th>
                          <th className="col-num">Pct</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.playoffMisses.map((playoff, index) => {
                          return (
                            <tr key={index}>
                              <td>{playoff.name}</td>
                              <td className="col-num">{playoff.years}</td>
                              <td className="col-num">
                                {playoff.playoffMisses}
                              </td>
                              <td className="col-num">
                                <NumberFormat
                                  value={playoff.playoffMissPercentage}
                                  displayType={"text"}
                                  allowLeadingZeros={false}
                                  decimalScale={3}
                                  fixedDecimalScale={true}
                                />
                              </td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div className="card card-doubledigitwins">
                <div className="card-body">
                  <h5 className="card-title">Double Digit Wins</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Year</th>
                          <th className="col-num">Wins</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.doubleDigitWins.map((win, index) => {
                          return (
                            <tr key={index}>
                              <td>{win.name}</td>
                              <td className="col-num">{win.year}</td>
                              <td className="col-num">{win.wins}</td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div className="card card-doubledigitlosses">
                <div className="card-body">
                  <h5 className="card-title">Double Digit Losses</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Year</th>
                          <th className="col-num">Losses</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.doubleDigitLosses.map((loss, index) => {
                          return (
                            <tr key={index}>
                              <td>{loss.name}</td>
                              <td className="col-num">{loss.year}</td>
                              <td className="col-num">{loss.losses}</td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div className="card card-bfls">
                <div className="card-body">
                  <h5 className="card-title">BFLs</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Years</th>
                          <th className="col-num">BFLs</th>
                          <th className="col-num">Pct</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.bfLs.map((bfl, index) => {
                          return (
                            <tr key={index}>
                              <td>{bfl.name}</td>
                              <td className="col-num">{bfl.years}</td>
                              <td className="col-num">{bfl.bfLs}</td>
                              <td className="col-num">
                                <NumberFormat
                                  value={bfl.bflPercentage}
                                  displayType={"text"}
                                  allowLeadingZeros={false}
                                  decimalScale={3}
                                  fixedDecimalScale={true}
                                />
                              </td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                  <div className="card-text">
                    <small className="text-muted">
                      Finished dead last in regular season
                    </small>
                  </div>
                </div>
              </div>

              <div className="card card-pointsfor">
                <div className="card-body">
                  <h5 className="card-title">Points For</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Years</th>
                          <th className="col-num">Points</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.pointsFor.map((points, index) => {
                          return (
                            <tr key={index}>
                              <td>{points.name}</td>
                              <td className="col-num">{points.years}</td>
                              <td className="col-num">
                                <NumberFormat
                                  value={points.points}
                                  displayType={"text"}
                                  thousandSeparator={true}
                                  decimalScale={2}
                                  fixedDecimalScale={true}
                                />
                              </td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div className="card card-pointsagainst">
                <div className="card-body">
                  <h5 className="card-title">Points Against</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Years</th>
                          <th className="col-num">Points</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.pointsAgainst.map((points, index) => {
                          return (
                            <tr key={index}>
                              <td>{points.name}</td>
                              <td className="col-num">{points.years}</td>
                              <td className="col-num">
                                <NumberFormat
                                  value={points.points}
                                  displayType={"text"}
                                  thousandSeparator={true}
                                  decimalScale={2}
                                  fixedDecimalScale={true}
                                />
                              </td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>

              <div className="card card-moves">
                <div className="card-body">
                  <h5 className="card-title">Moves</h5>
                  <div className="card-text">
                    <table className="table table-sm">
                      <thead className="thead-light">
                        <tr>
                          <th>Manager</th>
                          <th className="col-num">Moves</th>
                        </tr>
                      </thead>
                      <tbody>
                        {stats.moves.map((move, index) => {
                          return (
                            <tr key={index}>
                              <td>{move.name}</td>
                              <td className="col-num">{move.moves}</td>
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
        )}
      </div>
    </>
  );
};

export default withRouter(League);

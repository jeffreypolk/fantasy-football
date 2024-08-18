import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Nav, Navbar, NavDropdown } from "react-bootstrap";
import NumberFormat from "react-number-format";
import api from "../../services/api/index";
import "./Players.scss";

const Players = ({ history }) => {
  const [years, setYears] = useState([]);
  const [year, setYear] = useState(0);
  const [players, setPlayers] = useState([]);
  const [position, setPosition] = useState("ALL");

  const home = (event) => {
    event.preventDefault();
    history.push("/");
  };

  const onChangeYear = (event) => {
    setYear(parseInt(event.target.dataset.year));
  };

  const onChangePosition = (event) => {
    setPosition(event.target.dataset.position);
  };

  const getPositionText = () => {
    if (position === "ALL") {
      return "All";
    } else {
      return position;
    }
  };

  useEffect(() => {
    const newYears = [];
    for (let y = new Date().getFullYear(); y >= 2012; y--) {
      newYears.push(y);
    }
    setYears(newYears);
  }, []);

  useEffect(() => {
    if (year > 0) {
      api
        .getPlayers(year)
        .then((result) => {
          setPlayers(result.data);
          //console.log(result.data);
        })
        .catch((result) => {
          setPlayers([]);
        });
    }
  }, [year]);

  return (
    <>
      <div className="cmp-stats-players p-2">
        <Navbar className="toolbar">
          <Nav className="me-auto">
            <Nav.Link onClick={home} className="nav-home">
              <i className="fas fa-home"></i>
            </Nav.Link>
            <NavDropdown title={year === 0 ? "Choose a year" : year}>
              {years.map((y, index) => {
                return (
                  <NavDropdown.Item
                    key={index}
                    onClick={onChangeYear}
                    data-year={y}
                  >
                    {y}
                  </NavDropdown.Item>
                );
              })}
            </NavDropdown>
            <NavDropdown title={getPositionText()}>
              <NavDropdown.Item onClick={onChangePosition} data-position="ALL">
                All
              </NavDropdown.Item>
              <NavDropdown.Item onClick={onChangePosition} data-position="QB">
                QB
              </NavDropdown.Item>
              <NavDropdown.Item onClick={onChangePosition} data-position="RB">
                RB
              </NavDropdown.Item>
              <NavDropdown.Item onClick={onChangePosition} data-position="WR">
                WR
              </NavDropdown.Item>
              <NavDropdown.Item onClick={onChangePosition} data-position="TE">
                TE
              </NavDropdown.Item>
              <NavDropdown.Item onClick={onChangePosition} data-position="DEF">
                DEF
              </NavDropdown.Item>
              <NavDropdown.Item onClick={onChangePosition} data-position="K">
                K
              </NavDropdown.Item>
            </NavDropdown>
            <Navbar.Text className="justify-content-end">
              &nbsp;&nbsp;Download:&nbsp;&nbsp;
              {year > 0 && (
                <span>
                  <a
                    href={api.urlRoot + "/players/" + year + ".csv"}
                    target="_blank"
                    rel="noreferrer"
                  >
                    {year}
                  </a>
                  &nbsp;&nbsp;|&nbsp;&nbsp;
                </span>
              )}
              <a
                href={api.urlRoot + "/players/all.csv"}
                target="_blank"
                rel="noreferrer"
              >
                All Time
              </a>
            </Navbar.Text>
          </Nav>
        </Navbar>
        {players.length > 0 && (
          <table className="table table-sm">
            <thead className="thead-light">
              <tr>
                <th>Name</th>
                <th>Position</th>
                <th>Team</th>
                <th className="col-num">ADP</th>
                <th className="col-num">Projected</th>
                <th className="col-num">Actual</th>
              </tr>
            </thead>
            <tbody>
              {players
                .filter((p) => position === "ALL" || p.position === position)
                .map((player, index) => {
                  return (
                    <tr key={index}>
                      <td>{player.name}</td>
                      <td>{player.position}</td>
                      <td>{player.nflTeam}</td>
                      <td className="col-num">
                        {player.adp === 0 || player.adp === 999
                          ? ""
                          : player.adp}
                      </td>
                      <td className="col-num">
                        <NumberFormat
                          value={player.projectedPoints}
                          displayType={"text"}
                          thousandSeparator={true}
                          decimalScale={2}
                          fixedDecimalScale={true}
                        />
                      </td>
                      <td className="col-num">
                        <NumberFormat
                          value={player.actualPoints}
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
        )}
      </div>
    </>
  );
};

export default withRouter(Players);

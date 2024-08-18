import React, { useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { Nav, Navbar, NavDropdown } from "react-bootstrap";
import NumberFormat from "react-number-format";
import api from "../../services/api/index";
import "./Teams.scss";

const Players = ({ history }) => {
  const [managers, setManagers] = useState([]);
  const [managerId, setManagerId] = useState(null);
  const [manager, setManager] = useState(null);

  const home = (event) => {
    event.preventDefault();
    history.push("/");
  };

  const onChangeManager = (event) => {
    setManagerId(event.target.dataset.managerid);
  };

  useEffect(() => {
    api
      .getManagers()
      .then((result) => {
        setManagers(result.data);
        //console.log(result.data);
      })
      .catch((result) => {
        setManagers([]);
      });
  }, []);

  useEffect(() => {
    if (managerId) {
      api
        .getManagers(managerId)
        .then((result) => {
          setManager(result.data);
          //console.log(result.data);
        })
        .catch((result) => {
          setManager(null);
        });
    }
  }, [managerId]);

  return (
    <>
      <div className="cmp-stats-players p-2">
        <Navbar className="toolbar">
          <Nav className="me-auto">
            <Nav.Link onClick={home} className="nav-home">
              <i className="fas fa-home"></i>
            </Nav.Link>
            <NavDropdown title={manager ? manager.name : "Choose a manager"}>
              {managers.map((manager, index) => {
                return (
                  <NavDropdown.Item
                    onClick={onChangeManager}
                    data-managerid={manager.id}
                  >
                    {manager.name}
                  </NavDropdown.Item>
                );
              })}
            </NavDropdown>
          </Nav>
        </Navbar>

        {manager && (
          <table className="table table-sm">
            <thead className="thead-light">
              <tr>
                <th>Year</th>
                <th>Name</th>
                <th>Playoffs</th>
                <th>BFL</th>
                <th className="col-num">Wins</th>
                <th className="col-num">Losses</th>
                <th className="col-num">Ties</th>
                <th className="col-num">Win Pct</th>
                <th className="col-num">Finish</th>
                <th className="col-num">Points For</th>
                <th className="col-num">Points Against</th>
              </tr>
            </thead>
            <tbody>
              {manager.teams.map((team) => {
                return (
                  <tr key={team.id}>
                    <td>{team.year}</td>
                    <td>{team.name}</td>
                    <td>
                      {team.isBottom
                        ? "No"
                        : team.isConsolation
                        ? "Consolation"
                        : team.isPlayoffs
                        ? "Yes"
                        : ""}
                    </td>
                    <td>{team.isBFL ? "Yes" : "No"}</td>
                    <td className="col-num">{team.wins}</td>
                    <td className="col-num">{team.losses}</td>
                    <td className="col-num">{team.ties}</td>
                    <td className="col-num">
                      <NumberFormat
                        value={
                          team.wins / (team.wins + team.losses + team.ties)
                        }
                        displayType={"text"}
                        allowLeadingZeros={false}
                        decimalScale={2}
                        fixedDecimalScale={true}
                      />
                    </td>
                    <td className="col-num">
                      {team.finish === 0 ? "" : team.finish}
                    </td>
                    <td className="col-num">
                      <NumberFormat
                        value={team.pointsFor}
                        displayType={"text"}
                        thousandSeparator={true}
                        decimalScale={2}
                        fixedDecimalScale={true}
                      />
                    </td>
                    <td className="col-num">
                      <NumberFormat
                        value={team.pointsAgainst}
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

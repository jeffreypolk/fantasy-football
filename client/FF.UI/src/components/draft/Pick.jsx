import React from "react";
import classnames from "classnames";
import NumberFormat from "react-number-format";
import "./Pick.scss";

const Pick = ({ pick }) => {
  const displayName = (pick) => {
    if (pick.player.position === "DEF") {
      return pick.player.name;
    } else {
      const parts = pick.player.name.split(" ");
      return parts[0].substring(0, 1) + ". " + parts[1];
    }
  };
  return (
    <div
      className={classnames("pick", "pos-" + pick.player.position, {
        keeper: pick.isKeeper,
      })}
    >
      <div className="name">{displayName(pick)}</div>
      <div className="info">
        {pick.player.position} - {pick.player.nflTeam}
        <div>
          {pick.overall}
          {pick.player.adp !== 0 && pick.player.adp !== 999 && (
            <span>
              {" "}
              /{" "}
              <NumberFormat
                value={pick.player.adp}
                displayType={"text"}
                decimalScale={0}
                fixedDecimalScale={true}
              />
              &nbsp;=&nbsp;
              <NumberFormat
                value={pick.overall - pick.player.adp}
                displayType={"text"}
                decimalScale={0}
                fixedDecimalScale={true}
              />{" "}
              <i
                className={classnames({
                  "far fa-thumbs-up adp-good":
                    pick.overall - pick.player.adp > 10,
                  "far fa-thumbs-down adp-bad":
                    pick.overall - pick.player.adp < -20,
                })}
              ></i>
            </span>
          )}
        </div>
      </div>
    </div>
  );
};

export default Pick;

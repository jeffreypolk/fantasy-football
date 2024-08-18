import React from "react";
import { withRouter } from "react-router-dom";

const Anchor = ({ href, text, target, history, className, style }) => {
  const targetActual = target ? target : "_self";
  const classNameActual = className ? className : "";
  const styleActual = style ? style : {};

  const click = (event) => {
    if (targetActual === "_self") {
      event.preventDefault();
      history.push(href);
    }
  };

  return (
    <a
      href={href}
      target={targetActual}
      className={classNameActual}
      style={styleActual}
      onClick={click}
      rel="noreferrer"
    >
      {text}
    </a>
  );
};

export default withRouter(Anchor);

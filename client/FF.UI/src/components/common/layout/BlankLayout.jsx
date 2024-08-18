import React from 'react';
import PropTypes from 'prop-types';

const BlankLayout = ({ children }) => {
  
  return (
    <>
      {children}
    </>
  );
};

BlankLayout.propTypes = {
  children: PropTypes.any,
};

export default BlankLayout;

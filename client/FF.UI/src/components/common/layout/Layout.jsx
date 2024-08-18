import React from 'react';
import PropTypes from 'prop-types';
import Header from './Header';

const Layout = ({ children }) => {
  
  return (
    <>
      <div className="page-wrapper">
        <div className="page-inner">
          <div className="page-content-wrapper">
            <Header />
            <main role="main" className="page-content">
              {children}
            </main>
          </div>
        </div>
      </div>
    </>
  );
};

Layout.propTypes = {
  children: PropTypes.any,
};

export default Layout;

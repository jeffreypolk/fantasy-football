import React from 'react';
import { PropTypes } from 'prop-types';
import { Route } from 'react-router-dom';

const AppRoute = ({ component: Component, layout: Layout, isPublic, path, location, onSearchOpen, ...rest }) => {
  return (
    <Route
      {...rest}
      render={(props) => {
        return (
          <Layout>
            <Component {...props} />
          </Layout>
        );
      }}
    />
  );
};

AppRoute.propTypes = {
  component: PropTypes.any,
  layout: PropTypes.any,
  isPublic: PropTypes.bool,
  path: PropTypes.string,
  location: PropTypes.object,
};

export default AppRoute;

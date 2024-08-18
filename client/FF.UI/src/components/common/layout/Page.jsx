import React from 'react';
import { PropTypes } from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as layoutActions from '../../../services/layout/actions';

const mapStatetoProps = (state) => {
  return {
    layout: state.layout,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(layoutActions, dispatch),
  };
};

export const Page = (props) => {
  return <div {...props}>{props.children}</div>;
};

export const PageContent = (props) => {
  return <div {...props}>{props.children}</div>;
};

export const PageHeader = connect(
  mapStatetoProps,
  mapDispatchToProps,
)((props) => {
  return (
    <div className="page-header-2" style={{ fontSize: '1.4rem', padding: '0px 0px 10px 0px' }}>
      {props.title && <span>{props.title}</span>}
      {props.children}
    </div>
  );
});

Page.propTypes = {
  children: PropTypes.any,
};

PageContent.propTypes = {
  children: PropTypes.any,
};

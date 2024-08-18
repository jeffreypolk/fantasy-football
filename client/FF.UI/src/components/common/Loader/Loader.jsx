import React from 'react';
import './Loader.scss';

const Loader = () => {
  return (
    <>
      <div className="loader-wrapper">
        <div className="outer">
          <div className="middle">
            <div className="inner">
              <div className={'spinner-border text-primary'} role="status">
                <span className="sr-only">Loading...</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Loader;

import React from 'react';
import ReactDOM from 'react-dom';
import './styles/index.css';
import App from './components/App';

import { Router } from "react-router";
import { createBrowserHistory } from "history";


const history = createBrowserHistory();

ReactDOM.render(
    <Router history={history}>
      <App />
    </Router>,
  document.getElementById('root')
);
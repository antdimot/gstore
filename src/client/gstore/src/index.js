import React from 'react';
import ReactDOM from 'react-dom';
import './styles/index.css';
import App from './components/App';

import { Router } from "react-router";
import { createBrowserHistory } from "history";


const history = createBrowserHistory();

ReactDOM.render(
  <React.StrictMode>
    <Router history={history}>
      <App />
    </Router>
  </React.StrictMode>,
  document.getElementById('root')
);
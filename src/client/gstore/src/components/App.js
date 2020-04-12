import React, { useState, useEffect } from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";

import '../styles/App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

import { Container,Col,Row } from 'react-bootstrap';
import TokenManager from '../helpers/TokenManager'

import Menu from './Menu'
import Home from './Home';
import Login from './Login';
import UserList from './UserList';

const App = () => {
  const [isLogged,setLogged] = useState(false);

  useEffect( () => {
    setLogged(TokenManager.hasToken);
  },[])

  const loginCallback = () => setLogged(true);
  const logoutCallback = () => setLogged(false);

  return isLogged ? (
    <Router>  
      <Container>
        <Row>
          <Col>
            <Menu logoutCallback={logoutCallback}></Menu>
          </Col>
        </Row>
        <Row className="justify-content-md-center">
          <Switch>
            <Route path="/home">
              <Col>
                <Home />
              </Col>
            </Route>
            <Route path="/userlist">
              <Col md={{ offset: 1, span: 8 }}>
                <UserList />
              </Col>
            </Route>
          </Switch>
        </Row>
      </Container>
    </Router>
  ) : (
    <Router>
      <Container>
        <Row>
          <Col md={{ offset: 2, span: 6 }}>
            <Login loginCallback={loginCallback}/>
          </Col>
        </Row>
      </Container>
      <Switch>
        <Route path="/home"/>
      </Switch>
    </Router>
  )
}

export default App;

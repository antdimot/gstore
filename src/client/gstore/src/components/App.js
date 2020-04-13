import React, { useState, useEffect } from 'react';
import {
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
import GeodataList from './GeodataList';

import { useHistory } from "react-router-dom";

const App = (props) => {
  let history = useHistory();
  const [isLogged,setLogged] = useState(false);

  useEffect( () => {
    setLogged(TokenManager.hasToken);
  },[])

  const loginCallback = () => {
    history.push("/home");
    setLogged(true);
  } 
  const logoutCallback = () => {
    history.push("/");
    setLogged(false);
  } 

  return isLogged ? (
      <Container>
        <Row>
          <Col>
            <Menu logoutCallback={logoutCallback}></Menu>
          </Col>
        </Row>
        <Row >
          <Switch>
            <Route path="/home">
              <Col>
                <Home />
              </Col>
            </Route>
            <Route path="/geodatalist">
              <Col>
                <GeodataList />
              </Col>
            </Route>
          </Switch>
        </Row>
      </Container>
  ) : (
      <Container>
        <Row>
          <Col md={{ offset: 2, span: 5 }}>
            <Login loginCallback={loginCallback}/>
          </Col>
        </Row>
      </Container>
  )
}

export default App;

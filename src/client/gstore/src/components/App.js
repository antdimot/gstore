import React from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";

import '../styles/App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

import { Container,Col,Row } from 'react-bootstrap';

import Menu from './Menu'
import Home from './Home';
import Login from './Login';
import UserList from './UserList';

const App = () => {
  return (
    <Router>
      <Container>
        <Row>
          <Col>
            <Menu></Menu>
          </Col>
        </Row>
        <Row className="justify-content-md-center">
            <Switch>
              <Route path="/home">
                <Col>
                  <Home />
                </Col>
              </Route>
              <Route path="/login">
                <Col md={{ offset: 1, span: 5 }}>
                  <Login />
                </Col>         
              </Route>
              <Route path="/userlist">
                <Col md={{ offset: 1, span: 8 }}>
                  <UserList></UserList>
                </Col>
              </Route>
            </Switch>
        </Row>
      </Container>
    </Router>
  );
}

export default App;

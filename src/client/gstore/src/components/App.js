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
import UserList from './UserList';

const App = () => {

  return (
    <Router>
    <Container fluid>
      <Row>
        <Col>
          <Menu></Menu>
        </Col>
      </Row>
      <Row>
        <Col>
          <Switch>
            <Route exact path="/home">
              <Home />
            </Route>
            <Route path="/userlist">
              <UserList></UserList>
            </Route>
          </Switch>
        </Col>
      </Row>
    </Container>
    </Router>
  );
}

export default App;

import React from 'react';
import {
    BrowserRouter as Router,
  } from "react-router-dom";

import { Navbar,Nav,NavDropdown } from 'react-bootstrap';

const Menu = (props) => {
    return (
        <Router>
            <Navbar bg="light" expand="lg">
                <Navbar.Brand href="#home">GSTore</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="mr-auto">
                    <Nav.Link href="/home">Home</Nav.Link>
                    <Nav.Link href="/login">Login</Nav.Link>
                    <NavDropdown title="Admin" id="basic-nav-dropdown">
                        <NavDropdown.Item href="/userlist">User List</NavDropdown.Item>
                        {/* <NavDropdown.Divider />
                        <NavDropdown.Item href="#action/3.4">Separated link</NavDropdown.Item> */}
                    </NavDropdown>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        </Router>
    );
}


export default Menu;
import React from 'react';

import { Navbar,Nav, Button } from 'react-bootstrap';
import TokenManager from '../helpers/TokenManager';


const Menu = (props) => {
    const logoutHandler = (event) => {
        TokenManager.clearToken();
        props.logoutCallback();
    } 

    return (
        <Navbar bg="light" expand="lg">
            <Navbar.Brand href="#home">GSTore</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="mr-auto">
                <Nav.Link href="/home">Home</Nav.Link>
                <Nav.Link href="/geodatalist">Geodata</Nav.Link>
                <Button variant="link" size="sm"  onClick={logoutHandler}>
                    Logout
                </Button>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
}


export default Menu;
import React, {useState,useEffect} from 'react';

import { Navbar,Nav, Form } from 'react-bootstrap';
import TokenManager from '../helpers/TokenManager';
import DataManager from '../helpers/DataManager'

const Menu = (props) => {
    const [user,setUser] = useState('pippo');

    const logoutHandler = (event) => {
        TokenManager.clearToken();
        props.logoutCallback();
    }

    useEffect(()=> {
        DataManager().get('/user/info')
        .then(function (response) {
            //console.log(response.data);
            setUser(response.data.firstname + ' ' + response.data.lastname);
        })
        .catch(function (error) {
            console.log(error);
        }); 
    },[])

    return (
        <Navbar bg="light" expand="lg">
            <Navbar.Brand href="#">GSTore</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="mr-auto">
                    <Nav.Link href="/home">Home</Nav.Link>
                    <Nav.Link href="/geodatalist">Geodata</Nav.Link>
                    <Nav.Link onClick={logoutHandler}>
                        Logout
                    </Nav.Link>
                </Nav>
                <Form inline>
                    <Form.Label className="mr-sm-5">{user}</Form.Label>
                </Form>
            </Navbar.Collapse>
        </Navbar>
    );
}


export default Menu;
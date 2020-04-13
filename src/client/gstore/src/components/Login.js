import React, {useState} from 'react';
import { Form,Button } from 'react-bootstrap';
import DataManager from '../helpers/DataManager';
import TokenManager from '../helpers/TokenManager';

const Login = (props) => {
    const [validated, setValidated] = useState(false);
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const validateForm = () => {
        return username.length > 0 && password.length > 0;
    }

    const submitHandler = (event) => {
        const form = event.currentTarget;
        if (form.checkValidity() === true) { 
            var formData = new FormData();
            formData.set('username',username);
            formData.set('password',password);
            
            DataManager().post('/user/authenticate',formData)
                        .then(function (response) {
                            TokenManager.setToken( response.data.access_token );
                            props.loginCallback();
                        })
                        .catch(function (response) {
                            TokenManager.clearToken();
                            console.log(response);
                        });
        }

        event.preventDefault();
        event.stopPropagation();

        setValidated(true);
    }

    return (
        <>
            <h1>GSTore management</h1>
            <br/>
            <h3>Authentication</h3>

            <Form noValidate validated={validated} onSubmit={submitHandler}>
                <Form.Group controlId="formBasicEmail">
                    <Form.Label>Username</Form.Label>
                    <Form.Control name="username" type="text" placeholder="username" required
                        value={username}
                        onChange={e => setUsername(e.target.value)} />
                </Form.Group>

                <Form.Group controlId="formBasicPassword">
                    <Form.Label>Password</Form.Label>
                    <Form.Control name="password" type="password" placeholder="Password" required
                        value={password}
                        onChange={e => setPassword(e.target.value)} />
                </Form.Group>
                <Button variant="primary" disabled={!validateForm()} type="submit">
                    Login
                </Button>
            </Form>
        </>
    )
};

export default Login;
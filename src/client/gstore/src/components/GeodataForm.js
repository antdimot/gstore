import React, { useState  }  from 'react';
import DataManager from '../helpers/DataManager';
import { Col, Form, Button } from 'react-bootstrap';

const GeodataForm = (props) => {
    const [name, setName] = useState('');
    const [latitude, setLatitude] = useState('');
    const [longitude, setLongitude] = useState('');
    const [tag, setTag] = useState('');
    const [content, setContent] = useState('');

    const submitHandler = (event) => {
        var formData = new FormData(event.target);

        console.log(event.target);

        DataManager().post('/geodata',formData)
            .then(function (response) {                 
                console.log(response);
            })
            .catch(function (error) {
                console.log(error);
            });

        event.preventDefault();
        event.stopPropagation();
    }

    return (
        <>
            <br/>
            <h4>Geodata Form</h4>
            <hr/>
            <Form onSubmit={submitHandler}>
                <Form.Row>
                    <Form.Group as={Col} controlId="formGridName">
                        <Form.Label>Name</Form.Label>
                        <Form.Control name="name" required placeholder="location name"
                            value={name} onChange={e => setName(e.target.value)} />
                    </Form.Group>

                    <Form.Group as={Col} controlId="formGridTag">
                        <Form.Label>Tag</Form.Label>
                        <Form.Control name="tag" placeholder="tag"
                            value={tag} onChange={e => setTag(e.target.value)} />
                    </Form.Group>
                </Form.Row>

                <Form.Row>
                    <Form.Group as={Col} controlId="formGridLatitude">
                        <Form.Label>Latitude</Form.Label>
                        <Form.Control name="lat" required type="text" placeholder="enter latitude position"
                            value={latitude} onChange={e => setLatitude(e.target.value)} />
                        </Form.Group>

                        <Form.Group as={Col} controlId="formGridLongitude">
                        <Form.Label>Longitude</Form.Label>
                        <Form.Control name="lon" required type="text" placeholder="enter longitude position"
                            value={longitude} onChange={e => setLongitude(e.target.value)} />
                    </Form.Group>
                </Form.Row>

                <Form.Group controlId="formGridContent">
                    <Form.Label>Content</Form.Label>
                    <Form.Control name="content" required placeholder="information content" value={content} onChange={e => setContent(e.target.value)} />
                </Form.Group>

                <Button variant="primary" type="submit">
                    Submit
                </Button>
            </Form>
        </>
    )
}

export default GeodataForm;
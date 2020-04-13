import React, { useState, useEffect }  from 'react';
import { Table, Button, Modal } from 'react-bootstrap';
import DataManager from '../helpers/DataManager';

const GeodataList = (props) => {
    const [selectedItem, setSelectedItem] = useState(null);
    const [show, setShow] = useState(false);
    const [geodata, setGeodata] = useState([]);

    const handleClose = () => setShow(false);
    const handleShow = (id) => {
        setSelectedItem(id);
        setShow(true);
    } 

    const loadData = () => {
        DataManager().get('/geodata/list')
            .then(function (response) {                 
                setGeodata(response.data);
            })
            .catch(function (error) {
                console.log(error);
            }); 
    }

    useEffect( ()=> loadData(), []);

    const deleteHandler = (id) => {
        console.log(id);
     
        var formData = new FormData();
        formData.set('id',id);

        DataManager().post('/geodata/delete',formData)
            .then(function (response) {
                setSelectedItem(null);
                setShow(false);               
                loadData();
            })
            .catch(function (error) {
                console.log(error);
            });   
    }

    return (
        <>
            <br/>
            <h4>Geodata List</h4>
            <hr/>
            <a href="/geodataform">add</a>
            <Table striped bordered size="sm">
                <thead>
                    <tr>
                        <th></th>
                        <th>Name</th>
                        <th>Position (lat,lon)</th>
                        <th>Content</th>
                    </tr>
                </thead>
                <tbody>
                { geodata.map( (g) => (
                    <tr key={g.id}>
                        <td>
                            <Button variant="outline-danger" size="sm" onClick={(e) => handleShow(g.id)}>
                                delete
                            </Button>
                        </td>
                        <td>{g.name}</td>
                        <td>({g.lat},{g.lon})</td>
                        <td>{g.content}</td>
                    </tr>
                )) }              
                </tbody>
            </Table>

            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                <Modal.Title>Confirm</Modal.Title>
                </Modal.Header>
                <Modal.Body>Are you sure to delete this item?</Modal.Body>
                <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    No
                </Button>
                <Button variant="primary" onClick={(e) => deleteHandler(selectedItem)}>
                    Yes
                </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default GeodataList;
import React, { useState, useEffect }  from 'react';
import { Table } from 'react-bootstrap';
import DataManager from '../helpers/DataManager';

const GeodataList = (props) => {
    const [geodata, setGeodata] = useState([]);

    useEffect( () => {
        DataManager().get('/geodata/list')
            .then(function (response) {                 
                setGeodata(response.data);
            })
            .catch(function (error) {
                console.log(error);
            }); 
        
      }, []);

    return (
        <>
            <br/>
            <h4>Geodata List</h4>
            <hr/>
            <a href="/geodataform">add</a>
            <Table striped bordered size="sm">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Position (lat,lon)</th>
                        <th>Content</th>
                    </tr>
                </thead>
                <tbody>
                { geodata.map( (g) => (
                    <tr key={g.id}>
                        <td>{g.name}</td>
                        <td>({g.lat} {g.lon})</td>
                        <td>{g.content}</td>
                    </tr>
                )) }              
                </tbody>
            </Table>
        </>
    );
}

export default GeodataList;
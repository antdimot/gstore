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
        <Table striped bordered>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Latitude</th>
                    <th>Longitude</th>
                </tr>
            </thead>
            <tbody>
            { geodata.map( (g) => (
                <tr key={g.name}>
                    <td>{g.name}</td>
                    <td>{g.lat}</td>
                    <td>{g.lon}</td>
                </tr>
            )) }              
            </tbody>
        </Table>
    );
}

export default GeodataList;
import React, { useState, useEffect }  from 'react';

import { Table } from 'react-bootstrap';

import DataManager from '../helpers/DataManager';

const UserList = (props) => {

    const [users, setUsers] = useState([]);

    useEffect( () => {
        DataManager().get('/user/list')
                    .then(function (response) {                 
                        setUsers(response.data);
                    })
                    .catch(function (error) {
                        console.log(error);
                    }); 
      }, []);

    const userItems = users.map((u) =>
        <tr key={u.username}>
            <td>{u.firstname}</td>
            <td>{u.lastname}</td>
            <td>{u.username}</td>
            <td>{u.enabled ? "enabled" : "disabled"}</td>
        </tr>
    );

    return (
        <Table striped bordered hover>
            <thead>
                <tr>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Username</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                {userItems}              
            </tbody>
        </Table>
    );
}


export default UserList;
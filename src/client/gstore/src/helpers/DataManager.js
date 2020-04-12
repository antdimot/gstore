import axios from 'axios';

const DataManager = () => {

    let access_token = localStorage.getItem('gstore_token')

    return axios.create({
        baseURL: 'http://localhost:5000/api/v1',
        timeout: 10000,
        headers: {
            Authorization: "Bearer " + access_token
        }
    });
}

export default DataManager;
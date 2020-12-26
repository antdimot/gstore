import axios from 'axios';
import TokenManager from './TokenManager';

const DataManager = () => {

    let access_token = TokenManager.getToken();

    return axios.create({
        baseURL: process.env.REACT_APP_GSTORE_API_BASE_URL, //'http://localhost:5000/api/v1',
        timeout: 10000,
        headers: {
            Authorization: "Bearer " + access_token
        }
    });
}

export default DataManager;
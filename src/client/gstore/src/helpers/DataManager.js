import axios from 'axios';

const DataManager = () => {

    return axios.create({
        baseURL: 'http://localhost:5000/api/v1',
        timeout: 1000,
        headers: {
            Authorization: "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFkbWluIiwiVXNlcklkIjoiNWU5MmRiYTBhZTAxN2E1MDZiMjY2YTAxIiwiVXNlckF1dGh6IjoiYWRtaW4scmVhZGVyLHdyaXRlciIsIm5iZiI6MTU4NjY4Nzc2MCwiZXhwIjoxNTg2NjkwMTYwLCJpYXQiOjE1ODY2ODc3NjAsImlzcyI6IkdTdG9yZUlzc3VlciIsImF1ZCI6IkdTdG9yZUF1ZGllbmNlIn0.qcQL_xj0oJI2iTbhV9u60P8GHzvwK5QuvVwcIhIaxaA"
         }
    });
   
    // return {
    //     getUsers: async () => {
    //         await instance.get('/user/list')
    //             .then(function (response) {
    //                 console.log(response.data);
    //                 return response.data;
    //             })
    //             .catch(function (error) {
    //                 console.log(error);
    //             })
    //             .then(function () {
    //             });
    //     }
    // }
}

export default DataManager;
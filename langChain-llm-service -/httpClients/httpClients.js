import axios from 'axios';
// Disable SSL certificate verification (NOT RECOMMENDED for production )
process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

const  httpClient =  axios.create({
    baseURL:"https://localhost:44311/api/services/app",
    headers: {
        'Content-Type': 'application/json'
    }
});

export {httpClient};
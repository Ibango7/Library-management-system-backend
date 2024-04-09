import { httpClient } from '../httpClients/httpClients.js';

export const getBooks = ( ) => new Promise((resolve, reject) => {
    httpClient.get(`/Book/GetAll?MaxResultCount=150`)
    .then((response) =>{
        resolve(response.data)
    })
    .catch(error => {
        reject(error)
        console.log("Error getting all books");
    })
})

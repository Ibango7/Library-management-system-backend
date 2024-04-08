import express from 'express';
import cors from 'cors';
import userRoute from './routes/appRoute.js';
const app = express();
const port = 3002;

// Enable CORS for all routes
app.use(cors())

app.get('/',  (req, res) =>{
    res.send('Welcome to NodeJS API')
});

// define route
app.use('/llm', userRoute);


// start the server 
app.listen(port, () =>{
    console.log(`Server is listening on port ${port}`);
});
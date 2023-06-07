const express = require('express');
const fs = require('fs');

const calculateProbabilityFirst = require('./controllers/first.js');
const calculateProbabilitySecond = require('./controllers/second.js');
const calculateProbabilityThird = require('./controllers/third.js');
const calculateProbabilityFourth = require('./controllers/fourth.js');

const hbs = require('express-handlebars').create({
    extname: 'hbs',
    defaultLayout: 'index',
    layoutsDir: __dirname + '/views/layouts/',
    partialsDir: __dirname + '/views/partials/',
});

const app = express();
const port = 3000;

app.engine('hbs', hbs.engine);
app.set('view engine', 'hbs');
app.set("views", "./views");

app.use(express.static('public'));

app.get('/first', (req, res) => {
    calculateProbabilityFirst.calculateProbabilityFirst(req, res);
});

app.get('/second', (req, res) => {
    calculateProbabilitySecond.calculateProbabilitySecond(req, res);
});

app.get('/third', (req, res) => {
    calculateProbabilityThird.calculateProbabilityThird(req, res);
});

app.get('/fourth', (req, res) => {
    calculateProbabilityFourth.calculateProbabilityFourth(req, res);
});



app.listen(port, () => console.log(`App listening to http://localhost:${port}/first`));

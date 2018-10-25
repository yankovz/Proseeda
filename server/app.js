var 
    url  = require('url'),
    sys  = require('sys'),
    express = require('express'),
    http=require('http');

//var app = express();

var app = express();
var server = http.createServer(app);

//var express = require('express');
var router = express.Router();

//var clientsData = require('meeting.json');
var Datastore = require('nedb');
//var db = new Datastore();
//db.insert(clientsData);
var db = new Datastore({ filename: 'meeting.json', autoload: true });
//var db = new Datastore();

//db.insert({
//	   "Name": "PBC Limited",
//	    "Hour": 2.23,
//	    "Description": "meeting",
//	    "Age": 61,
//	    "Country": 6,
//	    "Address": "Ap #897-1459 Quam Avenue",
//	    "Married": false
//	    });

var getClientFilter = function(query) {
    var result = {
        Name: new RegExp(query.Name, "i"),
        Address: new RegExp(query.Address, "i")
    };

    if(query.Married) {
        result.Married = query.Married === 'true' ? true : false;
    }

    if(query.Country && query.Country !== '0') {
        result.Country = parseInt(query.Country, 10);
    }

    return result;
};

var prepareItem = function(source) {
    var result = source;
    result.Married = 'true';
    result.Country = 6;
    result.Address = 'ps';
    return result;
};

console.log("ziv");

//app.engine('.html', require('ejs').__express);
//app.set('views', __dirname + '/views');
//app.set('view engine', 'html');

app.get('/', function(req, res){
    res.render('index');
});

app.get('/jq', function(req, res){
    res.render('jqwidget.html');
});

app.get('/jq2', function(req, res){
    res.render('jqwidget2.html');
});



app.get('/main', function(req, res){
    res.render('DataTable.html');
});

//MongoClient.connect(url, function(err, db) {
//	  if (err) throw err;
//	  var dbo = db.db("proseeda");
//	  //var query = { address: "Park Lane 38" };
//	  dbo.collection("customers").find().toArray(function(err, result) {
//	    if (err) throw err;
//	    console.log(result);
//	    res.json(result);
//	    db.close();
//	  });
//	});
	
app.get('/loaddata', function(req, res){
	var car = {id:1,Name:'PWC',Case:'533',Hour:'0.4',Description:'Editing',Source:'Document',Confirmed:true};
	var cars = [];
	cars.push(car);
	sys.puts('going to find');
	sys.puts(cars);
	res.setHeader('Content-Type', 'application/json;charset=utf-8');
//	res.set({ 'content-type': 'application/json;charset=utf-8' });
    res.json(cars);
	
	//var data = db.loadDatabase();
	//res.json(data);
});

//var path = require('path')
//app.use(express.static(path.join(__dirname, 'public')));

server.listen(3000);
sys.puts('server running ' + 'now ' + Date.now());
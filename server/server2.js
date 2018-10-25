var someRandomPort = 8099,
jot = require('json-over-tcp');
 
var Datastore = require('nedb');
//var db = new Datastore();
//db.insert(clientsData);
//var db = new Datastore({ filename: '../db/clients.json', autoload: true });
var db = new Datastore({ filename: '../db/meeting.json', autoload: true });
//db.insert({
//	   "Name": "PBC Limited",
//	    "Hour": 2.23,
//	    "Description": "meeting",
//	    "Age": 61,
//	    "Country": 6,
//	    "Address": "Ap #897-1459 Quam Avenue",
//	    "Married": false
//	    
var server = jot.createServer();
//server.on('listening', createConnection);
server.on('connection', newConnectionHandler);
server.on('error', function(ex) {
	  console.log("handled error");
	  console.log(ex);
	}); 
// Triggered whenever something connects to the server
function newConnectionHandler(socket){
  // Whenever a connection sends us an object...
	console.log("got new connection");
  socket.on('data', function(data){
	  console.log("got data");
    // Output the question property of the client's message to the console
    console.log("Client's question: " + data);
    db.insert(data)},function(err){
	  console.log("got error");
	    // Output the question property of the client's message to the console
	    console.log("Error: " + err);});
};
 
// Creates one connection to the server when the server starts listening

// Start listening
server.listen(someRandomPort);
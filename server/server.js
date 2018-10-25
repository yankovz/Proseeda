var someRandomPort = 8099,
net = require('net');
var MongoClient = require('mongodb').MongoClient;
var url = "mongodb://localhost:27017/";

//  var myobj =  {"id":"1","Name":"PWC","Case":"533","Hour":"0.4","Description":"Editing Document NamedContract 96573.docx","Source":"Document Edit","Confirmed":"true"};
  var dbo;
MongoClient.connect(url, function(err, db) {
	  if (err) throw err;
	  dbo = db.db("proseeda");
	});
//var db = new Datastore();
//db.insert(clientsData);
//var db = new Datastore({ filename: '../db/clients.json', autoload: true });
//var db = new Datastore({ filename: '../db/meeting.json', autoload: true });
var server = net.createServer(function (socket) {
	  // Send a nice welcome message and announce
	  // Handle incoming messages from clients.
	  socket.on('data', function (data) {
	    console.log("got data: " + data);
	    try{
		    //@todo handle parsing error
		    var obj = JSON.parse(data);
		    var today = new Date();
			var dd = today.getDate();
			var mm = today.getMonth()+1; //January is 0!
			var yyyy = today.getFullYear();
			
			if(dd<10) {
			    dd = '0'+dd
			} 
			
			if(mm<10) {
			    mm = '0'+mm
			} 
			
			today = mm + '/' + dd + '/' + yyyy;
		    obj.Confirmed=false;
		    obj.date=today;
		    console.log("got user: " + obj.user);
		    var query = { userName: "Ziv Yankowitz" };
		    var userId; 
		    console.log("query: " + query);
		    dbo.collection("users").findOne(query, function(err, result) {
		        if (err) throw err;
		        
		        obj.userId = result.userId;
		        console.log("going to insert: " + result.userId);
		        dbo.collection("activties").insertOne(obj, function(err, res) {
			        if (err) throw err;
			        console.log("1 document inserted");
			        //db.close();
			      });
			    
			    console.log("converted data: " + obj);
		        
		      });
		    
	    }catch(ex){
	    	console.log(ex);
	    }   
		    //obj.userId=result.userId;
	    
	  });});
// Creates one connection to the server when the server starts listening

// Start listening
server.listen(someRandomPort);
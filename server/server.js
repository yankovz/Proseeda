var someRandomPort = 8099,
net = require('net');
var mongodb = require('mongodb');
var MongoClient = mongodb.MongoClient;
var url = "mongodb://localhost:27017/";
var msgRequestInsert="insert";
var msgRequestQuery="query";
var fs = require('fs');
var util = require('util');
var log_file = fs.createWriteStream(__dirname + '/debug.log', {flags : 'w'});
var log_stdout = process.stdout;

console.log = function(d) { //
  log_file.write(util.format(d) + '\n');
  log_stdout.write(util.format(d) + '\n');
};
try{
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
		    console.log("**************New Message got data: " + data);
		    try{
		    	
				 //@todo handle parsing error
				 var obj = JSON.parse(data);
				 if(obj.msgRequestInsert!=null && obj.msgRequestInsert===msgRequestInsert)
				 {
					 processInsert(obj);
				 }
				 if(obj.msgRequestInsert!=null && obj.msgRequestInsert===msgRequestQuery)
				 {
					 processQueryCustomer(obj,socket);
				 }
				    
		    }catch(ex){
		    	console.log(ex);
		    }   
			    //obj.userId=result.userId;
		    
		  });});
	// Creates one connection to the server when the server starts listening
	
	// Start listening
	server.listen(someRandomPort);
}catch(error){
	console.log("got error: " + error);
}

function updateStats(obj){
	console.log("1 document inserted");
    //update avrage month data
	var today = new Date(obj.date);
	var dd = today.getDate();
	var mm = today.getMonth()+1; //January is 0!
	var yyyy = today.getFullYear();
	
	
	
	
	
	today = mm + '/01' + '/' + yyyy;
    var query = { userId: obj.userId, date: today };
    
    console.log("query avgactivitiy: userid " + obj.userId + ", date : " + today);
    dbo.collection("avgactivitiy").findOne(query, function(err, result) {
        if (!err && result!=null)
        {//updating the total hour worked on month for this user
        	//first let's delete the old recod and than insert
        	console.log("found avrage activity");
	        var duration = parseInt(result.Duration, 10);
	        var currentDuration = parseInt(obj.Duration, 10);
	        obj.Duration = duration + currentDuration;
	        obj.date = today;
        	dbo.collection("avgactivitiy").remove(query, function(err, result) {
		        
	        	dbo.collection("avgactivitiy").insertOne(obj, function(err, res) {
			        if (err) throw err;		
			        console.log("inserted new avrage activity");
			        //db.close();
			      });
        	});
        }
        else{
        	obj.date = today;
        	dbo.collection("avgactivitiy").insertOne(obj, function(err, res) {
		        if (err) throw err;		
		        console.log("inserted new avrage activity");
		        //db.close();
		      });
        }
    });
}

function processQueryCustomer(obj,socket){
	console.log("processQueryCustomer starting");
	dbo.collection('customers').aggregate([
		{ $lookup:
	       {
	         from: 'cases',
	         localField: '_id',
	         foreignField: 'customerid',
	         as: 'cases'
	       }
	     }
	    ]).toArray(function(err, res){
	    	console.log("processQueryCustomer starting2 : " + JSON.stringify(res));
	    	var cust = res[0].cases;
	    	console.log("processQueryCustomer starting3 : " + JSON.stringify(cust));
    		if(!err && res.length>0)
    		{
    			socket.write(JSON.stringify(res));
    			//socket.flush();
    		}
	    });	
}

function processInsert(obj){
	
    var today = new Date(obj.date);
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
    var query = { userName: obj.user };
    var userId; 
    console.log("query: " + query.toString());
    dbo.collection("users").findOne(query, function(err, result) {
        if (!err && result!=null)
        {
	        console.log("found1 user going to check source phone");
	        obj.userId = result.userId;
	        console.log(obj.Source);
	        if(obj.Source==='phone call')
	        {
	        	console.log("found user going to check source phone : " +obj.Name );
	        	if(obj.Name!=null){
	        		query = {phone: obj.Name};
	        		dbo.collection('customersdetails').aggregate([
	        			{ "$match": { "phone": obj.Name } },
	        			{ $lookup:
	        		       {
	        		         from: 'customers',
	        		         localField: 'customerid',
	        		         foreignField: '_id',
	        		         as: 'customer'
	        		       }
	        		     }
	        		    ]).toArray(function(err, res){
		        		if(!err && res.length>0)
		        		{
		        			var result = res[0];//assume one phone per client
		        			
					        console.log("going to insert1: " + JSON.stringify(result));
					        var cust = result.customer[0];
					        console.log("going to insert2: " + JSON.stringify(cust));
					        obj.Name=cust.name;
					        //find the default case in case of phone call
					        var o_id =  new mongodb.ObjectID(cust._id);
					        query = {customerid: o_id
					        ,default:true};
					        dbo.collection('cases').findOne(query, function(err, result) {
					        	if(!err && result!=null && result.name!=null)
					        	{
							        obj.Case=result.name;
							        console.log("going to insert client case: " + obj.Case);
							        dbo.collection("activities").insertOne(obj, function(err, res) {
								        if (err) throw err;		
								        updateStats(obj);//update statistics
								        //db.close();
								      });
								    
								    console.log("converted data: " + obj);
					        	}
					        });
		        		}
		        	});
	        	}
	        }
	        else{
	        	query = {name: obj.Name};
		        dbo.collection('customers').findOne(query, function(err, result) {
		        	if(!err && result!=null && result.name!=null)
		        	{
			        	console.log("going to insert: " + result.userId);
				        dbo.collection("activities").insertOne(obj, function(err, res) {
					        if (err) throw err;
					        console.log("1 document inserted");
					        updateStats(obj);//update statistics
					        //db.close();
					      });
		        	}});
			    
			    console.log("converted data: " + obj);
	        }
        }
        else{
        	console.log("couldnt find user with Name: " + obj.user)
        }
      });

}
var someRandomPort = 8099,
jot = require('json-over-tcp');
 
var socket = jot.connect(someRandomPort, function(){
    // Send the initial message once connected
    socket.write({
	   "Name": "KFC LTD",
	    "Hour": 2,
	    "Description": "meeting",
	    "Age": 61,
	    "Country": 6,
	    "Address": "Ap #897-1459 Quam Avenue",
	    "Married": false
	    });
  });
  
  // Whenever the server sends us an object...
 socket.on('data', function(data){
	// Output the answer property of the server's message to the console
	console.log("Server's answer: " + data.answer);
	
	// Wait one second, then write a question to the socket
	
 	});

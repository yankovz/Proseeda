var MongoClient = require('mongodb').MongoClient;
var url = "mongodb://localhost:27017/";

MongoClient.connect(url, function(err, db) {
  if (err) throw err;
  var dbo = db.db("proseeda");
  var myobj =  {"id":"1","Name":"PWC","Case":"533","Hour":"0.4","Description":"Editing Document NamedContract 96573.docx","Source":"Document Edit","Confirmed":"true"};
  dbo.collection("customers").insertOne(myobj, function(err, res) {
    if (err) throw err;
    console.log("1 document inserted");
    db.close();
  });
});
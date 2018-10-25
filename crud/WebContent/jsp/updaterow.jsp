<%@ page language="java" contentType="text/html; charset=windows-1255"
    pageEncoding="windows-1255"%>
<%@ page import="java.sql.*"%>
<%@ page import="com.google.gson.*"%>
<%@ page import="com.mongodb.*"%>
<%@ page import="org.bson.types.ObjectId"%>
<%
	String rowToUpdate = request.getParameter("id");
	String Name = request.getParameter("Name");
	String LastName = request.getParameter("Case");
	String Hour = request.getParameter("Hour");
	String Description = request.getParameter("Description");
	String Source = request.getParameter("Source");
	String Confirmed = request.getParameter("Confirmed");
	
	// (A) database connection
	// "jdbc:mysql://localhost:3306/northwind" - the database url of the form jdbc:subprotocol:subname
	// "dbusername" - the database user on whose behalf the connection is being made
	// "dbpassword" - the user's password
	
	// (C) format returned ResultSet as a JSON array
	System.out.println("i was called");
	MongoClient mongoClient = new MongoClient(new MongoClientURI("mongodb://localhost:27017"));
	
	DB database = mongoClient.getDB("proseeda");
	System.out.println("i was called2");
	DBCollection collection = database.getCollection("activties");
	System.out.println("i was called3");
	System.out.println("id to update: " + rowToUpdate);

    BasicDBObject query = new BasicDBObject();
    query.put("_id", new ObjectId(rowToUpdate));

    DBObject cursor = collection.findOne(query);
   
	//DBCursor cursor = collection.find();
	
	System.out.println("i was called4");
	JsonArray recordsArray = new JsonArray();
	if(cursor!=null){
		
		
		System.out.println("i was called");
		System.out.println("found" + cursor.toString());
		
		//while (employees.next()) {
			
			BasicDBObject  currentRecord = new BasicDBObject();
			
			currentRecord.put("Name",Name);
			currentRecord.put("Hour",Hour);
			currentRecord.put("Source",Source);
			currentRecord.put("Description",Description);
			currentRecord.put("Confirmed",Confirmed);
			BasicDBObject updateObj = new BasicDBObject();

            updateObj.put("$set", currentRecord);
            collection.update(query, updateObj, false, true);

		}		
	// (D)
	
	
%>

<%@ page language="java" contentType="text/html; charset=windows-1255"
    pageEncoding="windows-1255"%>
<%@ page import="java.sql.*"%>
<%@ page import="com.google.gson.*"%>
<%@ page import="com.mongodb.*"%>
<%
	// (A) database connection
	// "jdbc:mysql://localhost:3306/northwind" - the database url of the form jdbc:subprotocol:subname
	// "dbusername" - the database user on whose behalf the connection is being made
	// "dbpassword" - the user's password
	
	// (C) format returned ResultSet as a JSON array
	System.out.println("i was called");
	MongoClient mongoClient = new MongoClient(new MongoClientURI("mongodb://localhost:27017"));
	
	DB database = mongoClient.getDB("proseeda");
	System.out.println("i was called2");
	DBCollection collection = database.getCollection("activities");
	System.out.println("i was called3");
	String date1 = request.getParameter("date");
	String userId = request.getParameter("userId");          
    //cursorObj = collectionObj.find(selectQuery);
    //DBCursor cursor = collection.find(selectQuery);
    System.out.println("got date: " + date1 );
    DBCursor cursor;
    if(date1!=null)
    {
    	
    	BasicDBObject query = new BasicDBObject();
        query.put("date", date1);
        query.put("userId",userId);
    	cursor = collection.find(query);
    	
    }
    else
    {
    	cursor = collection.find();
    }
	
	
	System.out.println("i was called4");
	JsonArray recordsArray = new JsonArray();
	while(cursor.hasNext()){
		
		DBObject jo = (DBObject)cursor.next();
		System.out.println("i was called");
		System.out.println("found" + jo.toString());
		
		//while (employees.next()) {
			
			JsonObject currentRecord = new JsonObject();
			currentRecord.add("id",
					new JsonPrimitive(((String)jo.get("_id").toString())));
			currentRecord.add("Name",
					new JsonPrimitive(((String)jo.get("Name"))));
			currentRecord.add("Case",
					new JsonPrimitive(((String)jo.get("Case"))));
			currentRecord.add("Date",
					new JsonPrimitive(((String)jo.get("date"))));
			currentRecord.add("Time",
					new JsonPrimitive(((String)jo.get("time"))));
			currentRecord.add("Duration",
					new JsonPrimitive(((String)jo.get("Duration"))));
			currentRecord.add("Source",
					new JsonPrimitive(((String)jo.get("Source"))));
			currentRecord.add("Description",
					new JsonPrimitive(((String)jo.get("Description"))));
			currentRecord.add("Confirmed",
					new JsonPrimitive(((String)jo.get("Confirmed").toString())));
			recordsArray.add(currentRecord);
		}		
	// (D)
	System.out.println(recordsArray.toString());
	out.print(recordsArray);
	out.flush();
%>

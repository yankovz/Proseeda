<%@ page language="java" contentType="text/html; charset=windows-1255"
    pageEncoding="windows-1255"%>
<%@ page import="java.sql.*"%>
<%@ page import="com.google.gson.*"%>
<%@ page import="com.mongodb.*"%>
<%@ page import="java.util.*"%>
<%@ page import="java.text.*"%>
<%
System.out.println("i was called");
MongoClient mongoClient = new MongoClient(new MongoClientURI("mongodb://localhost:27017"));

DB database = mongoClient.getDB("proseeda");
System.out.println("i was called2");
DBCollection collection = database.getCollection("activities");
System.out.println("i was called3");

String userId = request.getParameter("userId");
//String userId = "ziv@proseeda.com";
//cursorObj = collectionObj.find(selectQuery);
//DBCursor cursor = collection.find(selectQuery);

DBCursor cursor;
if(userId!=null)
{
	
	BasicDBObject query = new BasicDBObject();
    
    query.put("userId",userId);
	cursor = collection.find(query);
	
}
else
{
	cursor = collection.find();
}


System.out.println("i was called4");
JsonArray recordsArray = new JsonArray();
HashMap map = new HashMap();
while(cursor.hasNext()){
	
	DBObject jo = (DBObject)cursor.next();
	System.out.println("i was called");
	System.out.println("found" + jo.toString());
	if(map.containsKey(jo.get("Name")))
	{
		double hours = ((Double)map.get(jo.get("Name"))).doubleValue();
		hours += Double.parseDouble((String)jo.get("Duration"));
		map.put(jo.get("Name"), new Double(hours));
		System.out.println("********* Name: " + jo.get("Name")+", Duration : " + hours);
	}
	else {
		double hours = Double.parseDouble((String)jo.get("Duration"));
		map.put(jo.get("Name"),new Double(hours));
	}
	//while (employees.next()) {
		
		
}
Iterator iter = map.entrySet().iterator();
while(iter.hasNext()) {
	Map.Entry en = (Map.Entry)iter.next();
	JsonObject currentRecord = new JsonObject();
	
	currentRecord.add("Name",
			new JsonPrimitive((String)en.getKey()));
	DecimalFormat df = new DecimalFormat("####.#");
	System.out.println("************* : " + ((Double)en.getValue()).toString());
	currentRecord.add("Duration",
			new JsonPrimitive(df.format(((Double)en.getValue()).doubleValue()/60)));
	
	recordsArray.add(currentRecord);
}
// (D)
System.out.println(recordsArray.toString());
out.print(recordsArray);
out.flush();%>

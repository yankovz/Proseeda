<%@ page language="java" contentType="text/html; charset=windows-1255"
    pageEncoding="windows-1255"%>
<%@ page import="java.sql.*"%>
<%@ page import="com.google.gson.*"%>
<%
	// (A) database connection
	// "jdbc:mysql://localhost:3306/northwind" - the database url of the form jdbc:subprotocol:subname
	// "dbusername" - the database user on whose behalf the connection is being made
	// "dbpassword" - the user's password
	
	// (C) format returned ResultSet as a JSON array
	System.out.println("i was called");
	JsonArray recordsArray = new JsonArray();
	//while (employees.next()) {
		JsonObject currentRecord = new JsonObject();
		currentRecord.add("EmployeeID",
				new JsonPrimitive("5554"));
		currentRecord.add("FirstName",
				new JsonPrimitive("Ziv"));
		currentRecord.add("LastName",
				new JsonPrimitive("yan"));
		currentRecord.add("Title",
				new JsonPrimitive("vp"));
		currentRecord.add("BirthDate",
				new JsonPrimitive("7 June 75"));
		recordsArray.add(currentRecord);
	
	// (D)
	System.out.println(recordsArray.toString());
	out.print(recordsArray);
	out.flush();
%>

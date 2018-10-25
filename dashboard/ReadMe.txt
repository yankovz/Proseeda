Proseeda Sources are made of the following:

WebPages:

Index.html - c:\dev\Proseeda\dashboard\views\index.html
dayview.html - c:\dev\Proseeda\dashboard\views\dayview.html
	DayView will be using JSP pages for crud operation on mongo db
	
JSP pages
loaddata.jsp - c:\dev\Proseeda\crud\WebContent\jsp\loaddata.jsp

Node.js - tcp server
server.js - c:\dev\Proseeda\server\server.js

Outlook addin - C# based need visual studio
c:\dev\Proseeda\OutlookAddIn1\*

Wordaddin
c:\dev\Proseeda\WordAddIn1\*


Installing Proseeda
---------------------
Proseeda is running in AWS
prereq:
	MongoDB installation
	apache Tomcat 8.5 
	Node.js - need to install mongodb package for node (npm install mongodb)

instalation steps:
	sync proseeda repository from git
	deploy html pages mentioned above from %PROSEEDA_SOURCE_REPO%\Proseeda\dashboard\views (e.g. c:\dev\Proseeda\dashboard\views\) into %APACHE_TOMCAT_HOME%\webapps\proseeda\ (e.g. pache-tomcat-8.5.34\webapps\proseeda\)
	deploy jsp from %PROSEEDA_SOURCE_REPO%\Proseeda\crud\WebContent\jsp (e.g c:\dev\Proseeda\crud\WebContent\jsp\) pages mentioned above into %APACHE_TOMCAT_HOME%\webapps\proseeda\jsp (e.g. c:\dev\apache-tomcat-8.5.34\webapps\proseeda\jsp\)
	copy jar files from %PROSEEDA_SOURCE_REPO%\Proseeda\crud\lib\ to %APACHE_TOMCAT_HOME%\lib(e.g. c:\dev\apache-tomcat-8.5.34\lib\)
	start mongodb
	start tomcat server
	start node.js server.js (node server.js)
	@todo describe installation steps for outlook and word

access proseeda web on www.proseeda.com

	
	

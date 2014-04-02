<%@ Language="C#" Debug="true" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Simple Clickable XY Chart Handler</title>
</head>
<body style="margin:5px 0px 0px 5px">
<div style="font-size:18pt; font-family:verdana; font-weight:bold">
    Simple Clickable XY Chart Handler
</div>
<hr style="border:solid 1px #000080" />
<div style="font-size:10pt; font-family:verdana; margin-bottom:20px">
    <a href="viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>">View Source Code</a>
</div>
<div style="font-size:10pt; font-family:verdana;">
<b>You have clicked on the following chart element :</b><br />
<ul>
    <li>Data Set : <%=Request["dataSetName"]%></li>
    <li>X Position : <%=Request["x"]%></li>
    <li>X Label : <%=Request["xLabel"]%></li>
    <li>Data Value : <%=Request["value"]%></li>
</ul>
</div>
</body>
</html>

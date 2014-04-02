<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    // The data for the bar chart
    double[] data = {85, 156, 179.5, 211, 123};

    // The labels for the bar chart
    string[] labels = {"Mon", "Tue", "Wed", "Thu", "Fri"};

    // Create a XYChart object of size 250 x 250 pixels
    XYChart c = new XYChart(250, 250);

    // Set the plotarea at (30, 20) and of size 200 x 200 pixels
    c.setPlotArea(30, 20, 200, 200);

    // Add a bar chart layer using the given data
    c.addBarLayer(data);

    // Set the labels on the x axis.
    c.xAxis().setLabels(labels);

    // Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
        "title='{xLabel}: US${value}K'");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Simple Bar Chart</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Simple Bar Chart
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


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
    double[] data0 = {100, 125, 245, 147, 67};
    double[] data1 = {85, 156, 179, 211, 123};
    double[] data2 = {97, 87, 56, 267, 157};

    // The labels for the bar chart
    string[] labels = {"Mon", "Tue", "Wed", "Thu", "Fri"};

    // Create a XYChart object of size 500 x 320 pixels
    XYChart c = new XYChart(500, 320);

    // Set the plotarea at (100, 40) and of size 280 x 240 pixels
    c.setPlotArea(100, 40, 280, 240);

    // Add a legend box at (400, 100)
    c.addLegend(400, 100);

    // Add a title to the chart using 14 points Times Bold Itatic font
    c.addTitle("Weekday Network Load", "Times New Roman Bold Italic", 14);

    // Add a title to the y axis. Draw the title upright (font angle = 0)
    c.yAxis().setTitle("Average\nWorkload\n(MBytes\nPer Hour)").setFontAngle(0);

    // Set the labels on the x axis
    c.xAxis().setLabels(labels);

    // Add a stacked bar layer and set the layer 3D depth to 8 pixels
    BarLayer layer = c.addBarLayer2(Chart.Stack, 8);

    // Add the three data sets to the bar layer
    layer.addDataSet(data0, 0xff8080, "Server # 1");
    layer.addDataSet(data1, 0x80ff80, "Server # 2");
    layer.addDataSet(data2, 0x8080ff, "Server # 3");

    // Enable bar label for the whole bar
    layer.setAggregateLabelStyle();

    // Enable bar label for each segment of the stacked bar
    layer.setDataLabelStyle();

    // Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
        "title='{dataSetName} on {xLabel}: {value} MBytes/hour'");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Stacked Bar Chart</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Stacked Bar Chart
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


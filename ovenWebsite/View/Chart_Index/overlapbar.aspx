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
    double[] data0 = {100, 125, 156, 147, 87, 124, 178, 109, 140, 106, 192, 122};
    double[] data1 = {122, 156, 179, 211, 198, 177, 160, 220, 190, 188, 220, 270};
    double[] data2 = {167, 190, 213, 267, 250, 320, 212, 199, 245, 267, 240, 310};
    string[] labels = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug",
        "Sept", "Oct", "Nov", "Dec"};

    // Create a XYChart object of size 580 x 280 pixels
    XYChart c = new XYChart(580, 280);

    // Add a title to the chart using 14 pts Arial Bold Italic font
    c.addTitle("Product Revenue For Last 3 Years", "Arial Bold Italic", 14);

    // Set the plot area at (50, 50) and of size 500 x 200. Use two alternative
    // background colors (f8f8f8 and ffffff)
    c.setPlotArea(50, 50, 500, 200, 0xf8f8f8, 0xffffff);

    // Add a legend box at (50, 25) using horizontal layout. Use 8pts Arial as font,
    // with transparent background.
    c.addLegend(50, 25, false, "Arial", 8).setBackground(Chart.Transparent);

    // Set the x axis labels
    c.xAxis().setLabels(labels);

    // Draw the ticks between label positions (instead of at label positions)
    c.xAxis().setTickOffset(0.5);

    // Add a multi-bar layer with 3 data sets
    BarLayer layer = c.addBarLayer2(Chart.Side);
    layer.addDataSet(data0, 0xff8080, "Year 2003");
    layer.addDataSet(data1, 0x80ff80, "Year 2004");
    layer.addDataSet(data2, 0x8080ff, "Year 2005");

    // Set 50% overlap between bars
    layer.setOverlapRatio(0.5);

    // Add a title to the y-axis
    c.yAxis().setTitle("Revenue (USD in millions)");

    // Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
        "title='{xLabel} Revenue on {dataSetName}: {value} millions'");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Overlapping Bar Chart</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Overlapping Bar Chart
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


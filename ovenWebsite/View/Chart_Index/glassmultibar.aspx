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
    string[] labels = {"Mon", "Tue", "Wed", "Thur", "Fri"};

    // Create a XYChart object of size 540 x 375 pixels
    XYChart c = new XYChart(540, 375);

    // Add a title to the chart using 18 pts Times Bold Italic font
    c.addTitle("Average Weekly Network Load", "Times New Roman Bold Italic", 18);

    // Set the plotarea at (50, 55) and of 440 x 280 pixels in size. Use a vertical
    // gradient color from light blue (f9f9ff) to blue (6666ff) as background. Set
    // border and grid lines to white (ffffff).
    c.setPlotArea(50, 55, 440, 280, c.linearGradientColor(0, 55, 0, 335, 0xf9f9ff,
        0x6666ff), -1, 0xffffff, 0xffffff);

    // Add a legend box at (50, 28) using horizontal layout. Use 10pts Arial Bold as
    // font, with transparent background.
    c.addLegend(50, 28, false, "Arial Bold", 10).setBackground(Chart.Transparent);

    // Set the x axis labels
    c.xAxis().setLabels(labels);

    // Draw the ticks between label positions (instead of at label positions)
    c.xAxis().setTickOffset(0.5);

    // Set axis label style to 8pts Arial Bold
    c.xAxis().setLabelStyle("Arial Bold", 8);
    c.yAxis().setLabelStyle("Arial Bold", 8);

    // Set axis line width to 2 pixels
    c.xAxis().setWidth(2);
    c.yAxis().setWidth(2);

    // Add axis title
    c.yAxis().setTitle("Throughput (MBytes Per Hour)");

    // Add a multi-bar layer with 3 data sets
    BarLayer layer = c.addBarLayer2(Chart.Side);
    layer.addDataSet(data0, 0xff0000, "Server #1");
    layer.addDataSet(data1, 0x00ff00, "Server #2");
    layer.addDataSet(data2, 0xff8800, "Server #3");

    // Set bar border to transparent. Use glass lighting effect with light direction
    // from left.
    layer.setBorderColor(Chart.Transparent, Chart.glassEffect(Chart.NormalGlare,
        Chart.Left));

    // Configure the bars within a group to touch each others (no gap)
    layer.setBarGap(0.2, Chart.TouchBar);

    // Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
        "title='{dataSetName} on {xLabel}: {value} MBytes/hour'");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Glass Multi-Bar Chart</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Glass Multi-Bar Chart
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    // The data for the chart
    double[] data0 = {0.05, 0.06, 0.48, 0.1, 0.01, 0.05};
    double[] data1 = {100, 125, 265, 147, 67, 105};
    string[] labels = {"Jan", "Feb", "Mar", "Apr", "May", "Jun"};

    // Create a XYChart object of size 300 x 180 pixels
    XYChart c = new XYChart(300, 180);

    // Set the plot area at (50, 20) and of size 200 x 130 pixels
    c.setPlotArea(50, 20, 200, 130);

    // Add a title to the chart using 8 pts Arial Bold font
    c.addTitle("Independent Y-Axis Demo", "Arial Bold", 8);

    // Set the labels on the x axis.
    c.xAxis().setLabels(labels);

    // Add a title to the primary (left) y axis
    c.yAxis().setTitle("Packet Drop Rate (pps)");

    // Set the axis, label and title colors for the primary y axis to red (0xc00000)
    // to match the first data set
    c.yAxis().setColors(0xc00000, 0xc00000, 0xc00000);

    // Add a title to the secondary (right) y axis
    c.yAxis2().setTitle("Throughtput (MBytes)");

    // set the axis, label and title colors for the primary y axis to green
    // (0x008000) to match the second data set
    c.yAxis2().setColors(0x008000, 0x008000, 0x008000);

    // Add a line layer to for the first data set using red (0xc00000) color with a
    // line width to 3 pixels
    LineLayer lineLayer = c.addLineLayer(data0, 0xc00000);
    lineLayer.setLineWidth(3);

    // tool tip for the line layer
    lineLayer.setHTMLImageMap("", "",
        "title='Packet Drop Rate on {xLabel}: {value} pps'");

    // Add a bar layer to for the second data set using green (0x00C000) color. Bind
    // the second data set to the secondary (right) y axis
    BarLayer barLayer = c.addBarLayer(data1, 0x00c000);
    barLayer.setUseYAxis2();

    // tool tip for the bar layer
    barLayer.setHTMLImageMap("", "", "title='Throughput on {xLabel}: {value} MBytes'"
        );

    // Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dual Y-Axis</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Dual Y-Axis
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


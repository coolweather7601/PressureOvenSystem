<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Create chart
//
protected void createChart(WebChartViewer viewer, string img)
{
    // The value to display on the meter
    double value = 4.75;

    // Create an AugularMeter object of size 110 x 110 pixels, using silver
    // background color with a black 2 pixel 3D depressed border.
    AngularMeter m = new AngularMeter(110, 110, Chart.silverColor(), 0x000000, -2);

    // Set meter appearance according to a parameter
    if (img == "0") {
        // Set the meter center at bottom left corner (15, 95), with radius 85
        // pixels. Meter spans from 90 - 0 degrees.
        m.setMeter(15, 95, 85, 90, 0);
        // Add a label to the meter centered at (35, 75)
        m.addText(35, 75, "VDC", "Arial Bold", 12, Chart.TextColor, Chart.Center);
        // Add a text box to show the value at top right corner (103, 7)
        m.addText(103, 7, m.formatValue(value, "2"), "Arial", 8, 0xffffff,
            Chart.TopRight).setBackground(0, 0, -1);
    } else if (img == "1") {
        // Set the meter center at top left corner (15, 15), with radius 85 pixels.
        // Meter spans from 90 - 180 degrees.
        m.setMeter(15, 15, 85, 90, 180);
        // Add a label to the meter centered at (35, 35)
        m.addText(35, 35, "AMP", "Arial Bold", 12, Chart.TextColor, Chart.Center);
        // Add a text box to show the value at bottom right corner (103, 103)
        m.addText(103, 103, m.formatValue(value, "2"), "Arial", 8, 0xffffff,
            Chart.BottomRight).setBackground(0, 0, -1);
    } else if (img == "2") {
        // Set the meter center at top right corner (15, 95), with radius 85 pixels.
        // Meter spans from 270 - 180 degrees.
        m.setMeter(95, 15, 85, 270, 180);
        // Add a label to the meter centered at (75, 35)
        m.addText(75, 35, "KW", "Arial Bold", 12, Chart.TextColor, Chart.Center);
        // Add a text box to show the value at bottom left corner (7, 103)
        m.addText(7, 103, m.formatValue(value, "2"), "Arial", 8, 0xffffff,
            Chart.BottomLeft).setBackground(0, 0, -1);
    } else {
        // Set the meter center at bottom right corner (95, 95), with radius 85
        // pixels. Meter spans from 270 - 360 degrees.
        m.setMeter(95, 95, 85, 270, 360);
        // Add a label to the meter centered at (75, 75)
        m.addText(75, 75, "RPM", "Arial Bold", 12, Chart.TextColor, Chart.Center);
        // Add a text box to show the value at top left corner (7, 7)
        m.addText(7, 7, m.formatValue(value, "2"), "Arial", 8, 0xffffff,
            Chart.TopLeft).setBackground(0, 0, -1);
    }

    // Meter scale is 0 - 10, with a major tick every 2 units, and minor tick every 1
    // unit
    m.setScale(0, 10, 2, 1);

    // Set 0 - 6 as green (99ff99) zone, 6 - 8 as yellow (ffff00) zone, and 8 - 10 as
    // red (ff3333) zone
    m.addZone(0, 6, 0x99ff99, 0x808080);
    m.addZone(6, 8, 0xffff00, 0x808080);
    m.addZone(8, 10, 0xff3333, 0x808080);

    // Add a semi-transparent black (80000000) pointer at the specified value
    m.addPointer(value, unchecked((int)0x80000000));

    // Output the chart
    viewer.Image = m.makeWebImage(Chart.PNG);
}

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    createChart(WebChartViewer0, "0");
    createChart(WebChartViewer1, "1");
    createChart(WebChartViewer2, "2");
    createChart(WebChartViewer3, "3");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Square Angular Meters</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Square Angular Meters
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer0" runat="server" />
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
    <chart:WebChartViewer id="WebChartViewer2" runat="server" />
    <chart:WebChartViewer id="WebChartViewer3" runat="server" />
</body>
</html>


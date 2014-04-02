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
    double value = 6.5;

    // Create an AugularMeter object of size 200 x 100 pixels with rounded corners
    AngularMeter m = new AngularMeter(200, 100);
    m.setRoundedFrame();

    // Set meter background according to a parameter
    if (img == "0") {
        // Use gold background color
        m.setBackground(Chart.goldColor(), 0x000000, -2);
    } else if (img == "1") {
        // Use silver background color
        m.setBackground(Chart.silverColor(), 0x000000, -2);
    } else if (img == "2") {
        // Use metallic blue (9898E0) background color
        m.setBackground(Chart.metalColor(0x9898e0), 0x000000, -2);
    } else if (img == "3") {
        // Use a wood pattern as background color
        m.setBackground(m.patternColor2(Server.MapPath("wood.png")), 0x000000, -2);
    } else if (img == "4") {
        // Use a marble pattern as background color
        m.setBackground(m.patternColor2(Server.MapPath("marble.png")), 0x000000, -2);
    } else {
        // Use a solid light purple (EEBBEE) background color
        m.setBackground(0xeebbee, 0x000000, -2);
    }

    // Set the meter center at (100, 235), with radius 210 pixels, and span from -24
    // to +24 degress
    m.setMeter(100, 235, 210, -24, 24);

    // Meter scale is 0 - 100, with a tick every 1 unit
    m.setScale(0, 10, 1);

    // Set 0 - 6 as green (99ff99) zone, 6 - 8 as yellow (ffff00) zone, and 8 - 10 as
    // red (ff3333) zone
    m.addZone(0, 6, 0x99ff99, 0x808080);
    m.addZone(6, 8, 0xffff00, 0x808080);
    m.addZone(8, 10, 0xff3333, 0x808080);

    // Add a title at the bottom of the meter using 10 pts Arial Bold font
    m.addTitle2(Chart.Bottom, "OUTPUT POWER LEVEL\n", "Arial Bold", 10);

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
    createChart(WebChartViewer4, "4");
    createChart(WebChartViewer5, "5");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Wide Angular Meters</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Wide Angular Meters
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer0" runat="server" />
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
    <chart:WebChartViewer id="WebChartViewer2" runat="server" />
    <chart:WebChartViewer id="WebChartViewer3" runat="server" />
    <chart:WebChartViewer id="WebChartViewer4" runat="server" />
    <chart:WebChartViewer id="WebChartViewer5" runat="server" />
</body>
</html>


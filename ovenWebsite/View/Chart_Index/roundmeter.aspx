<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    // The value to display on the meter
    double value = 45.17;

    // Create an AugularMeter object of size 200 x 200 pixels, with silver
    // background, black border, 2 pixel 3D depressed border and rounded corners.
    AngularMeter m = new AngularMeter(200, 200, Chart.silverColor(), 0x000000, -2);
    m.setRoundedFrame();

    // Set the meter center at (100, 100), with radius 85 pixels, and span from -135
    // to +135 degress
    m.setMeter(100, 100, 85, -135, 135);

    // Meter scale is 0 - 100, with major tick every 10 units, minor tick every 5
    // units, and micro tick every 1 units
    m.setScale(0, 100, 10, 5, 1);

    // Disable default angular arc by setting its width to 0. Set 2 pixels line width
    // for major tick, and 1 pixel line width for minor ticks.
    m.setLineWidth(0, 2, 1);

    // Set the circular meter surface as metallic blue (9999DD)
    m.addRing(0, 90, Chart.metalColor(0x9999dd));

    // Add a blue (6666FF) ring between radii 88 - 90 as decoration
    m.addRing(88, 90, 0x6666ff);

    // Set 0 - 60 as green (99FF99) zone, 60 - 80 as yellow (FFFF00) zone, and 80 -
    // 100 as red (FF3333) zone
    m.addZone(0, 60, 0x99ff99);
    m.addZone(60, 80, 0xffff00);
    m.addZone(80, 100, 0xff3333);

    // Add a text label centered at (100, 135) with 15 pts Arial Bold font
    m.addText(100, 135, "CPU", "Arial Bold", 15, Chart.TextColor, Chart.Center);

    // Add a text box centered at (100, 165) showing the value formatted to 2 decimal
    // places, using white text on a black background, and with 1 pixel 3D depressed
    // border
    m.addText(100, 165, m.formatValue(value, "2"), "Arial", 8, 0xffffff, Chart.Center
        ).setBackground(0x000000, 0x000000, -1);

    // Add a semi-transparent blue (40333399) pointer at the specified value
    m.addPointer(value, 0x40333399);

    // Output the chart
    WebChartViewer1.Image = m.makeWebImage(Chart.PNG);
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Round Meter</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Round Meter
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


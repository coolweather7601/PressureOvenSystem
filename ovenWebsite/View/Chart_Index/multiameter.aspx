<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    // Create an AugularMeter object of size 200 x 200 pixels
    AngularMeter m = new AngularMeter(200, 200);

    // Use white on black color palette for default text and line colors
    m.setColors(Chart.whiteOnBlackPalette);

    // Set the meter center at (100, 100), with radius 85 pixels, and span from 0 to
    // 360 degress
    m.setMeter(100, 100, 85, 0, 360);

    // Meter scale is 0 - 100, with major tick every 10 units, minor tick every 5
    // units, and micro tick every 1 units
    m.setScale(0, 100, 10, 5, 1);

    // Set angular arc, major tick and minor tick line widths to 2 pixels.
    m.setLineWidth(2, 2, 2);

    // Add a blue (9999ff) ring between radii 88 - 90 as decoration
    m.addRing(88, 90, 0x9999ff);

    // Set 0 - 60 as green (00AA00) zone, 60 - 80 as yellow (CCCC00) zone, and 80 -
    // 100 as red (AA0000) zone
    m.addZone(0, 60, 0x00aa00);
    m.addZone(60, 80, 0xcccc00);
    m.addZone(80, 100, 0xaa0000);

    // Add a text label centered at (100, 70) with 12 pts Arial Bold font
    m.addText(100, 70, "PSI", "Arial Bold", 12, Chart.TextColor, Chart.Center);

    // Add a semi-transparent blue (806666FF) pointer    using the default shape
    m.addPointer(25, unchecked((int)0x806666ff), 0x6666ff);

    // Add a semi-transparent red (80FF6666) pointer using the arrow shape
    m.addPointer(9, unchecked((int)0x80ff6666), 0xff6666).setShape(
        Chart.ArrowPointer2);

    // Add a semi-transparent yellow (80FFFF66) pointer using another arrow shape
    m.addPointer(51, unchecked((int)0x80ffff66), 0xffff66).setShape(
        Chart.ArrowPointer);

    // Add a semi-transparent green (8066FF66) pointer using the line shape
    m.addPointer(72, unchecked((int)0x8066ff66), 0x66ff66).setShape(Chart.LinePointer
        );

    // Add a semi-transparent grey (80CCCCCC) pointer using the pencil shape
    m.addPointer(85, unchecked((int)0x80cccccc), 0xcccccc).setShape(
        Chart.PencilPointer);

    // Output the chart
    WebChartViewer1.Image = m.makeWebImage(Chart.PNG);
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Multi-Pointer Angular Meter</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Multi-Pointer Angular Meter
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


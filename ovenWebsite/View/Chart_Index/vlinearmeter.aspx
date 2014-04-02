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
    double value = 75.35;

    // Create an LinearMeter object of size 60 x 265 pixels, using silver background
    // with a 2 pixel black 3D depressed border.
    LinearMeter m = new LinearMeter(60, 265, Chart.silverColor(), 0, -2);

    // Set the scale region top-left corner at (25, 30), with size of 20 x 200
    // pixels. The scale labels are located on the left (default - implies vertical
    // meter)
    m.setMeter(25, 30, 20, 200);

    // Set meter scale from 0 - 100, with a tick every 10 units
    m.setScale(0, 100, 10);

    // Set 0 - 50 as green (99ff99) zone, 50 - 80 as yellow (ffff66) zone, and 80 -
    // 100 as red (ffcccc) zone
    m.addZone(0, 50, 0x99ff99);
    m.addZone(50, 80, 0xffff66);
    m.addZone(80, 100, 0xffcccc);

    // Add a deep blue (000080) pointer at the specified value
    m.addPointer(value, 0x000080);

    // Add a text box label at top-center (30, 5) using Arial Bold/8 pts/deep blue
    // (000088), with a light blue (9999ff) background
    m.addText(30, 5, "Temp C", "Arial Bold", 8, 0x000088, Chart.TopCenter
        ).setBackground(0x9999ff);

    // Add a text box to show the value formatted to 2 decimal places at bottom
    // center (30, 260). Use white text on black background with a 1 pixel depressed
    // 3D border.
    m.addText(30, 260, m.formatValue(value, "2"), "Arial", 8, 0xffffff,
        Chart.BottomCenter).setBackground(0, 0, -1);

    // Output the chart
    WebChartViewer1.Image = m.makeWebImage(Chart.PNG);
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Vertical Linear Meter</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Vertical Linear Meter
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


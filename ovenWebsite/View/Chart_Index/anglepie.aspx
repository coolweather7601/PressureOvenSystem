<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Create chart
//
protected void createChart(WebChartViewer viewer, string img)
{
    // Determine the starting angle and direction based on input parameter
    int angle = 0;
    bool clockwise = true;
    if (img != "0") {
        angle = 90;
        clockwise = false;
    }

    // The data for the pie chart
    double[] data = {60, 18, 15, 12, 8, 30, 35};

    // The labels for the pie chart
    string[] labels = {"LaborXXX", "Licenses", "Taxes", "Legal", "Insurance",
        "Facilities", "Production"};

    // Create a PieChart object of size 280 x 240 pixels
    PieChart c = new PieChart(280, 240);

    // Set the center of the pie at (140, 130) and the radius to 80 pixels
    c.setPieSize(140, 130, 80);

    // Add a title to the pie to show the start angle and direction
    if (clockwise) {
        c.addTitle("Start Angle = " + angle + " degrees\nDirection = Clockwise");
    } else {
        c.addTitle("Start Angle = " + angle + " degrees\nDirection = AntiClockwise");
    }

    // Set the pie start angle and direction
    c.setStartAngle(angle, clockwise);

    // Draw the pie in 3D
    c.set3D();

    // Set the pie data and the pie labels
    c.setData(data, labels);

    // Explode the 1st sector (index = 0)
    c.setExplode(0);

    // Output the chart
    viewer.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    viewer.ImageMap = c.getHTMLImageMap("", "",
        "title='{label}: US${value}K ({percent}%)'");
}

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    createChart(WebChartViewer0, "0");
    createChart(WebChartViewer1, "1");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Start Angle and Direction</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Start Angle and Direction
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer0" runat="server" />
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


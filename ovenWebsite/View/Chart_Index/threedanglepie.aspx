<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Create chart
//
protected void createChart(WebChartViewer viewer, string img)
{
    // the tilt angle of the pie
    int angle = int.Parse(img) * 15;

    // The data for the pie chart
    double[] data = {25, 18, 15, 12, 8, 30, 35};

    // The labels for the pie chart
    string[] labels = {"Labor", "Licenses", "Taxes", "Legal", "Insurance",
        "Facilities", "Production"};

    // Create a PieChart object of size 100 x 110 pixels
    PieChart c = new PieChart(100, 110);

    // Set the center of the pie at (50, 55) and the radius to 38 pixels
    c.setPieSize(50, 55, 38);

    // Set the depth and tilt angle of the 3D pie (-1 means auto depth)
    c.set3D(-1, angle);

    // Add a title showing the tilt angle
    c.addTitle("Tilt = " + angle + " deg", "Arial", 8);

    // Set the pie data
    c.setData(data, labels);

    // Disable the sector labels by setting the color to Transparent
    c.setLabelStyle("", 8, Chart.Transparent);

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
    createChart(WebChartViewer2, "2");
    createChart(WebChartViewer3, "3");
    createChart(WebChartViewer4, "4");
    createChart(WebChartViewer5, "5");
    createChart(WebChartViewer6, "6");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>3D Angle</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        3D Angle
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
    <chart:WebChartViewer id="WebChartViewer6" runat="server" />
</body>
</html>


<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Create chart
//
protected void createChart(WebChartViewer viewer, string img)
{
    // The data for the pyramid chart
    double[] data = {156, 123, 211, 179};

    // The semi-transparent colors for the pyramid layers
    int[] colors = {0x400000cc, 0x4066aaee, 0x40ffbb00, 0x40ee6622};

    // The rotation angle
    int angle = int.Parse(img) * 15;

    // Create a PyramidChart object of size 200 x 200 pixels, with white (ffffff)
    // background and grey (888888) border
    PyramidChart c = new PyramidChart(200, 200, 0xffffff, 0x888888);

    // Set the pyramid center at (100, 100), and width x height to 60 x 120 pixels
    c.setPyramidSize(100, 100, 60, 120);

    // Set the elevation to 15 degrees and use the given rotation angle
    c.addTitle("Rotation = " + angle, "Arial Italic", 15);
    c.setViewAngle(15, angle);

    // Set the pyramid data
    c.setData(data);

    // Set the layer colors to the given colors
    c.setColors2(Chart.DataColor, colors);

    // Leave 1% gaps between layers
    c.setLayerGap(0.01);

    // Output the chart
    viewer.Image = c.makeWebImage(Chart.PNG);
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
    <title>Pyramid Rotation</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Pyramid Rotation
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


<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Create chart
//
protected void createChart(WebChartViewer viewer, string img)
{
    // The data for the chart
    double[] data = {85, 156, 179.5, 211, 123};
    string[] labels = {"Mon", "Tue", "Wed", "Thu", "Fri"};

    // Create a XYChart object of size 270 x 270 pixels
    XYChart c = new XYChart(270, 270);

    // Set the plot area at (40, 32) and of size 200 x 200 pixels
    PlotArea plotarea = c.setPlotArea(40, 32, 200, 200);

    // Set the background style based on the input parameter
    if (img == "0") {
        // Has wallpaper image
        c.setWallpaper(Server.MapPath("tile.gif"));
    } else if (img == "1") {
        // Use a background image as the plot area background
        plotarea.setBackground2(Server.MapPath("bg.png"));
    } else if (img == "2") {
        // Use white (0xffffff) and grey (0xe0e0e0) as two alternate plotarea
        // background colors
        plotarea.setBackground(0xffffff, 0xe0e0e0);
    } else {
        // Use a dark background palette
        c.setColors(Chart.whiteOnBlackPalette);
    }

    // Set the labels on the x axis
    c.xAxis().setLabels(labels);

    // Add a color bar layer using the given data. Use a 1 pixel 3D border for the
    // bars.
    c.addBarLayer3(data).setBorderColor(-1, 1);

    // Output the chart
    viewer.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    viewer.ImageMap = c.getHTMLImageMap("", "",
        "title='Revenue for {xLabel}: US${value}K'");
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
    <title>Background and Wallpaper</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Background and Wallpaper
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


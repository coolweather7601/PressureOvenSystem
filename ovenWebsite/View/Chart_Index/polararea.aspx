<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    // Data for the chart
    double[] data0 = {5, 3, 10, 4, 3, 5, 2, 5};
    double[] data1 = {12, 6, 17, 6, 7, 9, 4, 7};
    double[] data2 = {17, 7, 22, 7, 18, 13, 5, 11};

    string[] labels = {"North", "North<*br*>East", "East", "South<*br*>East",
        "South", "South<*br*>West", "West", "North<*br*>West"};

    // Create a PolarChart object of size 460 x 500 pixels, with a grey (e0e0e0)
    // background and 1 pixel 3D border
    PolarChart c = new PolarChart(460, 500, 0xe0e0e0, 0x000000, 1);

    // Add a title to the chart at the top left corner using 15pts Arial Bold Italic
    // font. Use a wood pattern as the title background.
    c.addTitle("Polar Area Chart Demo", "Arial Bold Italic", 15).setBackground(
        c.patternColor(Server.MapPath("wood.png")));

    // Set center of plot area at (230, 280) with radius 180 pixels, and white
    // (ffffff) background.
    c.setPlotArea(230, 280, 180, 0xffffff);

    // Set the grid style to circular grid
    c.setGridStyle(false);

    // Add a legend box at top-center of plot area (230, 35) using horizontal layout.
    // Use 10 pts Arial Bold font, with 1 pixel 3D border effect.
    LegendBox b = c.addLegend(230, 35, false, "Arial Bold", 9);
    b.setAlignment(Chart.TopCenter);
    b.setBackground(Chart.Transparent, Chart.Transparent, 1);

    // Set angular axis using the given labels
    c.angularAxis().setLabels(labels);

    // Specify the label format for the radial axis
    c.radialAxis().setLabelFormat("{value}%");

    // Set radial axis label background to semi-transparent grey (40cccccc)
    c.radialAxis().setLabelStyle().setBackground(0x40cccccc, 0);

    // Add the data as area layers
    c.addAreaLayer(data2, -1, "5 m/s or above");
    c.addAreaLayer(data1, -1, "1 - 5 m/s");
    c.addAreaLayer(data0, -1, "less than 1 m/s");

    // Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
        "title='[{label}] {dataSetName}: {value}%'");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Polar Area Chart</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Polar Area Chart
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


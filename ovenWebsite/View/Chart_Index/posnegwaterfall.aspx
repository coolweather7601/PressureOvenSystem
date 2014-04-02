<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    // 4 data points to represent the cash flow for the Q1 - Q4
    double[] data = {230, -140, 220, 330};

    // We want to plot a waterfall chart showing the 4 quarters as well as the total
    string[] labels = {"1st Quarter", "2nd Quarter", "3rd Quarter", "4th Quarter",
        "Total"};

    // The top side of the bars in a waterfall chart is the accumulated data. We use
    // the ChartDirector ArrayMath utility to accumulate the data. The "total" is
    // handled by inserting a zero point at the end before accumulation (after
    // accumulation it will become the total).
    double[] boxTop = new ArrayMath(data).insert2(0, 1, data.Length).acc().result();

    // The botom side of the bars is just the top side of the previous bar. So we
    // shifted the top side data to obtain the bottom side data.
    double[] boxBottom = new ArrayMath(boxTop).shift(1, 0).result();

    // The last point (total) is different. Its bottom side is always 0.
    boxBottom[boxBottom.Length - 1] = 0;

    // In this example, we want to use different colors depending on the data is
    // positive or negative.
    int posColor = 0x00ff00;
    int negColor = 0xff0000;

    // Create a XYChart object of size 500 x 280 pixels. Set background color to
    // light blue (ccccff), with 1 pixel 3D border effect.
    XYChart c = new XYChart(500, 300, 0xccccff, 0x000000, 1);

    // Add a title to the chart using 13 points Arial Bold Itatic font, with white
    // (ffffff) text on a deep blue (0x80) background
    c.addTitle("Corporate Cash Flow - Year 2004", "Arial Bold Italic", 13, 0xffffff
        ).setBackground(0x000080);

    // Set the plotarea at (55, 50) and of size 430 x 215 pixels. Use alternative
    // white/grey background.
    c.setPlotArea(55, 50, 430, 215, 0xffffff, 0xeeeeee);

    // Add a legend box at (55, 25) using 8 pts Arial Bold font with horizontal
    // layout, with transparent background and border color.
    LegendBox b = c.addLegend(55, 25, false, "Arial Bold", 8);
    b.setBackground(Chart.Transparent, Chart.Transparent);

    // Add keys to show the colors for positive and negative cash flows
    b.addKey("Positive Cash Flow", posColor);
    b.addKey("Negative Cash Flow", negColor);

    // Set the labels on the x axis using Arial Bold font
    c.xAxis().setLabels(labels).setFontStyle("Arial Bold");

    // Set the x-axis ticks and grid lines to be between the bars
    c.xAxis().setTickOffset(0.5);

    // Use Arial Bold as the y axis label font
    c.yAxis().setLabelStyle("Arial Bold");

    // Add a title to the y axis
    c.yAxis().setTitle("USD (in millions)");

    // Add a box-whisker layer to represent the waterfall bars
    BoxWhiskerLayer layer = c.addBoxWhiskerLayer(boxTop, boxBottom);

    // Color the bars depending on whether it is positive or negative
    for(int i = 0; i < boxTop.Length; ++i) {
        if (boxTop[i] >= boxBottom[i]) {
            layer.setBoxColor(i, posColor);
        } else {
            layer.setBoxColor(i, negColor);
        }
    }

    // Put data labels on the bars to show the cash flow using Arial Bold font
    layer.setDataLabelFormat("{={top}-{bottom}}M");
    layer.setDataLabelStyle("Arial Bold").setAlignment(Chart.Center);

    // Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "",
        "title='{xLabel}: {={top}-{bottom}} millions'");
}

</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Pos/Neg Waterfall Chart</title>
</head>
<body style="margin:5px 0px 0px 5px">
    <div style="font-size:18pt; font-family:verdana; font-weight:bold">
        Pos/Neg Waterfall Chart
    </div>
    <hr style="border:solid 1px #000080" />
    <div style="font-size:10pt; font-family:verdana; margin-bottom:1.5em">
        <a href='viewsource.aspx?file=<%=Request["SCRIPT_NAME"]%>'>View Source Code</a>
    </div>
    <chart:WebChartViewer id="WebChartViewer1" runat="server" />
</body>
</html>


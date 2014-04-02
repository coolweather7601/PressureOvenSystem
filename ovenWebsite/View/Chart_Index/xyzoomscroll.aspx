<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="ChartDirector" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<script runat="server">

//
// Initialize the WebChartViewer when the page is first loaded
//
private void initViewer(WebChartViewer viewer)
{
    //
    // In this example, we just use default values, so there is nothing to initialize.
    //
}

//
// Draw the chart
//
private void drawChart(WebChartViewer viewer)
{
    //
    // For simplicity, in this demo, the data arrays are filled with hard coded data. In a real
    // application, you may use a database or other data source to load up the arrays.
    //
    double[] dataX0 = {10, 15, 6, -12, 14, -8, 13, -3, 16, 12, 10.5, -7, 3, -10, -5, 2, 5};
    double[] dataY0 = {130, 150, 80, 110, -110, -105, -130, -15, -170, 125, 125, 60, 25, 150, 150,
        15, 120};
    double[] dataX1 = {6, 7, -4, 3.5, 7, 8, -9, -10, -12, 11, 8, -3, -2, 8, 4, -15, 15};
    double[] dataY1 = {65, -40, -40, 45, -70, -80, 80, 10, -100, 105, 60, 50, 20, 170, -25, 50, 75};
    double[] dataX2 = {-10, -12, 11, 8, 6, 12, -4, 3.5, 7, 8, -9, 3, -13, 16, -7.5, -10, -15};
    double[] dataY2 = {65, -80, -40, 45, -70, -80, 80, 90, -100, 105, 60, -75, -150, -40, 120, -50,
        -30};

    // Create an XYChart object 500 x 480 pixels in size, with light blue (c0c0ff) as background
    // color
    XYChart c = new XYChart(500, 480, 0xc0c0ff);

    // Set the plotarea at (50, 40) and of size 400 x 400 pixels. Use light grey (c0c0c0) horizontal
    // and vertical grid lines. Set 4 quadrant coloring, where the colors of the quadrants alternate
    // between lighter and deeper grey (dddddd/eeeeee).
    c.setPlotArea(50, 40, 400, 400, -1, -1, -1, 0xc0c0c0, 0xc0c0c0).set4QBgColor(0xdddddd, 0xeeeeee,
        0xdddddd, 0xeeeeee, 0x000000);

    // As the data can lie outside the plotarea in a zoomed chart, we need enable clipping
    c.setClipping();

    // Set 4 quadrant mode, with both x and y axes symetrical around the origin
    c.setAxisAtOrigin(Chart.XYAxisAtOrigin, Chart.XAxisSymmetric + Chart.YAxisSymmetric);

    // Add a legend box at (450, 40) (top right corner of the chart) with vertical layout and 8 pts
    // Arial Bold font. Set the background color to semi-transparent grey (40dddddd).
    LegendBox legendBox = c.addLegend(450, 40, true, "Arial Bold", 8);
    legendBox.setAlignment(Chart.TopRight);
    legendBox.setBackground(0x40dddddd);

    // Add titles to axes
    c.xAxis().setTitle("Alpha Index");
    c.yAxis().setTitle("Beta Index");

    // Set axes line width to 2 pixels
    c.xAxis().setWidth(2);
    c.yAxis().setWidth(2);

    // The default ChartDirector settings has a denser y-axis grid spacing and less-dense x-axis
    // grid spacing. In this demo, we want the tick spacing to be symmetrical. We use around 40
    // pixels between major ticks and 20 pixels between minor ticks.
    c.xAxis().setTickDensity(40, 20);
    c.yAxis().setTickDensity(40, 20);

    //
    // In this example, we represent the data by scatter points. You may modify the code below to
    // use other layer types (lines, areas, etc).
    //

    // Add scatter layer, using 11 pixels red (ff33333) X shape symbols
    c.addScatterLayer(dataX0, dataY0, "Group A", Chart.Cross2Shape(), 11, 0xff3333);

    // Add scatter layer, using 11 pixels green (33ff33) circle symbols
    c.addScatterLayer(dataX1, dataY1, "Group B", Chart.CircleShape, 11, 0x33ff33);

    // Add scatter layer, using 11 pixels blue (3333ff) triangle symbols
    c.addScatterLayer(dataX2, dataY2, "Group C", Chart.TriangleSymbol, 11, 0x3333ff);

    //
    // In this example, we have not explicitly configured the full x and y range. In this case, the
    // first time syncLinearAxisWithViewPort is called, ChartDirector will auto-scale the axis and
    // assume the resulting range is the full range. In subsequent calls, ChartDirector will set the
    // axis range based on the view port and the full range.
    //
    viewer.syncLinearAxisWithViewPort("x", c.xAxis());
    viewer.syncLinearAxisWithViewPort("y", c.yAxis());

    // Output the chart
    viewer.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    viewer.ImageMap = c.getHTMLImageMap("", "",
        "title='[{dataSetName}] Alpha = {x|G}, Beta = {value|G}'");
}

//
// Page Load event handler
//
protected void Page_Load(object sender, EventArgs e)
{
    //
    // This script handles both the full page request, as well as the subsequent partial updates
    // (AJAX chart updates). We need to determine the type of request first before we processing it.
    //
    if (WebChartViewer.IsPartialUpdateRequest(Page)) {
        // Is a partial update request.

        // The .NET platform will not restore the states of the controls before or during Page_Load,
        // so we need to restore the state ourselves
        WebChartViewer1.LoadViewerState();

        // Draw the chart in partial update mode
        drawChart(WebChartViewer1);

        // Output the chart immediately and then terminate the page life cycle (PartialUpdateChart
        // will cause Page_Load to terminate immediately without running the following code).
        WebChartViewer1.PartialUpdateChart();
    }

    //
    // If the code reaches here, it is a full page request.
    //

    // In this exapmle, we just need to initialize the WebChartViewer and draw the chart.
    initViewer(WebChartViewer1);
    drawChart(WebChartViewer1);
}

</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>XY Zooming and Scrolling</title>
    <script type="text/javascript" src="cdjcv.js"></script>
    <style type="text/css">
        div.chartPushButton { padding:5px; cursor:hand; }
        div.chartPushButton:hover { background-color: #EEEEFF; }
        div.chartPushButtonPressed { padding:5px; cursor:hand; background-color: #CCFFCC; }
        td.chartPushButton { font:12px Verdana; border-bottom:#000000 1px solid; }
    </style>
</head>
<body style="margin:0px">
<script type="text/javascript">

//
// Execute the following initialization code after the web page is loaded
//
JsChartViewer.addEventListener(window, 'load', function() {
    // Update the chart when the view port has changed (eg. when the user zooms in using the mouse)
    var viewer = JsChartViewer.get('<%=WebChartViewer1.ClientID%>');
    viewer.attachHandler("ViewPortChanged", viewer.partialUpdate);

    // Set the zoom and scroll mode to bi-directional
    viewer.setScrollDirection(JsChartViewer.HorizontalVertical);
    viewer.setZoomDirection(JsChartViewer.HorizontalVertical);

    // Set the initial mouse usage to "zoom in"
    setMouseMode(JsChartViewer.ZoomIn);

    // Initialize the navigation pad
    initNavigationPad('NavigationPad', viewer.getId());
});

//
// This method is called when the user clicks on the Pointer, Zoom In or Zoom Out buttons
//
function setMouseMode(mode)
{
    // Set the button color based on the selected mouse mode
    document.getElementById("scrollButton").className =
        (mode  == JsChartViewer.Scroll) ? "chartPushButtonPressed" : "chartPushButton";
    document.getElementById("zoomInButton").className =
        (mode  == JsChartViewer.ZoomIn) ? "chartPushButtonPressed" : "chartPushButton";
    document.getElementById("zoomOutButton").className =
        (mode  == JsChartViewer.ZoomOut) ? "chartPushButtonPressed" : "chartPushButton";

    // Set the mouse mode
    JsChartViewer.get('<%=WebChartViewer1.ClientID%>').setMouseUsage(mode);
}

//
// The following functions implements the Navigation Pad user interface
//

// Initialize the Navigation Pad user interface
function initNavigationPad(controlId, viewerId)
{
    // Get the draggable rectangle of the Navigation Pad and the Javascript ChartViewer
    var handle = document.getElementById(controlId);
    var viewer = JsChartViewer.get(viewerId);

    // Set up the mouse down event handler to initiate dragging
    handle.onmousedown = NavigationPad_mouseDown;

    // Remember the width and height of the outer container and the id of the associated Javascript ChartViewer
    var parent = handle.parentElement || handle.parentNode;
    handle.parentW = parent.clientWidth;
    handle.parentH = parent.clientHeight;
    handle.viewerId = viewerId;

    // Set up the Navigation Pad to reflect the current view port
    updateNavigationPad(controlId);

    // When the chart is updated (eg. when zoomed in), the Navigation Pad needs to be updated too.
    viewer.attachHandler("PostUpdate", function() { updateNavigationPad(controlId); });

    // The Navigation Pad has been set up, so can display it now.
    handle.style.visibility = "visible";
}

// Update the Navigation Pad to reflect the view port
function updateNavigationPad(controlId)
{
    // Get the draggable rectangle of the Navigation Pad and the Javascript ChartViewer
    var handle = document.getElementById(controlId);
    var viewer = JsChartViewer.get(handle.viewerId);

    // Set the size and position of the handle based on Javascript ChartViewer view port
    handle.currentWidth = Math.round(handle.parentW * viewer.getViewPortWidth());
    handle.currentHeight = Math.round(handle.parentH * viewer.getViewPortHeight());
    handle.currentX = Math.round(handle.parentW * viewer.getViewPortLeft());
    handle.currentY = Math.round(handle.parentH * viewer.getViewPortTop());

    handle.style.width = Math.max(0, (handle.currentWidth - handle.offsetWidth + parseInt(handle.style.width))) + "px";
    handle.style.height = Math.max(0, (handle.currentHeight - handle.offsetHeight + parseInt(handle.style.height))) + "px";
    handle.style.left = handle.currentX + "px";
    handle.style.top = handle.currentY + "px";
}

// The Navigation Pad mouse down event handler
function NavigationPad_mouseDown(e)
{
    // Prevent browser default action, if any.
    if (e && e.preventDefault) e.preventDefault();

    // To initiate drag and drop, we need to capture the mouse and handle the mouse move and mouse up
    if (document.onmousemove != NavigationPad_mouseMove)
    {
        if (this.setCapture) this.setCapture();

        // Remember the current onmousemove and onmouseup event handlers and replace them with our own handlers
        document.NavigationPad_onmousemovesave = document.onmousemove;
        document.NavigationPad_onmouseupsave = document.onmouseup;
        document.onmousemove = NavigationPad_mouseMove;
        document.onmouseup = NavigationPad_mouseUp;
    }

    // Remember the mouse down position
    document.activeNavigationPad = this;
    this.refX = this.currentX - (window.event || e).clientX;
    this.refY = this.currentY - (window.event || e).clientY;
}

// The Navigation Pad mouse move event handler
function NavigationPad_mouseMove(e)
{
    // Set the position of the navigation pad depending on how far the mouse has been dragged
    e = e || window.event;
    var handle = document.activeNavigationPad;
    handle.currentX = Math.max(0, Math.min(handle.refX + e.clientX, handle.parentW - handle.currentWidth));
    handle.currentY = Math.max(0, Math.min(handle.refY + e.clientY, handle.parentH - handle.currentHeight));
    handle.style.left = handle.currentX + "px";
    handle.style.top = handle.currentY + "px";
    return false;
}

// The Navigation Pad mouse up event handler
function NavigationPad_mouseUp(e)
{
    // Release the mouse and restore the original nmousemove and onmouseup event handlers
    if (this.releaseCapture) this.releaseCapture();
    document.onmousemove = document.NavigationPad_onmousemovesave;
    document.onmouseup = document.NavigationPad_onmouseupsave;

    // Get the draggable rectangle of the Navigation Pad and the Javascript ChartViewer
    var handle = document.activeNavigationPad;
    var viewer = JsChartViewer.get(handle.viewerId);

    // Compute the new view port position based on the handle position
    var newVpLeft = 0;
    var newVpTop = 0;
    if (viewer.getViewPortWidth() < 1)
        newVpLeft = handle.currentX / (handle.parentW - handle.currentWidth) * (1 - viewer.getViewPortWidth());
    if (viewer.getViewPortHeight() < 1)
        newVpTop = handle.currentY / (handle.parentH - handle.currentHeight) * (1 - viewer.getViewPortHeight());

    // Update the view port if the new view port position is different from the old position
    if ((Math.abs(viewer.getViewPortLeft() - newVpLeft) > 0.0000001) ||
        (Math.abs(viewer.getViewPortTop() - newVpTop) > 0.0000001))
    {
        viewer.setViewPortLeft(newVpLeft);
        viewer.setViewPortTop(newVpTop);
        viewer.raiseViewPortChangedEvent();
    }
}

</script>
<form method="post" id="XYZoomScroll" runat="server">
<table cellspacing="0" cellpadding="0" style="border:black 1px solid;">
    <tr>
        <td align="right" colspan="2" style="background:#000088; color:#ffff00; padding:0px 4px 2px 0px;">
            <a style="color:#FFFF00; font:italic bold 10pt Arial; text-decoration:none" href="http://www.advsofteng.com/">
                Advanced Software Engineering
            </a>
        </td>
    </tr>
    <tr valign="top">
        <td style="width:130px; background:#e0e0e0;">
            <!-- The following table is to create 3 cells for 3 buttons to control the mouse usage mode. -->
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td class="chartPushButton">
                        <div class="chartPushButton" id="scrollButton" onclick="setMouseMode(JsChartViewer.Scroll)">
                            <img src="pointer.gif" style="vertical-align:middle" alt="Pointer" />&nbsp;&nbsp;Pointer
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="chartPushButton">
                        <div class="chartPushButton" id="zoomInButton" onclick="setMouseMode(JsChartViewer.ZoomIn)">
                            <img src="zoomInIcon.gif" style="vertical-align:middle" alt="Zoom In" />&nbsp;&nbsp;Zoom In
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="chartPushButton">
                        <div class="chartPushButton" id="zoomOutButton" onclick="setMouseMode(JsChartViewer.ZoomOut)">
                            <img src="zoomOutIcon.gif" style="vertical-align:middle" alt="Zoom Out" />&nbsp;&nbsp;Zoom Out
                        </div>
                    </td>
                </tr>
            </table>
            <br /><br /><br /><br />
            <div style="margin:10px 4px; text-align:center">
                <!-- The following two DIV blocks constitute the Navigation Pad and its container. -->
                <div style="border:black 1px solid; width:120px; height:120px; background-color:#c0c0ff; text-align:left">
                    <div id="NavigationPad" style="border:black 1px solid; visibility:hidden; width:60px; height:60px;
                        position:relative; background-color:#c0c0c0">
                        <img src="" style="display:none" height="1" width="1" alt="" />
                    </div>
                </div>
            </div>
        </td>
        <td style="border-left: black 1px solid; background-color: #c0c0ff; padding:5px">
            <chart:WebChartViewer id="WebChartViewer1" runat="server" width="500px" height="480px" />
        </td>
    </tr>
</table>
</form>
</body>
</html>

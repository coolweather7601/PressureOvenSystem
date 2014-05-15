<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master/MasterPage.master" AutoEventWireup="true" CodeFile="chart_realtimetrack.aspx.cs" Inherits="nModBusWeb.chart_realtimetrack" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagPrefix="chart" Namespace="ChartDirector" Assembly="netchartdir" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<script type="text/javascript" src="../../js/cdjcv.js"></script>
<script type="text/javascript">

    //
    // Execute the following initialization code after the web page is loaded
    //
    JsChartViewer.addEventListener(window, 'load', function () {
        var viewer = JsChartViewer.get('<%=WebChartViewer1.ClientID%>');

        // Draw track cursor when mouse is moving over plotarea or if the chart is updated. In the latter case,
        // if the mouse is not on the plot area, we will update the legend to show the latest data values.
        viewer.attachHandler(["MouseMovePlotArea", "PostUpdate"], function (e) {
            if (viewer.isMouseOnPlotArea())
                trackLineLegend(viewer, viewer.getPlotAreaMouseX())
            else
                trackLineLegend(viewer, viewer.getChart().getPlotArea().getRightX());
        });

        // Initialize the track line legend to show the latest data values
        trackLineLegend(viewer, viewer.getChart().getPlotArea().getRightX());

        // When the chart is being updated, by default, an "Updating" box will pop up. In this example, we
        // will disable this box.
        viewer.updatingMsg = "";
    });

    //
    // Draw track line with legend
    //
    function trackLineLegend(viewer, mouseX) {
        // Remove all previously drawn tracking object
        viewer.hideObj("all");

        // The chart and its plot area
        var c = viewer.getChart();
        var plotArea = c.getPlotArea();

        // Get the data x-value that is nearest to the mouse, and find its pixel coordinate.
        var xValue = c.getNearestXValue(mouseX);
        var xCoor = c.getXCoor(xValue);
        if (xCoor == null)
            return;

        // Draw a vertical track line at the x-position
        viewer.drawVLine("trackLine", xCoor, plotArea.getTopY(), plotArea.getBottomY(), "black 1px dotted");

        // Array to hold the legend entries
        var legendEntries = [];

        // Iterate through all layers to build the legend array
        for (var i = 0; i < c.getLayerCount(); ++i) {
            var layer = c.getLayerByZ(i);

            // The data array index of the x-value
            var xIndex = layer.getXIndexOf(xValue);

            // Iterate through all the data sets in the layer
            for (var j = 0; j < layer.getDataSetCount(); ++j) {
                var dataSet = layer.getDataSetByZ(j);

                // We are only interested in visible data sets with names, as they are required for legend entries.
                var dataName = dataSet.getDataName();
                var color = dataSet.getDataColor();
                if ((!dataName) || (color == null))
                    continue;

                // Build the legend entry, consist of a colored square box, the name and the data value.
                var dataValue = dataSet.getValue(xIndex);
                legendEntries.push("<nobr>" + viewer.htmlRect(7, 7, color) + " " + dataName + ": " +
                ((dataValue == null) ? "N/A" : dataValue.toPrecision(4)) + viewer.htmlRect(20, 0) + "</nobr> ");

                // Draw a track dot for data points within the plot area
                var yCoor = c.getYCoor(dataSet.getPosition(xIndex), dataSet.getUseYAxis());
                if ((yCoor != null) && (yCoor >= plotArea.getTopY()) && (yCoor <= plotArea.getBottomY())) {
                    viewer.showTextBox("dataPoint" + i + "_" + j, xCoor, yCoor, JsChartViewer.Center,
                    viewer.htmlRect(7, 7, color));
                }
            }
        }

        // Create the legend by joining the legend entries.
        var legend = "<nobr>[" + c.xAxis().getFormattedLabel(xValue, "hh:nn:ss") + "]" + viewer.htmlRect(20, 0) +
        "</nobr> " + legendEntries.reverse().join("");

        // Display the legend on the top of the plot area
        viewer.showTextBox("legend", plotArea.getLeftX(), plotArea.getTopY(), JsChartViewer.BottomLeft, legend,
        "width:" + (plotArea.getWidth() - 1) + "px;font:bold 11px Arial;padding:3px;");
    }

    //
    // Executes once every second to update the countdown display. Updates the chart when the countdown reaches 0.
    //
    function timerTick() {
        // Get the update period and the time left
        var updatePeriod = parseInt(document.getElementById("UpdatePeriod").value);
        var timeLeft = Math.min(parseInt(document.getElementById("TimeRemaining").innerHTML), updatePeriod) - 1;

        if (timeLeft == 0)
        // Can update the chart now
            JsChartViewer.get('<%=WebChartViewer1.ClientID%>').partialUpdate();
        else if (timeLeft < 0)
        // Reset the update period
            timeLeft += updatePeriod;

        // Update the countdown display
        document.getElementById("TimeRemaining").innerHTML = timeLeft;
    }
    window.setInterval("timerTick()", 1000);

</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td align="right" colspan="2" style="background: #000088">
                    <div style="font: italic bold 10pt Arial; padding: 1px 3px 2px 0px;">
                        <a style="color: #FFFF00; text-decoration: none" href="http://www.advsofteng.com/">Advanced
                            Software Engineering </a>
                    </div>
                </td>
            </tr>
            <tr valign="top">
                <td style="width: 150px; background: #c0c0ff; border-right: black 1px solid; border-bottom: black 1px solid;">
                    <br />
                    <br />
                    <div style="font: 9pt Verdana; padding: 10px;">
                        <b>Update Period</b><br />
                        <select id="UpdatePeriod" style="width: 130px">
                            <option value="5">5</option>
                            <option value="10" selected="selected">10</option>
                            <option value="20">20</option>
                            <option value="30">30</option>
                            <option value="60">60</option>
                        </select>
                    </div>
                    <div style="font: 9pt Verdana; padding: 10px;">
                        <b>Time Remaining</b><br />
                        <div style="width: 128px; border: #888888 1px inset;">
                            <div style="margin: 3px" id="TimeRemaining">
                                0</div>
                        </div>
                    </div>
                </td>
                <td>
                    <div style="font: bold 20pt Arial; margin: 5px 0px 0px 5px;">
                        Realtime Chart with Track Line
                    </div>
                    <hr style="border: solid 1px #000080" />
                    <div style="padding: 0px 5px 5px 10px">
                        <chart:WebChartViewer ID="WebChartViewer1" runat="server" Width="600px" Height="270px" />
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
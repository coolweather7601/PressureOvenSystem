/* Define the site identitier */
_page.siteIdentifier = 'APK ModbusWeb_Oven <font color="red">(trial-run site)</font>'
_page.hideLeftNavigation = true;

/* Define the site menu structure using an associative array */

//_page.items["0"]        = new _Item("List","../others/listIndex.aspx")

_page.items["0"] = new _Item("Check-In", "../Others/CheckIn.aspx")

_page.items["1"] = new _Item("PTN Maintain", "../Others/PTN_maintain.aspx")
_page.items["1_1"] = new _Item("PTN Maintain", "../Others/PTN_maintain.aspx")
_page.items["1_2"] = new _Item("Oven Manage", "../Others/oven_manage.aspx")

//_page.items["2"] = new _Item("Chart_Index", "../Chart_Index/Index.htm")

_page.items["2"] = new _Item("Trendline", "../Chart/chart_trendline.aspx")
_page.items["2_1"] = new _Item("Trendline", "../Chart/chart_trendline.aspx")
_page.items["2_2"] = new _Item("Multiaxes", "../Chart/chart_multiaxes.aspx")
/* Define the site menu structure using an associative array */
_page.sites[0] = new _Site("Semiconductors", "http://twgkhhpsk1ms011 ")
_page.sites[1] = new _Site("APK","http://twgkhhpsk1ms011/imo/backend/kaohsiung/")


/* This table of content is required for the Autonomy crawler 
<meta name="robots" content="noindex, follow" />
<a href="index.html"></a>
*/

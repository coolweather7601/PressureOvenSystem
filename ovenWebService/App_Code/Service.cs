using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Diagnostics;
using System.Web.Hosting;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class Service : System.Web.Services.WebService
{
    public Service () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string callConsole(string parmes)
    {
        Process w = new Process();
        //指定 調用程序的路徑
        
        //w.StartInfo.FileName = Request.PhysicalApplicationPath + @"ovenWin\ovenWin\bin\debug\ovenWin.exe";
        w.StartInfo.FileName = HostingEnvironment.ApplicationPhysicalPath +System.Configuration.ConfigurationManager.AppSettings["applicationPath"].ToString();
        w.StartInfo.UseShellExecute = false;
        //不顯示執行窗口
        w.StartInfo.CreateNoWindow = false;

        //指定 調用程序的參數
        w.StartInfo.Arguments = parmes;
        w.Start();

        return w.StartInfo.FileName;
    }
}
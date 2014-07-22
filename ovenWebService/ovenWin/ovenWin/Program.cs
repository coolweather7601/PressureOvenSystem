using System;
using System.Collections.Generic;
using System.Text;
//step1. reference nmodbuspc.dll, and using the namespaces.
using Modbus.Device;      //for modbus master
using System.IO.Ports;    //for controlling serial ports
using System.Net.Sockets;
using System.Data;
using System.Timers;
using System.Diagnostics;
using System.Data.OracleClient;

namespace ovenWin
{
    class Program
    {
        static private SerialPort serialPort;
        static private ModbusSerialMaster master;
        static byte slaveID = 1;
        static ushort startAddress;
        static ushort numOfPoints = 8;
        //Timer        
        static string Machine_ID;
        static private int TimeInterval = 1000 * 60, StartTime = 0;
        static private string Batch_NO;
        static private double ScanTime,bakeTime, LimitTemp, LimitPressure, bakeTime2, LimitTemp2, LimitPressure2;
        static private bool reachTemp = false, reachPress = false, reachTemp2 = false, reachPress2 = false;
        static string maillist = "Db.prod@nxp.com,db.inst@nxp.com,db.rm@nxp.com,c.h.fang@nxp.com,howard.liu@nxp.com,h.f.fong@nxp.com,chien-chou.kuo@nxp.com,alan.kuo@nxp.com";
        static double control_Limit_Press = 0.5, control_Limit_Temp = 5;

        //oven_assy_logkindid=0 => process done;
        //oven_assy_logkindid=1 => pressure; 
        //oven_assy_logkindid=2 => Temperature(CH1); 
        //oven_assy_logkindid=3 => Temperature(CH2); 
        //oven_assy_logkindid=4 => Temperature(CH3);
        //oven_assy_logkindid=616~660 => exception code

        static void Main(string[] args)
        {
            try
            {
                // create a new SerialPort object with default settings.
                serialPort = new SerialPort();

                serialPort.BaudRate = 9600;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.DataBits = 8;

                //args = new string[] { "COM11", "OV0204-1" , "3" , "1-0-2-110-7|2-0-1-126-7", "941E43C108,941E43C207"}; //LOCAL TEST
                if (args != null && args.Length > 0)
                {
                    //args[0] -> COM
                    //args[1] -> Machine_ID
                    //args[2] -> BakeTime(minutes)
                    //args[3] -> PROCESS-1.2.3... (Process1-Hour1-Min1-Temp1-Pressure1|Process2-Hour2-Min2-Temp2-Pressure2)
                    //args[4] -> Batch_NO1,Batch_NO2

                    // set the appropriate properties.
                    serialPort.PortName = args[0].ToString();
                    Machine_ID = args[1].ToString();
                    Batch_NO = args[4].ToString().ToUpper();

                    string[] arrPara = args[3].ToString().Split('|');
                    bakeTime = (Convert.ToInt32(arrPara[0].Split('-')[1]) * 60 + Convert.ToInt32(arrPara[0].Split('-')[2])) * 60 * 1000;
                    LimitTemp = Convert.ToInt32(arrPara[0].Split('-')[3]);
                    LimitPressure = Convert.ToInt32(arrPara[0].Split('-')[4]);
                    Console.WriteLine("bakeTime" + bakeTime + Environment.NewLine);
                    Console.WriteLine("LimitTemp" + LimitTemp + Environment.NewLine);
                    Console.WriteLine("LimitPressure: " + LimitPressure + Environment.NewLine);
                    if (arrPara.Length > 1)
                    {
                        bakeTime2 = (Convert.ToInt32(arrPara[1].Split('-')[1]) * 60 + Convert.ToInt32(arrPara[1].Split('-')[2])) * 60 * 1000;
                        LimitTemp2 = Convert.ToInt32(arrPara[1].Split('-')[3]);
                        LimitPressure2 = Convert.ToInt32(arrPara[1].Split('-')[4]);
                        Console.WriteLine("bakeTime2" + bakeTime2 + Environment.NewLine);
                        Console.WriteLine("LimitTemp2" + LimitTemp2 + Environment.NewLine);
                        Console.WriteLine("LimitPressure2: " + LimitPressure2 + Environment.NewLine);
                    }
                    ScanTime = bakeTime + bakeTime2;
                    //ScanTime = Convert.ToDouble(args[2].ToString()) * 60 * 1000;

                    Console.WriteLine("COM: " + args[0].ToString() + Environment.NewLine);
                    Console.WriteLine("MachineID: " + args[1].ToString() + Environment.NewLine);
                    Console.WriteLine("args[3]: " + args[3].ToString() + Environment.NewLine);
                    Console.WriteLine("ScanTime: " + ScanTime + Environment.NewLine);
                    Console.WriteLine("Batch_NO: " + Batch_NO + Environment.NewLine);

                    serialPort.Open();
                    // create Modbus RTU Master by the comport client
                    //document->Modbus.Device.Namespace->ModbusSerialMaster Class->CreateRtu Method
                    master = ModbusSerialMaster.CreateRtu(serialPort);
                    master.Transport.ReadTimeout = 300;

                    //test
                    System.Timers.Timer tmrx = new System.Timers.Timer();
                    tmrx.Interval = TimeInterval; //set interval of checking here
                    tmrx.Elapsed += delegate
                    {
                        tmr_func(tmrx);
                    };
                    Mode(machineMode.open);// active oven power
                    tmrx.Start();
                    Console.ReadKey(true);
                }
                else
                {
                    Console.WriteLine(string.Format(@"No args[]. {0}.", DateTime.Now.ToString()));
                    Console.ReadKey(true);
                    Environment.Exit(0);        
                }
            }
            catch (Exception ex) { Console.WriteLine("Exception: " + ex.ToString()); }
        }

        private static void tmr_func(System.Timers.Timer _tmr)
        {
            Console.WriteLine(string.Format(@"Timer Start : {0}.",DateTime.Now.ToString()));
            string[] arrBatch = Batch_NO.Split(',');

            StartTime += TimeInterval;

            try
            {
                //byte slaveID = 1;
                //ushort startAddress;                
                //ushort numOfPoints = 8;
                //master.Transport.ReadTimeout = 300;

                //string Conn = ovenWin.Properties.Settings.Default.Oven;
                string Conn = ovenWin.Properties.Settings.Default.Oven_Trial;
                App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
                App_Code.FunctionCode fc = new App_Code.FunctionCode();

                List<string> arrStr = new List<string>();
                string transaction_str1 = "", transaction_str2 = "", transaction_str3 = "", transaction_str4 = "", isOverSpec = "N";
                //==============================================================
                //insert log (Pressure & CH1 CH2 CH3)
                //over spec, warring letter
                //==============================================================
                double TempTarget = 0, PressTarget = 0;
                #region Process1 Target value

                if (bakeTime > StartTime)
                {
                    TempTarget = LimitTemp;
                    PressTarget = LimitPressure;
                }
                #endregion
                #region Process2 Target value

                if ((bakeTime2 + bakeTime) > StartTime && bakeTime < StartTime)
                {
                    TempTarget = LimitTemp2;
                    PressTarget = LimitPressure2;
                }
                #endregion

                //Pressure
                startAddress = fc.Pressure;
                ushort[] register_Pressure = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Pressure {1}十進位：{0}{1}十六進位：{2} {1}", register_Pressure[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Pressure[0].ToString()), 16)));
                #region Process1 check control limit(Pressure)

                if (bakeTime > StartTime)
                {
                    if (Convert.ToDouble(register_Pressure[0].ToString()) >= LimitPressure) { reachPress = true; }
                    //Pressure正負0.5kg
                    if (Convert.ToDouble(register_Pressure[0].ToString()) > (LimitPressure + control_Limit_Press))
                    {
                        mail(maillist, string.Format(@"The Pressure of oven exceeds the spec"));
                        isOverSpec = "Y";                   
                    }
                    else if (reachPress == true && Convert.ToDouble(register_Pressure[0].ToString()) < (LimitPressure - control_Limit_Press))
                    {
                        mail(maillist, string.Format(@"The Pressure of oven not reach the spec"));
                        isOverSpec = "Y";
                    }
                    else
                    {
                        isOverSpec = "N";
                    }

                    //add query into sql transaction
                    foreach (string bc in arrBatch)
                    {                        
                        if (!string.IsNullOrEmpty(bc))
                        {
                            transaction_str1 = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO,Data,target,control_limit,isOverSpec)
                                                               Values(oven_assy_log_sequence.nextval,'{0}','1',SYSTO_DATE ('{1}', 'YYYY/MM/DD hh24:mi:ss')DATE,'{2}','{3}','{4}','{5}','{6}')",
                                                                     Machine_ID,
                                                                     DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                                     bc,
                                                                     register_Pressure[0].ToString(),
                                                                     PressTarget,
                                                                     control_Limit_Press,
                                                                     isOverSpec);
                            arrStr.Add(transaction_str1);
                        }
                    }
                }
                #endregion
                #region Process2 check control limit(Pressure)

                if ((bakeTime2 + bakeTime) > StartTime && bakeTime < StartTime)
                {
                    if (Convert.ToDouble(register_Pressure[0].ToString()) >= LimitPressure2) { reachPress2 = true; }
                    //Pressure正負0.5kg
                    if (Convert.ToDouble(register_Pressure[0].ToString()) > (LimitPressure2 + control_Limit_Press))
                    {
                        mail(maillist, string.Format(@"The Pressure of oven exceeds the spec"));
                        isOverSpec = "Y";
                    }
                    else if (reachPress2 == true && Convert.ToDouble(register_Pressure[0].ToString()) < (LimitPressure2 - control_Limit_Press))
                    {
                        mail(maillist, string.Format(@"The Pressure of oven not reach the spec"));
                        isOverSpec = "Y";
                    }
                    else
                    {
                        isOverSpec = "N";
                    }

                    //add query into sql transaction
                    foreach (string bc in arrBatch)
                    {                        
                        if (!string.IsNullOrEmpty(bc))
                        {
                            transaction_str1 = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO,Data,Target,control_Limit,isOverSpec)
                                                                   Values(oven_assy_log_sequence.nextval,'{0}','1',TO_DATE ('{1}', 'YYYY/MM/DD hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}')", 
                                                                         Machine_ID,
                                                                         DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                                         bc,
                                                                         register_Pressure[0].ToString(),
                                                                         PressTarget,
                                                                         control_Limit_Press,
                                                                         isOverSpec);
                            arrStr.Add(transaction_str1);
                        }
                    }
                }
                #endregion

                //CH1
                startAddress = fc.CH1;
                ushort[] register_Ch1 = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH1 {1}十進位：{0}{1}十六進位：{2} {1}", register_Ch1[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Ch1[0].ToString()), 16)));
                #region Process1 check control limit(CH1)

                if (bakeTime > StartTime)
                {
                    if (Convert.ToDouble(register_Ch1[0].ToString()) >= LimitTemp) { reachTemp = true; }

                    //溫度正負五度
                    if (Convert.ToDouble(register_Ch1[0].ToString()) > (LimitTemp + control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH1) of oven exceeds the spec"));
                        isOverSpec = "Y";
                    }
                    else if (reachTemp == true && Convert.ToDouble(register_Ch1[0].ToString()) < (LimitTemp - control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH1) of oven not reach the spec"));
                        isOverSpec = "Y";
                    }
                    else
                    {
                        isOverSpec = "N";
                    }

                    //add query into sql transaction
                    foreach (string bc in arrBatch)
                    {                        
                        if (!string.IsNullOrEmpty(bc))
                        {
                            transaction_str2 = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO,Data,Target,control_Limit,isOverSpec)
                                                               Values(oven_assy_log_sequence.nextval,'{0}','2',TO_DATE ('{1}', 'YYYY/MM/DD hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}')",
                                                                     Machine_ID,
                                                                     DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                                     bc,
                                                                     register_Ch1[0].ToString(),
                                                                     TempTarget,
                                                                     control_Limit_Temp,
                                                                     isOverSpec);
                            arrStr.Add(transaction_str2);
                        }
                    }
                }
                #endregion
                #region Process2 check control limit(CH1)

                if ((bakeTime2 + bakeTime) > StartTime && bakeTime < StartTime)
                {
                    if (Convert.ToDouble(register_Ch1[0].ToString()) >= LimitTemp2) { reachTemp2 = true; }
                    //溫度正負五度
                    if (Convert.ToDouble(register_Ch1[0].ToString()) > (LimitTemp2 + control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH1) of oven exceeds the spec"));
                        isOverSpec = "Y";
                    }
                    else if (reachTemp2 == true && Convert.ToDouble(register_Ch1[0].ToString()) < (LimitTemp2 - control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH1) of oven not reach the spec"));
                        isOverSpec = "Y";
                    }
                    else
                    {
                        isOverSpec = "N";
                    }

                    //add query into sql transaction
                    foreach (string bc in arrBatch)
                    {
                        if (!string.IsNullOrEmpty(bc))
                        {
                            transaction_str2 = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO,Data,Target,control_Limit,isOverSpec)
                                                               Values(oven_assy_log_sequence.nextval,'{0}','2',TO_DATE ('{1}', 'YYYY/MM/DD hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}')", 
                                                                     Machine_ID,
                                                                     DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                                     bc,
                                                                     register_Ch1[0].ToString(),
                                                                     TempTarget,
                                                                     control_Limit_Temp,
                                                                     isOverSpec);
                            arrStr.Add(transaction_str2);
                        }
                    }
                }
                #endregion

                //CH2 
                startAddress = fc.CH2;
                ushort[] register_Ch2 = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH2 {1}十進位：{0}{1}十六進位：{2} {1}", register_Ch2[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Ch2[0].ToString()), 16)));
                #region Process1 check control limit(CH2)

                if (bakeTime > StartTime)
                {
                    if (Convert.ToDouble(register_Ch2[0].ToString()) >= LimitTemp) { reachTemp = true; }

                    //溫度正負五度
                    if (Convert.ToDouble(register_Ch2[0].ToString()) > (LimitTemp + control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH2) of oven exceeds the spec"));
                        isOverSpec = "Y";
                    }
                    else if (reachTemp == true && Convert.ToDouble(register_Ch2[0].ToString()) < (LimitTemp - control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH2) of oven not reach the spec"));
                        isOverSpec = "Y";
                    }
                    else
                    {
                        isOverSpec = "N";
                    }

                    //add query into sql transaction
                    foreach (string bc in arrBatch)
                    {
                        if (!string.IsNullOrEmpty(bc))
                        {                            
                            transaction_str3 = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO,Data,Target,control_Limit,isOverSpec)
                                                                Values(oven_assy_log_sequence.nextval,'{0}','3',TO_DATE ('{1}', 'YYYY/MM/DD hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}')", 
                                                                      Machine_ID,
                                                                      DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                                      bc,
                                                                      register_Ch2[0].ToString(),
                                                                      TempTarget,
                                                                      control_Limit_Temp,
                                                                      isOverSpec);
                            arrStr.Add(transaction_str3);
                        }
                    }
                }
                #endregion
                #region Process2 check control limit(CH2)

                if ((bakeTime2 + bakeTime) > StartTime && bakeTime < StartTime)
                {
                    if (Convert.ToDouble(register_Ch2[0].ToString()) >= LimitTemp2) { reachTemp2 = true; }
                    //溫度正負五度
                    if (Convert.ToDouble(register_Ch2[0].ToString()) > (LimitTemp2 + control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH2) of oven exceeds the spec"));
                        isOverSpec = "Y";
                    }
                    else if (reachTemp2 == true && Convert.ToDouble(register_Ch2[0].ToString()) < (LimitTemp2 - control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH2) of oven not reach the spec"));
                        isOverSpec = "Y";
                    }
                    else
                    {
                        isOverSpec = "N";
                    }

                    //add query into sql transaction
                    foreach (string bc in arrBatch)
                    {
                        if (!string.IsNullOrEmpty(bc))
                        {                            
                            transaction_str3 = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO,Data,Target,control_Limit,isOverSpec)
                                                               Values(oven_assy_log_sequence.nextval,'{0}','3',TO_DATE ('{1}', 'YYYY/MM/DD hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}')", 
                                                                     Machine_ID,
                                                                     DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                                     bc,
                                                                     register_Ch2[0].ToString(),
                                                                     TempTarget,
                                                                     control_Limit_Temp,
                                                                     isOverSpec);
                            arrStr.Add(transaction_str3);
                        }
                    }
                }
                #endregion

                //最可靠溫度Ch3
                startAddress = fc.Temperature;
                ushort[] register_Temp = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH3 {1}十進位：{0}{1}十六進位：{2}{1}", register_Temp[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Temp[0].ToString()), 16)));
                #region Process1 check control limit(CH3)

                if (bakeTime > StartTime)
                {
                    if (Convert.ToDouble(register_Temp[0].ToString()) >= LimitTemp) { reachTemp = true; }

                    //溫度正負五度
                    if (Convert.ToDouble(register_Temp[0].ToString()) > (LimitTemp + control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH3) of oven exceeds the spec"));
                        isOverSpec = "Y";
                    }
                    else if (reachTemp == true && Convert.ToDouble(register_Temp[0].ToString()) < (LimitTemp - control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH3) of oven not reach the spec"));
                        isOverSpec = "Y";
                    }
                    else
                    {
                        isOverSpec = "N";
                    }

                    //add query into sql transaction
                    foreach (string bc in arrBatch)
                    {
                        if (!string.IsNullOrEmpty(bc))
                        {                            
                            transaction_str4 = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO,Data,Target,control_Limit,isOverSpec)
                                                               Values(oven_assy_log_sequence.nextval,'{0}','4',TO_DATE ('{1}', 'YYYY/MM/DD hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}')", 
                                                                     Machine_ID,
                                                                     DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                                     bc,
                                                                     register_Temp[0].ToString(),
                                                                     TempTarget,
                                                                     control_Limit_Temp,
                                                                     isOverSpec);
                            arrStr.Add(transaction_str4);
                        }
                    }
                }
                #endregion
                #region Process2 check control limit(CH3)

                if ((bakeTime2 + bakeTime) > StartTime && bakeTime < StartTime)
                {
                    if (Convert.ToDouble(register_Temp[0].ToString()) >= LimitTemp2) { reachTemp2 = true; }
                    //溫度正負五度
                    if (Convert.ToDouble(register_Temp[0].ToString()) > (LimitTemp2 + control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH3) of oven exceeds the spec"));
                        isOverSpec = "Y";
                    }
                    else if (reachTemp2 == true && Convert.ToDouble(register_Temp[0].ToString()) < (LimitTemp2 - control_Limit_Temp))
                    {
                        mail(maillist, string.Format(@"The Temperature(CH3) of oven not reach the spec"));
                        isOverSpec = "Y";
                    }
                    else
                    {
                        isOverSpec = "N";                       
                    }

                    //add query into sql transaction
                    foreach (string bc in arrBatch)
                    {
                        if (!string.IsNullOrEmpty(bc))
                        {
                            transaction_str4 = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO,Data,Target,control_Limit,isOverSpec)
                                                               Values(oven_assy_log_sequence.nextval,'{0}','4',TO_DATE ('{1}', 'YYYY/MM/DD hh24:mi:ss'),'{2}','{3}','{4}','{5}','{6}')", 
                                                                     Machine_ID,
                                                                     DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                                                     bc,
                                                                     register_Temp[0].ToString(),
                                                                     TempTarget,
                                                                     control_Limit_Temp,
                                                                     isOverSpec);
                            arrStr.Add(transaction_str4);
                        }
                    }
                }
                #endregion

                //commit
                string reStr = ado.SQL_transaction(arrStr, Conn);
                if (reStr.ToUpper().Contains("SUCCESS"))
                {
                    Console.WriteLine(string.Format(@"{0} data insert sussess.", DateTime.Now.ToString()));
                }
                else
                    Console.WriteLine
                        (string.Format(@"{0} data insert error.", DateTime.Now.ToString()));

                
                //==============================================================
                //error code check for machine monitor(Uniga offer),
                //and check the status of the oven(fc.Terminate),
                //if recive terminate signal stop timer
                //==============================================================
                App_Code.ErrorCode ec = new App_Code.ErrorCode();
                List<ushort[]> lstScan = new List<ushort[]>
                {
                    new ushort[]{ec.error616,616}, new ushort[]{ec.error617,617}, new ushort[]{ec.error618,618}, new ushort[]{ec.error619,619}, new ushort[]{ec.error620,620},
                    new ushort[]{ec.error621,621}, new ushort[]{ec.error622,622}, new ushort[]{ec.error623,623}, new ushort[]{ec.error624,624}, new ushort[]{ec.error625,625},
                    new ushort[]{ec.error626,626}, new ushort[]{ec.error627,627}, new ushort[]{ec.error628,628}, new ushort[]{ec.error629,629}, new ushort[]{ec.error630,630},
                    new ushort[]{ec.error631,631}, new ushort[]{ec.error632,632}, new ushort[]{ec.error633,633}, new ushort[]{ec.error634,634}, new ushort[]{ec.error635,635},
                    new ushort[]{ec.error636,636}, new ushort[]{ec.error638,638}, new ushort[]{ec.error639,639}, new ushort[]{ec.error640,640}, new ushort[]{ec.error641,641},
                    new ushort[]{ec.error642,642}, new ushort[]{ec.error643,643}, new ushort[]{ec.error644,644}, new ushort[]{ec.error645,645}, new ushort[]{ec.error646,646},
                    new ushort[]{ec.error647,647}, new ushort[]{ec.error648,648}, new ushort[]{ec.error649,649}, new ushort[]{ec.error650,650}, new ushort[]{ec.error651,651},
                    new ushort[]{ec.error652,652}, new ushort[]{ec.error653,653}, new ushort[]{ec.error654,654}, new ushort[]{ec.error655,655}, new ushort[]{ec.error656,656},
                    new ushort[]{ec.error657,657}, new ushort[]{ec.error658,658}, new ushort[]{ec.error659,659}, new ushort[]{ec.error660,660}
                    //,new ushort[]{fc.Terminate,0}
                };

                List<string> stopStr = new List<string>();
                string transaction_error = "", transaction_stop = "";
                bool timerStop = false;
                foreach (ushort[] scan in lstScan)
                {                    
                    startAddress = scan[0];
                    bool[] register_ErrorCode = master.ReadCoils(slaveID, startAddress, numOfPoints);
                    if (register_ErrorCode[0] == true)
                    {
                        timerStop = true;
                        foreach (string bc in arrBatch)
                        {
                            if (!string.IsNullOrEmpty(bc))
                            {
                                transaction_error = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO)
                                                                    Values(oven_assy_log_sequence.nextval,'{0}','{1}',SYSDATE,'{2}')", Machine_ID,
                                                                                                                                       scan[1].ToString(),
                                                                                                                                       bc);
                                stopStr.Add(transaction_error);

//                                transaction_stop = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO)
//                                                                   Values(oven_assy_log_sequence.nextval,'{0}','999',SYSDATE,'{1}')", Machine_ID, bc);
//                                stopStr.Add(transaction_stop);
                            }
                        }
                        
                        string stopResult = ado.SQL_transaction(stopStr, Conn);
                        if (stopResult.ToUpper().Contains("SUCCESS"))
                        {
                            Console.WriteLine(string.Format(@"{0} Monitor stop process sussess.", DateTime.Now.ToString()));
                        }
                        else
                            Console.WriteLine(string.Format(@"{0} Monitor stop process error.", DateTime.Now.ToString()));
                        break;
                    }
                }

                //無法自動停timer, 用時間來強制停止
                if (StartTime >= ScanTime)
                {
                    timerStop = true;
                    foreach (string bc in arrBatch)
                    {
                        if (!string.IsNullOrEmpty(bc))
                        {
                            string str_stop = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Batch_NO)
                                                              Values(oven_assy_log_sequence.nextval,'{0}','0',SYSDATE,'{1}')", Machine_ID, bc);
                            ado.dbNonQuery(str_stop, null);
                        }
                    }
                }

                if (timerStop)
                {
                    _tmr.Stop();
                    Console.WriteLine("error: timer stop.");
                    Mode(machineMode.close);
                }
                
            }
            catch (Exception exception)
            {
                #region error code

                //Connection exception
                //No response from server.
                //The server maybe close the com port, or response timeout.
                if (exception.Source.Equals("System"))
                    Console.WriteLine(exception.Message);

                //The server return error code.
                //You can get the function code and exception code.
                if (exception.Source.Equals("nModbusPC"))
                {
                    string str = exception.Message;
                    int FunctionCode;
                    string ExceptionCode;

                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("\r\n")));
                    Console.WriteLine("Function Code: " + FunctionCode.ToString("X"));

                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    ExceptionCode = str.Remove(str.IndexOf("-"));
                    switch (ExceptionCode.Trim())
                    {
                        case "1":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal function!");
                            break;
                        case "2":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data address!");
                            break;
                        case "3":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data value!");
                            break;
                        case "4":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Slave device failure!");
                            break;
                    }

                    /*
                        //Modbus exception codes definition
                            
                        * Code   * Name                                      * Meaning
                            01       ILLEGAL FUNCTION                            The function code received in the query is not an allowable action for the server.
                         
                            02       ILLEGAL DATA ADDRESS                        The data addrdss received in the query is not an allowable address for the server.
                         
                            03       ILLEGAL DATA VALUE                          A value contained in the query data field is not an allowable value for the server.
                           
                            04       SLAVE DEVICE FAILURE                        An unrecoverable error occurred while the server attempting to perform the requested action.
                             
                            05       ACKNOWLEDGE                                 This response is returned to prevent a timeout error from occurring in the client (or master)
                                                                                when the server (or slave) needs a long duration of time to process accepted request.
                          
                            06       SLAVE DEVICE BUSY                           The server (or slave) is engaged in processing a long–duration program command , and the
                                                                                client (or master) should retransmit the message later when the server (or slave) is free.
                             
                            08       MEMORY PARITY ERROR                         The server (or slave) attempted to read record file, but detected a parity error in the memory.
                             
                            0A       GATEWAY PATH UNAVAILABLE                    The gateway is misconfigured or overloaded.
                             
                            0B       GATEWAY TARGET DEVICE FAILED TO RESPOND     No response was obtained from the target device. Usually means that the device is not present on the network.                         
                        */
                }
                #endregion
            }//end catch
        }

        public enum machineMode : int { open = 0, close = 1 }
        static void Mode(machineMode _mode)
        {
            try
            {
                App_Code.FunctionCode fc = new App_Code.FunctionCode();
                switch (_mode.ToString())
                {
                    case "open":                        
                        startAddress = fc.OnBtnTwinkle;
                        master.WriteSingleRegister(slaveID, (ushort)startAddress, (ushort)Convert.ToInt32("1"));//write
                        ushort[] register_read = master.ReadHoldingRegisters(slaveID, (ushort)startAddress, numOfPoints);//read
                        Console.WriteLine(string.Format(@"Start Monitor 十進位：{0}{1}十六進位：{2}", register_read[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_read[0].ToString()), 16)));
                        break;
                    case "close":
                        //startAddress = fc.StopMachine;
                        //master.WriteSingleRegister(slaveID, (ushort)startAddress, (ushort)Convert.ToInt32("1"));//write
                        //ushort[] register_close = master.ReadHoldingRegisters(slaveID, (ushort)startAddress, numOfPoints);//read
                        //Console.WriteLine(string.Format(@"Stop Monitor 十進位：{0}{1}十六進位：{2}", register_close[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_close[0].ToString()), 16)));

                        serialPort.Close();
                        Environment.Exit(0); 
                        break;
                }
            }
            catch (Exception exception)
            {
                serialPort.Close();

                //Connection exception
                //No response from server.
                //The server maybe close the com port, or response timeout.
                if (exception.Source.Equals("System"))
                    Console.WriteLine(exception.Message);

                //The server return error code.
                //You can get the function code and exception code.
                if (exception.Source.Equals("nModbusPC"))
                {
                    string str = exception.Message;
                    int FunctionCode;
                    string ExceptionCode;

                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("\r\n")));
                    Console.WriteLine("Function Code: " + FunctionCode.ToString("X"));

                    str = str.Remove(0, str.IndexOf("\r\n") + 17);
                    ExceptionCode = str.Remove(str.IndexOf("-"));
                    switch (ExceptionCode.Trim())
                    {
                        case "1":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal function!");
                            break;
                        case "2":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data address!");
                            break;
                        case "3":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Illegal data value!");
                            break;
                        case "4":
                            Console.WriteLine("Exception Code: " + ExceptionCode.Trim() + "----> Slave device failure!");
                            break;
                    }

                    /*
                       //Modbus exception codes definition
                            
                       * Code   * Name                                      * Meaning
                         01       ILLEGAL FUNCTION                            The function code received in the query is not an allowable action for the server.
                         
                         02       ILLEGAL DATA ADDRESS                        The data addrdss received in the query is not an allowable address for the server.
                         
                         03       ILLEGAL DATA VALUE                          A value contained in the query data field is not an allowable value for the server.
                           
                         04       SLAVE DEVICE FAILURE                        An unrecoverable error occurred while the server attempting to perform the requested action.
                             
                         05       ACKNOWLEDGE                                 This response is returned to prevent a timeout error from occurring in the client (or master)
                                                                              when the server (or slave) needs a long duration of time to process accepted request.
                          
                         06       SLAVE DEVICE BUSY                           The server (or slave) is engaged in processing a long–duration program command , and the
                                                                              client (or master) should retransmit the message later when the server (or slave) is free.
                             
                         08       MEMORY PARITY ERROR                         The server (or slave) attempted to read record file, but detected a parity error in the memory.
                             
                         0A       GATEWAY PATH UNAVAILABLE                    The gateway is misconfigured or overloaded.
                             
                         0B       GATEWAY TARGET DEVICE FAILED TO RESPOND     No response was obtained from the target device. Usually means that the device is not present on the network.                         
                     */
                }
            }
        }

        static void mail(string contact, string mail_data)
        {
            string title = "Pressure Oven System Fail";
            using (OracleConnection connection = new OracleConnection(ovenWin.Properties.Settings.Default.MAIL))
            {
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                OracleTransaction transaction;
                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Transaction = transaction;
                try
                {
                    command.CommandText = "select seq_mail_id.nextval from dual";
                    int mail_id = Convert.ToInt32(command.ExecuteScalar());
                    command.CommandText = "insert into mail_pool (id,from_name,disable,datetime_in,send_period,mail_to,mail_cc,mail_subject,datetime_exp,exclusive_flag,check_sum,html_body) " +
                    "values (" + mail_id + ",'Pressure Oven System mail agent',0,sysdate,0,'" + contact + "',null,'" + title + "',sysdate+1,0,null,1)";
                    command.ExecuteNonQuery();

                    command.CommandText = "insert into mail_body (id,sn,mail_cont) values (" + mail_id + ",1,'" + mail_data + "')";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    //msg.Text = ex.ToString();
                }
            }
        }


    }
}

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
        static Timer timer;
        static string Machine_ID;
        //static private int TimeInterval = 1000*60, StartTime = 0; //old
        //static private double ScanTime, LimitTemp, LimitPressure; //old
        static private int TimeInterval = 1000 * 60, StartTime = 0;//new 
        static private double ScanTime,bakeTime, LimitTemp, LimitPressure, bakeTime2, LimitTemp2, LimitPressure2;//new
        static private bool reachTemp = false, reachPress = false, reachTemp2 = false, reachPress2 = false;//new 
        

        static void Main(string[] args)
        {
            // create a new SerialPort object with default settings.
            serialPort = new SerialPort();

            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;

            //args = new string[] { "COM10", "OV-135", "1-0-2-110-7|2-0-2-126-7" }; //LOCAL TEST
            if (args != null && args.Length > 0)
            {
                //args[0] -> COM
                //args[1] -> Machine_ID
                //args[2] -> PROCESS-1.2.3... (Process1-Hour1-Min1-Temp1-Pressure1|Process2-Hour2-Min2-Temp2-Pressure2)
                
                // set the appropriate properties.
                serialPort.PortName = args[0].ToString();   
                Machine_ID = args[1].ToString();
                
                string[] arrPara = args[2].ToString().Split('|');
                bakeTime = Convert.ToInt32(arrPara[0].Split('-')[2]) * 60 * 1000;
                LimitTemp = Convert.ToInt32(arrPara[0].Split('-')[3]);
                LimitPressure = Convert.ToInt32(arrPara[0].Split('-')[4]);
                Console.WriteLine("bakeTime" + bakeTime + Environment.NewLine);
                Console.WriteLine("LimitTemp" + LimitTemp + Environment.NewLine);
                Console.WriteLine("LimitPressure: "+ LimitPressure + Environment.NewLine);  
                if (arrPara.Length > 1) 
                {
                    bakeTime2 = Convert.ToInt32(arrPara[1].Split('-')[2]) * 60 * 1000;
                    LimitTemp2 = Convert.ToInt32(arrPara[1].Split('-')[3]);
                    LimitPressure2 = Convert.ToInt32(arrPara[1].Split('-')[4]);
                    Console.WriteLine("bakeTime2" + bakeTime2 + Environment.NewLine);
                    Console.WriteLine("LimitTemp2" + LimitTemp2 + Environment.NewLine);
                    Console.WriteLine("LimitPressure2: " + LimitPressure2 + Environment.NewLine); 
                }
                ScanTime = bakeTime + bakeTime2;

                Console.WriteLine("COM: " + args[0].ToString() + Environment.NewLine);
                Console.WriteLine("MachineID: " + args[1].ToString() + Environment.NewLine);
                Console.WriteLine("args[2]: " + args[2].ToString() + Environment.NewLine);                
                Console.WriteLine("ScanTime: " + ScanTime + Environment.NewLine);

                serialPort.Open();
                // create Modbus RTU Master by the comport client
                //document->Modbus.Device.Namespace->ModbusSerialMaster Class->CreateRtu Method
                master = ModbusSerialMaster.CreateRtu(serialPort);                
                master.Transport.ReadTimeout = 300;

                timer = new Timer();
                timer.Interval = TimeInterval; //set interval of checking here
                timer.Elapsed += delegate
                {
                    //timer_Elapsed(); //old 
                    timer_Elapsed();//new 
                };
                
                Mode(machineMode.open);// active oven power
                timer.Start();
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine(string.Format(@"No args[]. {0}.", DateTime.Now.ToString()));
                Console.ReadKey(true);
            }
        }//new
        static void timer_Elapsed()//new 
        {
            Console.WriteLine(string.Format(@"{0}.",DateTime.Now.ToString()));
            StartTime += TimeInterval;

            try
            {
                //byte slaveID = 1;
                //ushort startAddress;                
                //ushort numOfPoints = 8;
                //master.Transport.ReadTimeout = 300;

                string Conn = ovenWin.Properties.Settings.Default.Oven;
                App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
                App_Code.FunctionCode fc = new App_Code.FunctionCode();

                List<string> arrStr = new List<string>();
                string transaction_str1 = "", transaction_str2 = "", transaction_str3 = "", transaction_str4 = "";

                //Pressure
                startAddress = fc.Pressure;
                ushort[] register_Pressure = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Pressure {1}十進位：{0}{1}十六進位：{2} {1}", register_Pressure[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Pressure[0].ToString()), 16)));
                transaction_str1 = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Data)
                                                   values(oven_assy_log_sequence.nextval,'{0}','1',SYSDATE,'{1}')", Machine_ID, register_Pressure[0].ToString());
                arrStr.Add(transaction_str1);

                //CH1
                startAddress = fc.CH1;
                ushort[] register_Ch1 = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH1 {1}十進位：{0}{1}十六進位：{2} {1}", register_Ch1[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Ch1[0].ToString()), 16)));
                transaction_str2 = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Data)
                                                   values(oven_assy_log_sequence.nextval,'{0}','2',SYSDATE,'{1}')", Machine_ID, register_Ch1[0].ToString());
                arrStr.Add(transaction_str2);

                //CH2 
                startAddress = fc.CH2;
                ushort[] register_Ch2 = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH2 {1}十進位：{0}{1}十六進位：{2} {1}", register_Ch2[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Ch2[0].ToString()), 16)));
                transaction_str3 = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Data)
                                                   values(oven_assy_log_sequence.nextval,'{0}','3',SYSDATE,'{1}')", Machine_ID, register_Ch2[0].ToString());
                arrStr.Add(transaction_str3);

                //最可靠溫度Ch3
                startAddress = fc.Temperature;
                ushort[] register_Temp = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH3 {1}十進位：{0}{1}十六進位：{2}{1}", register_Temp[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Temp[0].ToString()), 16)));
                transaction_str4 = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Data)
                                                   values(oven_assy_log_sequence.nextval,'{0}','4',SYSDATE,'{1}')", Machine_ID, register_Temp[0].ToString());
                arrStr.Add(transaction_str4);

                string reStr = ado.SQL_transaction(arrStr, Conn);
                if (reStr.ToUpper().Contains("SUCCESS"))
                {
                    Console.WriteLine(string.Format(@"{0} data insert sussess.", DateTime.Now.ToString()));
                }
                else
                    Console.WriteLine(string.Format(@"{0} data insert error.", DateTime.Now.ToString()));

                #region warring letter
                //==============================================================
                //Process1
                //==============================================================
                if (bakeTime > StartTime)
                {
                    if (Convert.ToDouble(register_Temp[0].ToString()) >= LimitTemp) { reachTemp = true; }
                    if (Convert.ToDouble(register_Pressure[0].ToString()) >= LimitPressure) { reachPress = true; }

                    //溫度正負五度
                    if (Convert.ToDouble(register_Temp[0].ToString()) > (LimitTemp + 5)) { mail("Alan.Kuo@nxp.com", string.Format(@"The Temperature of oven exceeds the spec")); }
                    if (reachTemp == true && Convert.ToDouble(register_Temp[0].ToString()) < (LimitTemp - 5)) { mail("Alan@Kuo@nxp.com", string.Format(@"The Temperature of oven not reach the spec")); }

                    //Pressure正負0.5kg
                    if (Convert.ToDouble(register_Pressure[0].ToString()) > (LimitPressure + 0.5)) { mail("Alan.Kuo@nxp.com", string.Format(@"The Pressure of oven exceeds the spec")); }
                    if (reachPress == true && Convert.ToDouble(register_Pressure[0].ToString()) < (LimitPressure - 0.5)) { mail("Alan@Kuo@nxp.com", string.Format(@"The Pressure of oven not reach the spec")); }
                }

                //==============================================================
                //Process2
                //==============================================================
                if (bakeTime2 > StartTime && bakeTime < StartTime)
                {
                    if (Convert.ToDouble(register_Temp[0].ToString()) >= LimitTemp2) { reachTemp2 = true; }
                    if (Convert.ToDouble(register_Pressure[0].ToString()) >= LimitPressure2) { reachPress2 = true; }

                    //溫度正負五度
                    if (Convert.ToDouble(register_Temp[0].ToString()) > (LimitTemp2 + 5)) { mail("Alan.Kuo@nxp.com", string.Format(@"The Temperature of oven exceeds the spec")); }
                    if (reachTemp2 == true && Convert.ToDouble(register_Temp[0].ToString()) < (LimitTemp2 - 5)) { mail("Alan.Kuo@nxp.com", string.Format(@"The Temperature of oven not reach the spec")); }

                    //Pressure正負0.5kg
                    if (Convert.ToDouble(register_Pressure[0].ToString()) > (LimitPressure2 + 0.5)) { mail("Alan.Kuo@nxp.com", string.Format(@"The Pressure of oven exceeds the spec")); }
                    if (reachPress2 == true && Convert.ToDouble(register_Pressure[0].ToString()) < (LimitPressure2 - 0.5)) { mail("Alan.Kuo@nxp.com", string.Format(@"The Pressure of oven not reach the spec")); }
                }
                #endregion


                if (ScanTime < StartTime)
                {
                    //end bake log 
                    string endLogstr = string.Format(@"Insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time)
                                                       Values(oven_assy_log_sequence.nextval,'{0}','0',SYSDATE)", Machine_ID);
                    ado.dbNonQuery(endLogstr, null);

                    Mode(machineMode.close);
                    timer.Stop();
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

        static void Main2(string[] args)
        {
            //args[0] -> COM
            //args[1] -> Machine_ID
            //args[2] -> Temperature Limit
            //args[3] -> Pressure Limit
            //args[4] -> Total Time

            // create a new SerialPort object with default settings.
            serialPort = new SerialPort();

            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.DataBits = 8;


            if (args != null && args.Length > 0)
            {
                // set the appropriate properties.
                serialPort.PortName = args[0].ToString();
                Machine_ID = args[1].ToString();
                LimitTemp = Convert.ToInt32(args[2].ToString());
                LimitPressure = Convert.ToInt32(args[3].ToString());
                ScanTime = Convert.ToInt32(args[4].ToString()) * 60 * 1000;

                Console.WriteLine(args[0].ToString() + Environment.NewLine);
                Console.WriteLine(args[1].ToString() + Environment.NewLine);
                Console.WriteLine(args[2].ToString() + Environment.NewLine);
                Console.WriteLine(args[3].ToString() + Environment.NewLine);
                Console.WriteLine(ScanTime + Environment.NewLine);

                serialPort.Open();
                // create Modbus RTU Master by the comport client
                //document->Modbus.Device.Namespace->ModbusSerialMaster Class->CreateRtu Method
                master = ModbusSerialMaster.CreateRtu(serialPort);
                master.Transport.ReadTimeout = 300;

                timer = new Timer();
                timer.Interval = TimeInterval; //set interval of checking here
                timer.Elapsed += delegate
                {
                    timer_Elapsed2();
                };

                Mode(machineMode.open);// active oven power
                timer.Start();
                Console.ReadKey(true);
            }
            else
            {
                Console.WriteLine(string.Format(@"No args[]. {0}.", DateTime.Now.ToString()));
                Console.ReadKey(true);
            }
        }//old
        static void timer_Elapsed2()
        {
            Console.WriteLine(string.Format(@"{0}.", DateTime.Now.ToString()));

            StartTime += TimeInterval;
            
            try
            {
                //byte slaveID = 1;
                //ushort startAddress;                
                //ushort numOfPoints = 8;
                //master.Transport.ReadTimeout = 300;

                string Conn = ovenWin.Properties.Settings.Default.Oven;
                App_Code.AdoDbConn ado = new App_Code.AdoDbConn(App_Code.AdoDbConn.AdoDbType.Oracle, Conn);
                App_Code.FunctionCode fc = new App_Code.FunctionCode();

                List<string> arrStr = new List<string>();
                string transaction_str1 = "", transaction_str2 = "", transaction_str3 = "", transaction_str4 = "";

                //Pressure
                startAddress = fc.Pressure;
                ushort[] register_Pressure = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Pressure {1}十進位：{0}{1}十六進位：{2} {1}", register_Pressure[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Pressure[0].ToString()), 16)));
                transaction_str1 = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Data)
                                                   values(oven_assy_log_sequence.nextval,'{0}','1',SYSDATE,'{1}')", Machine_ID, register_Pressure[0].ToString());
                arrStr.Add(transaction_str1);

                //CH1
                startAddress = fc.CH1;
                ushort[] register_Ch1 = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH1 {1}十進位：{0}{1}十六進位：{2} {1}", register_Ch1[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Ch1[0].ToString()), 16)));
                transaction_str2 = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Data)
                                                   values(oven_assy_log_sequence.nextval,'{0}','2',SYSDATE,'{1}')", Machine_ID, register_Ch1[0].ToString());
                arrStr.Add(transaction_str2);

                //CH2 
                startAddress = fc.CH2;
                ushort[] register_Ch2 = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH2 {1}十進位：{0}{1}十六進位：{2} {1}", register_Ch2[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Ch2[0].ToString()), 16)));
                transaction_str3 = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Data)
                                                   values(oven_assy_log_sequence.nextval,'{0}','3',SYSDATE,'{1}')", Machine_ID, register_Ch2[0].ToString());
                arrStr.Add(transaction_str3);

                //最可靠溫度Ch3
                startAddress = fc.Temperature;
                ushort[] register_Temp = master.ReadHoldingRegisters(slaveID, startAddress, numOfPoints);
                Console.WriteLine(string.Format(@"Temperature CH3 {1}十進位：{0}{1}十六進位：{2}{1}", register_Temp[0].ToString(), Environment.NewLine, Convert.ToString(Convert.ToInt32(register_Temp[0].ToString()), 16)));
                transaction_str4 = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time,Data)
                                                   values(oven_assy_log_sequence.nextval,'{0}','4',SYSDATE,'{1}')", Machine_ID, register_Temp[0].ToString());
                arrStr.Add(transaction_str4);

                string reStr = ado.SQL_transaction(arrStr, Conn);
                if (reStr.ToUpper().Contains("SUCCESS"))
                {
                    Console.WriteLine(string.Format(@"{0} data insert sussess.", DateTime.Now.ToString()));
                }
                else
                    Console.WriteLine(string.Format(@"{0} data insert error.", DateTime.Now.ToString()));

                #region warring letter
                
                //溫度正負五度
                if (Convert.ToDouble(register_Temp[0].ToString()) > (LimitTemp + 5)) { mail("Alan@Kuo@nxp.com", string.Format(@"The Temperature of oven exceeds the spec")); }

                //Pressure正負0.5kg
                if (Convert.ToDouble(register_Pressure[0].ToString()) > (LimitPressure + 0.5)) { mail("Alan@Kuo@nxp.com", string.Format(@"The Pressure of oven exceeds the spec")); }
                
                #endregion


                if (ScanTime < StartTime) 
                {
                    //end bake log 
                    string endLogstr = string.Format(@"insert into oven_assy_log(oven_assy_logid,machine_ID,oven_Assy_logKindID,Time)
                                                       values(oven_assy_log_sequence.nextval,'{0}','0',SYSDATE)", Machine_ID);
                    ado.dbNonQuery(endLogstr, null);

                    Mode(machineMode.close);
                    timer.Stop();
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
        }//old

        public enum machineMode : int { open = 0, close = 1 }
        static void Mode(machineMode _mode)
        {
            try
            {
                App_Code.FunctionCode fc = new App_Code.FunctionCode();
                startAddress = fc.Working;

                switch (_mode.ToString())
                {
                    case "open":
                        master.WriteSingleCoil(slaveID, (ushort)startAddress, true);//write
                        bool[] Coil_status = master.ReadCoils(slaveID, startAddress, numOfPoints);
                        if (Coil_status[0] == true) { Console.WriteLine("The Oven is ON"); }
                        else { Console.WriteLine("The Oven is OFF"); }
                        break;
                    case "close":
                        master.WriteSingleCoil(slaveID, (ushort)startAddress, false);//write
                        bool[] Coil2_status = master.ReadCoils(slaveID, startAddress, numOfPoints);
                        if (Coil2_status[0] == true) { Console.WriteLine("The Oven is ON"); }
                        else { Console.WriteLine("The Oven is OFF"); }
                        serialPort.Close();
                        break;
                }
            }
            catch (Exception exception)
            {
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

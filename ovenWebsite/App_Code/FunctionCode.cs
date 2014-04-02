using System;
using System.Collections.Generic;
using System.Web;

namespace nModBusWeb.App_Code
{
    public class FunctionCode
    {
        //read
        #region 作業中-01 01 0A 91 00 01
        private ushort _Working = 2705;
        /// <summary>
        /// Read Working or not(func. 01)
        /// </summary>
        public ushort Working
        {
            set { _Working = value; }
            get { return _Working; }
        }
        #endregion

        #region 警報-01 01 0B AE 00 01
        private ushort _Alarm = 2990;
        /// <summary>
        /// Read Alarm or not(func. 01)
        /// </summary>
        public ushort Alarm
        {
            set { _Alarm = value; }
            get { return _Alarm; }
        }
        #endregion

        #region 作業結束-01 01 0A 9B 00 01
        private ushort _Terminate = 2715;
        /// <summary>
        /// Read Terminate or not(func 01)
        /// </summary>
        public ushort Terminate
        {
            set { _Terminate = value; }
            get { return _Terminate; }
        }
        #endregion

        #region 最可靠的溫度-01 03 09 CE 00 01
        private ushort _Temperature = 2510;
        /// <summary>
        /// Read Temperature(func. 03)
        /// </summary>
        public ushort Temperature
        {
            set { _Temperature = value; }
            get { return _Temperature; }
        }
        #endregion

        #region CH1-01 03 09 BC 00 01
        private ushort _CH1 = 2492;
        /// <summary>
        /// Read CH1(func. 03)
        /// </summary>
        public ushort CH1
        {
            set { _CH1 = value; }
            get { return _CH1; }
        }
        #endregion

        #region CH2-01 03 09 BE 00 01
        private ushort _CH2 = 2494;
        /// <summary>
        /// Read CH2(func. 03)
        /// </summary>
        public ushort CH2
        {
            set { _CH2 = value; }
            get { return _CH2; }
        }
        #endregion

        #region 壓力-01 03 0A E8 00 01
        private ushort _Pressure = 2792;
        /// <summary>
        /// Read Pressure(func. 03)
        /// </summary>
        public ushort Pressure
        {
            set { _Pressure = value; }
            get { return _Pressure; }
        }
        #endregion

        #region 第一台第一組步驟 01 03 1C 84 00 01
        private ushort _firstProcess = 7300;
        /// <summary>
        /// Read firstPrecess(func. 03)
        /// </summary>
        public ushort firstProcess
        {
            set { _firstProcess = value; }
            get { return _firstProcess; }
        }
        #endregion

        #region 第一台第一組時間(時) 01 03 1C 85 00 01
        private ushort _firstHour = 7301;
        /// <summary>
        /// Read firstHour(func. 03)
        /// </summary>
        public ushort firstHour
        {
            set { _firstHour = value; }
            get { return _firstHour; }
        }
        #endregion

        #region 第一台第一組時間(分) 01 03 1C 86 00 01
        private ushort _firstMin = 7302;
        /// <summary>
        /// Read firstMin(func. 03)
        /// </summary>
        public ushort firstMin
        {
            set { _firstMin = value; }
            get { return _firstMin; }
        }
        #endregion

        #region 第一台第一組溫度 01 03 1C 87 00 01
        private ushort _firstTemperature = 7303;
        /// <summary>
        /// Read firstTemperature(func. 03)
        /// </summary>
        public ushort firstTemperature
        {
            set { _firstTemperature = value; }
            get { return _firstTemperature; }
        }
        #endregion

        #region 第一台第一組壓力 01 03 1C 88 00 01
        private ushort _firstPressure = 7304;
        /// <summary>
        /// Read firstPressure(func. 03)
        /// </summary>
        public ushort firstPressure
        {
            set { _firstPressure = value; }
            get { return _firstPressure; }
        }
        #endregion

        #region 第一台第二組步驟 01 03 1C 89 00 01
        private ushort _secondProcess = 7305;
        /// <summary>
        /// Read secondPrecess(func. 03)
        /// </summary>
        public ushort secondProcess
        {
            set { _secondProcess = value; }
            get { return _secondProcess; }
        }
        #endregion

        #region 第一台第二組時間(時) 01 03 1C 8A 00 01
        private ushort _secondHour = 7306;
        /// <summary>
        /// Read secondHour(func. 03)
        /// </summary>
        public ushort secondHour
        {
            set { _secondHour = value; }
            get { return _secondHour; }
        }
        #endregion

        #region 第一台第二組時間(分) 01 03 1C 8B 00 01
        private ushort _secondMin = 7307;
        /// <summary>
        /// Read secondMin(func. 03)
        /// </summary>
        public ushort secondMin
        {
            set { _secondMin = value; }
            get { return _secondMin; }
        }
        #endregion

        #region 第一台第二組溫度 01 03 1C 8C 00 01
        private ushort _secondTemperature = 7308;
        /// <summary>
        /// Read secondTemperature(func. 03)
        /// </summary>
        public ushort secondTemperature
        {
            set { _secondTemperature = value; }
            get { return _secondTemperature; }
        }
        #endregion

        #region 第一台第二組壓力 01 03 1C 8D 00 01
        private ushort _secondPressure = 7309;
        /// <summary>
        /// Read secondPressure(func. 03)
        /// </summary>
        public ushort secondPressure
        {
            set { _secondPressure = value; }
            get { return _secondPressure; }
        }
        #endregion

        //write
        #region 選用爐訊號-01 06 0D 48 00 01
        private ushort _Furnace = 3400;
        /// <summary>
        /// Write Furance(func. 06)
        /// </summary>
        public ushort Furnace
        {
            set { _Furnace = value; }
            get { return _Furnace; }
        }
        #endregion

        #region 啟動按鈕enable並閃爍-01 06 0D 5C 00 01
        private ushort _OnBtnTwinkle = 3420;
        /// <summary>
        /// Write ON Button Twinkle(func. 06)
        /// </summary>
        public ushort OnBtnTwinkle
        {
            set { _OnBtnTwinkle = value; }
            get { return _OnBtnTwinkle; }
        }
        #endregion

        #region 一般模式紅燈OFF-01 06 0D 70 00 01
        private ushort _RedLightOff = 3440;
        /// <summary>
        /// Write Red Light Off(func. 06)
        /// </summary>
        public ushort RedLightOff
        {
            set { _RedLightOff = value; }
            get { return _RedLightOff; }
        }
        #endregion

        #region 警報蜂鳴器ON-01 06 0D 84 00 01
        private ushort _AlarmON = 3460;
        /// <summary>
        /// Write Alarm ON(func. 06)
        /// </summary>
        public ushort AlarmON
        {
            set { _AlarmON = value; }
            get { return _AlarmON; }
        }
        #endregion

        #region 強制機台停止訊號-01 06 0D 98 00 01
        private ushort _StopMachine = 3480;
        /// <summary>
        /// Write Stop Machine(func. 06)
        /// </summary>
        public ushort StopMachine
        {
            set { _StopMachine = value; }
            get { return _StopMachine; }
        }
        #endregion
    }
}

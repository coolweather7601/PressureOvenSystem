using System;
using System.Collections.Generic;
using System.Text;

namespace ovenWin.App_Code
{
    class FunctionCode
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

        //write
        #region 啟動使用(系統允許設備可被執行操作)-01 05 0D 5C 00 01
        private ushort _OnBtnTwinkle = 3420;
        /// <summary>
        /// Write ON Button Twinkle(func. 05)
        /// </summary>
        public ushort OnBtnTwinkle
        {
            set { _OnBtnTwinkle = value; }
            get { return _OnBtnTwinkle; }
        }
        #endregion

        #region 通訊模式切換-01 05 0D 70 00 01
        private ushort _RedLightOff = 3440;
        /// <summary>
        /// Write Red Light Off(func. 05)
        /// </summary>
        public ushort RedLightOff
        {
            set { _RedLightOff = value; }
            get { return _RedLightOff; }
        }
        #endregion

        #region 警報蜂鳴器ON-01 05 0D 84 00 01
        private ushort _AlarmON = 3460;
        /// <summary>
        /// Write Alarm ON(func. 05)
        /// </summary>
        public ushort AlarmON
        {
            set { _AlarmON = value; }
            get { return _AlarmON; }
        }
        #endregion

        #region 強制機台停止訊號-01 05 0D 98 00 01
        private ushort _StopMachine = 3480;
        /// <summary>
        /// Write Stop Machine(func. 05)
        /// </summary>
        public ushort StopMachine
        {
            set { _StopMachine = value; }
            get { return _StopMachine; }
        }
        #endregion
    }

    public class ErrorCode
    {
        #region 616_來源空壓壓力不足-01 01 0A F0 00 01
        private ushort _error616 = 2800;
        /// <summary>
        /// error code 616 來源空壓壓力不足
        /// </summary>
        public ushort error616
        {
            set { _error616 = value; }
            get { return _error616; }
        }
        #endregion

        #region 617_腔門沒有關閉無法啟動-01 01 0A F1 00 01
        private ushort _error617 = 2801;
        /// <summary>
        /// error code 617 腔門沒有關閉無法啟動
        /// </summary>
        public ushort error617
        {
            set { _error617 = value; }
            get { return _error617; }
        }
        #endregion

        #region 618_膠條加壓未開啟腔體無法加壓 -01 01 0A F2 00 01
        private ushort _error618 = 2802;
        /// <summary>
        /// error code 618 膠條加壓未開啟腔體無法加壓
        /// </summary>
        public ushort error618
        {
            set { _error618 = value; }
            get { return _error618; }
        }
        #endregion

        #region 619_加壓時安全栓未前定位 -01 01 0A F3 00 01
        private ushort _error619 = 2803;
        /// <summary>
        /// error code 619 加壓時安全栓未前定位
        /// </summary>
        public ushort error619
        {
            set { _error619 = value; }
            get { return _error619; }
        }
        #endregion

        #region 620_外部程控加熱超溫異常 -01 01 0A F4 00 01
        private ushort _error620 = 2804;
        /// <summary>
        /// error code 620 外部程控加熱超溫異常
        /// </summary>
        public ushort error620
        {
            set { _error620 = value; }
            get { return _error620; }
        }
        #endregion

        #region 621_循環風扇變頻器過載 -01 01 0A F5 00 01
        private ushort _error621 = 2805;
        /// <summary>
        /// error code 621 循環風扇變頻器過載
        /// </summary>
        public ushort error621
        {
            set { _error621 = value; }
            get { return _error621; }
        }
        #endregion

        #region 622_腔門旋轉沒有關閉無法啟動 -01 01 0A F6 00 01
        private ushort _error622 = 2806;
        /// <summary>
        /// error code 622 腔門旋轉沒有關閉無法啟動
        /// </summary>
        public ushort error622
        {
            set { _error622 = value; }
            get { return _error622; }
        }
        #endregion

        #region 623_腔體加壓壓力超壓--異常 -01 01 0A F7 00 01
        private ushort _error623 = 2807;
        /// <summary>
        /// error code 623 腔體加壓壓力超壓--異常
        /// </summary>
        public ushort error623
        {
            set { _error623 = value; }
            get { return _error623; }
        }
        #endregion

        #region 624_腔門加壓中腔門無法開啟 -01 01 0A F8 00 01
        private ushort _error624 = 2808;
        /// <summary>
        /// error code 624 腔門加壓中腔門無法開啟
        /// </summary>
        public ushort error624
        {
            set { _error624 = value; }
            get { return _error624; }
        }
        #endregion

        #region 625_電熱空燒EGO異常 -01 01 0A F9 00 01
        private ushort _error625 = 2809;
        /// <summary>
        /// error code 625 電熱空燒EGO異常
        /// </summary>
        public ushort error625
        {
            set { _error625 = value; }
            get { return _error625; }
        }
        #endregion

        #region 626_自動啟動腔門未關定位 -01 01 0A FA 00 01
        private ushort _error626 = 2810;
        /// <summary>
        /// error code 626 自動啟動腔門未關定位
        /// </summary>
        public ushort error626
        {
            set { _error626 = value; }
            get { return _error626; }
        }
        #endregion

        #region 627_冷卻水管道水壓超壓 -01 01 0A FB 00 01
        private ushort _error627 = 2811;
        /// <summary>
        /// error code 627 冷卻水管道水壓超壓
        /// </summary>
        public ushort error627
        {
            set { _error627 = value; }
            get { return _error627; }
        }
        #endregion

        #region 628_允許開門溫度未到達 -01 01 0A FC 00 01
        private ushort _error628 = 2812;
        /// <summary>
        /// error code 628 允許開門溫度未到達
        /// </summary>
        public ushort error628
        {
            set { _error628 = value; }
            get { return _error628; }
        }
        #endregion

        #region 629_允許開門壓力未到達 -01 01 0A FD 00 01
        private ushort _error629 = 2813;
        /// <summary>
        /// error code 629 允許開門壓力未到達
        /// </summary>
        public ushort error629
        {
            set { _error629 = value; }
            get { return _error629; }
        }
        #endregion

        #region 630_膠條加壓腔門未關定位 -01 01 0A FE 00 01
        private ushort _error630 = 2814;
        /// <summary>
        /// error code 630 膠條加壓腔門未關定位
        /// </summary>
        public ushort error630
        {
            set { _error630 = value; }
            get { return _error630; }
        }
        #endregion

        #region 631_數位超溫異常 -01 01 0A FF 00 01
        private ushort _error631 = 2815;
        /// <summary>
        /// error code 631 數位超溫異常
        /// </summary>
        public ushort error631
        {
            set { _error631 = value; }
            get { return _error631; }
        }
        #endregion

        #region 632_電熱SCR異常 -01 01 0B 00 00 01
        private ushort _error632 = 2816;
        /// <summary>
        /// error code 632 電熱SCR異常
        /// </summary>
        public ushort error632
        {
            set { _error632 = value; }
            get { return _error632; }
        }
        #endregion

        #region 633_關門異常 -01 01 0B 01 00 01
        private ushort _error633 = 2817;
        /// <summary>
        /// error code 633 關門異常
        /// </summary>
        public ushort error633
        {
            set { _error633 = value; }
            get { return _error633; }
        }
        #endregion

        #region 634_關門異常 -01 01 0B 02 00 01
        private ushort _error634 = 2818;
        /// <summary>
        /// error code 634 開門異常
        /// </summary>
        public ushort error634
        {
            set { _error634 = value; }
            get { return _error634; }
        }
        #endregion

        #region 635_急停開關動作 -01 01 0B 03 00 01
        private ushort _error635 = 2819;
        /// <summary>
        /// error code 635 急停開關動作
        /// </summary>
        public ushort error635
        {
            set { _error635 = value; }
            get { return _error635; }
        }
        #endregion

        #region 636_腔體外超溫異常 -01 01 0B 04 00 01
        private ushort _error636 = 2820;
        /// <summary>
        /// error code 636 腔體外超溫異常
        /// </summary>
        public ushort error636
        {
            set { _error636 = value; }
            get { return _error636; }
        }
        #endregion

        #region 638_循環風扇馬達運轉異常 -01 01 0B 06 00 01
        private ushort _error638 = 2822;
        /// <summary>
        /// error code 638 循環風扇馬達運轉異常
        /// </summary>
        public ushort error638
        {
            set { _error638 = value; }
            get { return _error638; }
        }
        #endregion

        #region 639_冷卻水流量不足 -01 01 0B 07 00 01
        private ushort _error639 = 2823;
        /// <summary>
        /// error code 639 冷卻水流量不足
        /// </summary>
        public ushort error639
        {
            set { _error639 = value; }
            get { return _error639; }
        }
        #endregion

        #region 640_膠條加壓開關未開 -01 01 0B 08 00 01
        private ushort _error640 = 2824;
        /// <summary>
        /// error code 640 膠條加壓開關未開
        /// </summary>
        public ushort error640
        {
            set { _error640 = value; }
            get { return _error640; }
        }
        #endregion

        #region 641_循環風扇馬達運轉過熱超溫 01 01 0B 09 00 01
        private ushort _error641 = 2825;
        /// <summary>
        /// error code 641 循環風扇馬達運轉過熱超溫
        /// </summary>
        public ushort error641
        {
            set { _error641 = value; }
            get { return _error641; }
        }
        #endregion

        #region 642_自動壓力超時未到	01 01 0B 0A 00 01
        private ushort _error642 = 2826;
        /// <summary>
        /// error code 642 自動壓力超時未到
        /// </summary>
        public ushort error642
        {
            set { _error642 = value; }
            get { return _error642; }
        }
        #endregion

        #region 643_自動溫度超時未到 01 01 0B 0B 00 01
        private ushort _error643 = 2827;
        /// <summary>
        /// error code 643 自動溫度超時未到 
        /// </summary>
        public ushort error643
        {
            set { _error643 = value; }
            get { return _error643; }
        }
        #endregion

        #region 644_腔體門未關閉完成	01 01 0B 0C 00 01
        private ushort _error644 = 2828;
        /// <summary>
        /// error code 644 腔體門未關閉完成
        /// </summary>
        public ushort error644
        {
            set { _error644 = value; }
            get { return _error644; }
        }
        #endregion

        #region 645_停機排氣閥未關	01 01 0B 0D 00 01
        private ushort _error645 = 2829;
        /// <summary>
        /// error code 645 停機排氣閥未關
        /// </summary>
        public ushort error645
        {
            set { _error645 = value; }
            get { return _error645; }
        }
        #endregion

        #region 646_蓄壓桶超壓	01 01 0B 0E 00 01
        private ushort _error646 = 2830;
        /// <summary>
        /// error code 646蓄壓桶超壓
        /// </summary>
        public ushort error646
        {
            set { _error646 = value; }
            get { return _error646; }
        }
        #endregion

        #region 647_迴風口加熱超溫異常	01 01 0B 0F 00 01
        private ushort _error647 = 2831;
        /// <summary>
        /// error code 647 迴風口加熱超溫異常
        /// </summary>
        public ushort error647
        {
            set { _error647 = value; }
            get { return _error647; }
        }
        #endregion

        #region 648_腔體內左前加熱超溫異常	01 01 0B 10 00 01
        private ushort _error648 = 2832;
        /// <summary>
        /// error code 648 腔體內左前加熱超溫異常
        /// </summary>
        public ushort error648
        {
            set { _error648 = value; }
            get { return _error648; }
        }
        #endregion

        #region 649_腔體內右中前加熱超溫異常	01 01 0B 11 00 01
        private ushort _error649 = 2833;
        /// <summary>
        /// error code 649 腔體內右中前加熱超溫異常
        /// </summary>
        public ushort error649
        {
            set { _error649 = value; }
            get { return _error649; }
        }
        #endregion

        #region 650_數位溫控器加熱超溫	01 01 0B 12 00 01
        private ushort _error650 = 2834;
        /// <summary>
        /// error code 650 數位溫控器加熱超溫
        /// </summary>
        public ushort error650
        {
            set { _error650 = value; }
            get { return _error650; }
        }
        #endregion

        #region 651_迴風口加熱超溫	01 01 0B 13 00 01
        private ushort _error651 = 2835;
        /// <summary>
        /// error code 651 迴風口加熱超溫
        /// </summary>
        public ushort error651
        {
            set { _error651 = value; }
            get { return _error651; }
        }
        #endregion

        #region 652_腔門未閉合定位	01 01 0B 14 00 01
        private ushort _error652 = 2836;
        /// <summary>
        /// error code 652 腔門未閉合定位
        /// </summary>
        public ushort error652
        {
            set { _error652 = value; }
            get { return _error652; }
        }
        #endregion

        #region 653_腔門旋轉關閉未定位	01 01 0B 15 00 01
        private ushort _error653 = 2837;
        /// <summary>
        /// error code 653 腔門旋轉關閉未定位
        /// </summary>
        public ushort error653
        {
            set { _error653 = value; }
            get { return _error653; }
        }
        #endregion

        #region 654_腔內壓力太高	01 01 0B 16 00 01
        private ushort _error654 = 2838;
        /// <summary>
        /// error code 654 腔內壓力太高
        /// </summary>
        public ushort error654
        {
            set { _error654 = value; }
            get { return _error654; }
        }
        #endregion

        #region 655_安全卡榫縮回定位異常	01 01 0B 17 00 01
        private ushort _error655 = 2839;
        /// <summary>
        /// error code 655 安全卡榫縮回定位異常
        /// </summary>
        public ushort error655
        {
            set { _error655 = value; }
            get { return _error655; }
        }
        #endregion

        #region 656_ 自動狀態溫升時間設定錯誤	01 01 0B 18 00 01
        private ushort _error656 = 2840;
        /// <summary>
        /// error code 656 自動狀態溫升時間設定錯誤
        /// </summary>
        public ushort error656
        {
            set { _error656 = value; }
            get { return _error656; }
        }
        #endregion

        #region 657_冷卻水箱水溫過高	01 01 0B 19 00 01
        private ushort _error657 = 2841;
        /// <summary>
        /// error code 657  
        /// </summary>
        public ushort error657
        {
            set { _error657 = value; }
            get { return _error657; }
        }
        #endregion

        #region 658_冷卻水箱欠水	01 01 0B 1A 00 01
        private ushort _error658 = 2842;
        /// <summary>
        /// error code 658 冷卻水箱欠水
        /// </summary>
        public ushort error658
        {
            set { _error658 = value; }
            get { return _error658; }
        }
        #endregion

        #region 659_冷卻水箱循環水馬達過載	01 01 0B 1B 00 01
        private ushort _error659 = 2843;
        /// <summary>
        /// error code 659 冷卻水箱循環水馬達過載
        /// </summary>
        public ushort error659
        {
            set { _error659 = value; }
            get { return _error659; }
        }
        #endregion

        #region 660_排水閥開啟異常	01 01 0B 1C 00 01
        private ushort _error660 = 2844;
        /// <summary>
        /// error code 660 排水閥開啟異常
        /// </summary>
        public ushort error660
        {
            set { _error660 = value; }
            get { return _error660; }
        }
        #endregion
    }
}

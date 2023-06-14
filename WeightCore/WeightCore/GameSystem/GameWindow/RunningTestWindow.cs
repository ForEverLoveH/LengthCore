using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls;
using Sunny.UI;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameWindowSys;
using WeightCore.GameSystem.MyControl;
using WeightCoreModel.GameModel;
namespace WeightCore.GameSystem.GameWindow
{
    public partial class RunningTestWindow : Form
    {
        public RunningTestWindow()
        {
            InitializeComponent();
        }
        private SportProjectInfos sportProjectInfos { get; set; }
        public string CreateTime { get; internal set; }
        public string School { get; internal set; }
        /// <summary>
        ///  是否已经写入
        /// </summary>
        private bool IsWrite=false;
        /// <summary>
        /// 是否已经开始测试
        /// </summary>
        private bool IsStart= false;

        private string _groupName = string.Empty;
        private Dictionary<string, string> localInfo = new Dictionary<string, string>();
        private List<UserControl1> _userControl1s = new List<UserControl1>();
        private List<SerialReader> SerialReaders = new List<SerialReader>();
        private List<string> ConnectionPort = new List<string>();
        private NFC_Helper USBWatcher = new NFC_Helper();
        private bool isMatchingDevice = false;
        private string _portName = "CH340";
        private int _nowRound = 0;
        private StringBuilder writeScoreLog = new StringBuilder();
        private string AutoMatchLog = Application.StartupPath + "\\Data\\AutoMatchLog.log";
        private string AutoUploadLog = Application.StartupPath + "\\Data\\AutoUploadLog.log";
        private string AutoPrintLog = Application.StartupPath + "\\Data\\AutoPrintLog.log";

        private void RunningTestWindow_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            string code = "程序集版本:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            toolStripStatusLabel1.Text = code;
            sportProjectInfos = RunningTestingWindowSys.Instance.LoadingSportProjectInfo();
            if (!InitUserControll())
            {
                this.Close();
            }
            else
            {
                int roundCount = sportProjectInfos.RoundCount;
                if (roundCount > 0)
                {
                    for (int i = 0; i < roundCount; i++)
                    {
                        RoundCbx.Items.Add($"第{i + 1}轮");
                    }

                    RoundCbx.SelectedIndex = 0;
                }
                List<LocalInfos> localInfosList = RunningTestingWindowSys.Instance.GetLocalInfo();
                foreach (var itsm in localInfosList)
                {
                    localInfo.Add(itsm.key, itsm.value);
                }
                USBWatcher.AddUSBEventWatcher(USBEventHandler, USBEventHandler, new TimeSpan(0, 0, 1));
                RunningTestingWindowSys .Instance. InitListViewHead(sportProjectInfos.RoundCount,listView1);
                RunningTestingWindowSys.Instance.UpDataGroupData( GroupCombox,School,CreateTime);
                loadLocalData();
            }
        }

        private void loadLocalData()
        {
            try
            {
                string[] strg = File.ReadAllLines(AutoMatchLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        uiCheckBox1.Checked = true;
                    }
                    else
                    {
                        uiCheckBox1.Checked = false;
                    }
                }

                strg = File.ReadAllLines(AutoUploadLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        uiCheckBox2.Checked = true;
                    }
                    else
                    {
                        uiCheckBox2.Checked = false;
                    }
                }

                strg = File.ReadAllLines(AutoPrintLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        uiCheckBox3.Checked = true;
                    }
                    else
                    {
                        uiCheckBox3.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        /// usb设备拔插监视
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USBEventHandler(Object sender, EventArrivedEventArgs e)
        {
            //暂未实现
            var watcher = sender as ManagementEventWatcher;
            watcher.Stop();

            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
                Console.WriteLine("设备连接");
                if (isMatchingDevice)
                {
                    RefreshComPorts(); ///扫描串口
                }
            }
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {
                if (!isMatchingDevice)
                {
                    Console.WriteLine("设备断开");
                    List<string> list = CheckPortISConnected();
                    if (list.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var l in list)
                        {
                            sb.AppendLine($"{l}断开!");
                        }
                        MessageBox.Show(sb.ToString());
                    }
                    //检测断开,断开提示
                    //MessageBox.Show("设备断开请检查");
                }
            }

            watcher.Start();
        }

        private bool InitUserControll()
        {
            try
            {
                CloseAllSerial();
                bool S = RunningTestingWindowSys.Instance.ShowRunningMachineSetWindow();
                if (S == false)
                {
                    return false;
                }
                else
                {
                    int machineCount = RunningTestingWindowSys.Instance.GetMachineCount();
                    _portName = RunningTestingWindowSys.Instance.GetCurrentPortName();
                    _userControl1s.Clear();
                    flowLayoutPanel1.Controls.Clear();
                    List<string> lis = new List<string>();
                    for (int i = 0; i < sportProjectInfos.RoundCount; i++)
                    {
                        lis.Add($"第{i + 1}轮");
                    }

                    for (int i = 0; i < machineCount; i++)
                    {
                        UserControl1 us = new UserControl1()
                        {
                            p_title = $"第{i + 1}号设备",
                            p_roundCbx_items = lis,

                        };
                        _userControl1s.Add(us);
                        flowLayoutPanel1.Controls.Add(us);
                        SerialReader reader = new SerialReader(i);
                        reader.AnalyCallback = AnalyData;
                        reader.ReceiveCallback = ReceiveData;
                        reader.SendCallback = SendData;
                        SerialReaders.Add(reader);
                    }

                    return true;
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
                return false;
            }
        }

        private void SendData(MachineMsgCode data)
        {
        }

        private void ReceiveData(byte[] data)
        {
        }

        private void AnalyData(SerialMsg data)
        {
            if (data == null) return;
            switch (data.mms.type)
            {
                case 1:
                    string temp = _userControl1s[data.number].p_toolState;
                    try
                    {
                        string ts = ConnectionPort[data.number];
                        _userControl1s[data.number].p_toolState = ts + "已连接,设备登录成功";
                    }
                    catch (Exception ex)
                    {
                        _userControl1s[data.number].p_toolState = temp;
                        LoggerHelper.Debug(ex);
                    }

                    break;
                case 2:
                    //设备开始测试返回命令
                    if (data.mms.code == 1)
                        Console.WriteLine("测试开始成功");
                    break;
                case 3:
                    //获取成绩
                    Console.WriteLine();
                    try
                    {
                        IsWrite = true;
                        double sg_result = data.mms.sg_result;
                        double tz_result = data.mms.tz_result;
                        double bmi_result = data.mms.bmi_result;
                        int index = data.number;
                        if (sg_result > 0 && tz_result > 0)
                        {
                            string idnumber = _userControl1s[index].p_IdNumber;
                            string stuName = _userControl1s[index].p_Name;
                            writeScoreLog.AppendLine($"考号:{idnumber},姓名:{stuName}," +
                               $"成绩:{sg_result.ToString()}cm,{tz_result.ToString()}kg,BMI:{bmi_result.ToString()}");
                        }
                        _userControl1s[index].p_Score = sg_result.ToString();
                        _userControl1s[index].p_Score1 = tz_result.ToString();
                        _userControl1s[index].p_Score2 = bmi_result.ToString();
                        if (_userControl1s[index].p_stateCbx_selectIndex == 0)
                            _userControl1s[index].p_stateCbx_selectIndex = 1;
                        SerialReaders[index].SendMessage(data.mms);
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Debug(ex);
                    }
                     

                    break;
                case 4:
                    //查询设备信息
                    break;
                default: break;

            }
        }

        private void CloseAllSerial()
        {
            foreach (var item in SerialReaders)
            {
                if (item != null)
                {
                    item.CloseCom();
                }
            }

            SerialReaders.Clear();
            ConnectionPort.Clear();
            foreach (var it in _userControl1s)
            {
                it.p_toolState = "设备未连接";
                it.p_toolState_color = Color.Red;
                it.p_title_Color = System.Drawing.SystemColors.ControlLight;

            }

            GC.Collect();
        }

        /// <summary>
        /// 刷新串口
        /// </summary>
        private void RefreshComPorts()
        {
            try
            {
                string[] portNames = GetPortDeviceName();
                if (portNames.Length == 0)
                {
                    MessageBox.Show($"未找到{_portName}串口,请检查驱动");
                    MatchBtnSwitch(true);
                    return;
                }
                foreach (var port in portNames)
                {
                    CheckPortISConnected();
                    //已连接则跳过
                    if (ConnectionPort.Contains(port)) continue;
                    int step = 0;
                    foreach (var sr in SerialReaders)
                    {
                        if (sr != null && !sr.IsComOpen())
                        {
                            string strException = string.Empty;
                            int nRet = sr.OpenCom(port, 9600, out strException);
                            if (nRet == 0)
                            {
                                //连接成功
                                _userControl1s[step].p_toolState = $"{port}已连接";
                                _userControl1s[step].p_toolState_color = Color.Green;
                                _userControl1s[step].p_title_Color = Color.MediumSpringGreen;
                            }
                            else
                            {
                                MessageBox.Show($"{port}连接失败\n错误:{strException},请检查");
                            }
                            break;
                        }
                        step++;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
            finally
            {
                try
                {
                    CheckPortISConnected();
                    if (ConnectionPort.Count == _userControl1s.Count)
                    {
                        ControlHelper.ThreadInvokerControl(this, () =>
                        {
                            MatchBtnSwitch(true);
                            string portSavePath = Application.StartupPath + "\\Data\\portSave.log";
                            File.WriteAllLines(portSavePath, ConnectionPort);
                            MessageBox.Show("设备串口匹配完成");
                        });
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.Debug(ex);
                }
            }
        }
        /// <summary>
        /// 检查串口是否连接
        /// </summary>
        /// <returns></returns>
        private List<string> CheckPortISConnected()
        {
            List<string> breakPorts = new List<string>();
            ConnectionPort.Clear();
            int step = 0;
            foreach (var sr in SerialReaders)
            {
                step++;
                if (sr != null)
                {
                    try
                    {
                        if (sr.IsComOpen())
                        {
                            ConnectionPort.Add(sr.iSerialPort.PortName);
                        }
                        else
                        {
                            breakPorts.Add(sr.iSerialPort.PortName);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Debug(ex);
                    }
                }
                else
                {
                    breakPorts.Add($"设备{step}号");
                }
            }
            return breakPorts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void MatchBtnSwitch(bool flag)
        {
            if (flag)
            {
                uiButton3.Text = "匹配设备";
                uiButton3.BackColor = Color.White;
                isMatchingDevice = false;
            }
            else
            {
                uiButton3.Text = "匹配中";
                uiButton3.BackColor = Color.Red;
                isMatchingDevice = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        private string[] GetPortDeviceName(string portName = null)
        {
            if (string.IsNullOrEmpty(portName)) portName = _portName;
            List<string> strs = new List<string>();
            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("select * from Win32_PnPEntity where Name like '%(COM%'"))
            {
                var hardInfos = searcher.Get();
                foreach (var hardInfo in hardInfos)
                {
                    if (hardInfo.Properties["Name"].Value != null)
                    {
                        string deviceName = hardInfo.Properties["Name"].Value.ToString();
                        if (deviceName.Contains(portName))
                        {
                            int a = deviceName.IndexOf("(COM") + 1; //a会等于1
                            string str = deviceName.Substring(a, deviceName.Length - a);
                            a = str.IndexOf(")"); //a会等于1
                            str = str.Substring(0, a);
                            strs.Add(str);
                        }
                    }
                }
            }
            return strs.ToArray();
        }
        /// <summary>
        /// 导入最近匹配
        /// </summary>
        private void ImportRecentMatch()
        {
            try
            {
                string portSavePath = Application.StartupPath + "\\Data\\portSave.log";
                string[] devPorts = GetPortDeviceName();
                if (devPorts.Length == SerialReaders.Count)
                {
                    ConnectionPort.Clear();
                    int step = 0;
                    foreach (var sr in SerialReaders)
                    {
                        string port = devPorts[step];
                        if (sr != null && !sr.IsComOpen())
                        {
                            string strException = string.Empty;
                            int nRet = sr.OpenCom(port, 9600, out strException);
                            if (nRet == 0)
                            {
                                //连接成功
                                _userControl1s[step].p_toolState = $"{port}已连接";
                                _userControl1s[step].p_toolState_color = Color.Green;
                                _userControl1s[step].p_title_Color = Color.MediumSpringGreen;
                                ConnectionPort.Add(port);
                            }
                            else
                            {
                                UIMessageBox.ShowError($"{port}连接失败\n错误:{strException},请检查");
                            }

                            break;
                        }

                        step++;
                    }

                    ControlHelper.ThreadInvokerControl(this, () =>
                    {
                        MatchBtnSwitch(true);
                        File.WriteAllLines(portSavePath, ConnectionPort);
                        MessageBox.Show("设备串口匹配完成");
                    });
                }
            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
                return;
            }
        }
        #region 页面事件
        /// <summary>
        /// 参数设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            InitUserControll();
        }

        /// <summary>
        /// 导入最近匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            CloseAllSerial();
            int nlen = _userControl1s.Count;
            for (int i = 0; i < nlen; i++)
            {
                //初始化访问读写器实例
                SerialReader reader = new SerialReader(i);
                //回调函数
                reader.AnalyCallback = AnalyData;
                reader.ReceiveCallback = ReceiveData;
                reader.SendCallback = SendData;
                SerialReaders.Add(reader);
            }

            //导入最近匹配
            ImportRecentMatch();
        }
        /// <summary>
        /// 匹配设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            CloseAllSerial();
            int nlen = _userControl1s.Count;
            for (int i = 0; i < nlen; i++)
            {
                //初始化访问读写器实例
                SerialReader reader = new SerialReader(i);
                //回调函数
                reader.AnalyCallback = AnalyData;
                reader.ReceiveCallback = ReceiveData;
                reader.SendCallback = SendData;
                SerialReaders.Add(reader);
            }
            //匹配设备
            MatchBtnSwitch(isMatchingDevice);
        }
        
        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton4_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.RefreshGetGroup(GroupCombox,CreateTime,School);
        }
        private void GroupCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.UpDateListView(listView1, GroupCombox);
        }
        private void RoundCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RoundCbx.SelectedIndex >= 0)
            {
                _nowRound = RoundCbx.SelectedIndex;
            }
        }
        /// <summary>
        /// 自动匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void uiButton5_Click(object sender, EventArgs e)
        {
            AutoMatchStudent();
        }
        /// <summary>
        /// 选择匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton6_Click(object sender, EventArgs e)
        {
            ChooseMatchStudent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton7_Click(object sender, EventArgs e)
        {
            IsStart = true;
            List<string> list = CheckPortISConnected();
            if (list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var l in list)
                {
                    sb.AppendLine($"{l}断开!");
                }
                MessageBox.Show(sb.ToString());
                return;
            }
            if (!IsWrite)
            {
                MachineMsgCode mmc = new MachineMsgCode();
                mmc.type = 2;
                int step = 0;
                foreach (var sr in SerialReaders)
                {
                    if (string.IsNullOrEmpty(_userControl1s[step].p_IdNumber)
                       || _userControl1s[step].p_IdNumber == "未分配") continue;
                    if (sr != null && sr.IsComOpen())
                    {
                        sr.SendMessage(mmc);
                    }
                    step++;
                }
            }
            else
            {
                UIMessageBox.ShowWarning("请先写入上一次的测试结果！！");
                return;
            }
        }
        private void uiButton8_Click(object sender, EventArgs e)
        {
            IsWrite = false;
            RunningTestingWindowSys.Instance. WriteScoreIntoDb(listView1,sportProjectInfos,GroupCombox,_userControl1s);
            if(uiCheckBox1.Checked)
            {
                AutoMatchStudent();
            }
        }
        /// <summary>
        /// 上传本组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        private void uiButton9_Click(object sender, EventArgs e)
        {
            UpLoadCurrentGroupScore();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton10_Click(object sender, EventArgs e)
        {
            string groupName = GroupCombox.Text;
            new Thread((ThreadStart)delegate
            {
              RunningTestingWindowSys.Instance  .PrintScore(groupName,sportProjectInfos);
            }).Start();
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            ListView lv1 = (ListView)sender;
            ListViewItem lvi1 = lv1.GetItemAt(e.X, e.Y);
            if (lvi1 != null && e.Button == MouseButtons.Right)
            {
                this.cmsListViewItem.Show(lv1, e.X, e.Y);
            }
        }
        private void 缺考ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetGradeError(listView1,"缺考",_nowRound,GroupCombox);
        }
        private void 中退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetGradeError(listView1, "中退", _nowRound, GroupCombox);
        }

        private void 犯规ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetGradeError(listView1, "犯规", _nowRound, GroupCombox);
        }

        private void 弃权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunningTestingWindowSys.Instance.SetGradeError(listView1, "弃权", _nowRound, GroupCombox);
        }

        private void 修正成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
            FromRoundWindow fcr = new FromRoundWindow();
            fcr._idnumber = idnumber;
            fcr.mode = 1;
            fcr.ShowDialog();
            RunningTestingWindowSys.Instance.UpDateListView(listView1, GroupCombox);
        }

        private void 成绩重测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
                FromRoundWindow fcr = new FromRoundWindow();
                fcr._idnumber = idnumber;
                fcr.mode = 0;
                fcr.ShowDialog();
                RunningTestingWindowSys.Instance.UpDateListView(listView1, GroupCombox);
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        #endregion
        /// <summary>
        /// 自动匹配
        /// </summary>
        private void AutoMatchStudent()
        {
            try
            {
                ClearMatchStudent();
                List<DbPersonInfos> dbPersonInfos = RunningTestingWindowSys.Instance.UpDateListView(listView1,GroupCombox);
                int nlen = SerialReaders.Count;
                int step = 0;
                int i = 0;
                foreach (var dpi in dbPersonInfos)
                {
                    List<ResultInfos> resultInfos = RunningTestingWindowSys.Instance.GetResultInfo(dpi.Id.ToString(), _nowRound);
                    i++;
                    if (resultInfos.Count != 0) continue;
                    listView1.Items[i - 1].Selected = true;
                    _userControl1s[step].p_IdNumber = dpi.IdNumber;
                    _userControl1s[step].p_Name = dpi.Name;
                    _userControl1s[step].p_Score = "0 cm";
                    _userControl1s[step].p_Score1 = "0 kg";
                    _userControl1s[step].p_Score2 = "0";
                    _userControl1s[step].p_roundCbx_selectIndex = resultInfos.Count;
                    _userControl1s[step].p_stateCbx_selectIndex = 0;
                    step++;
                    if (step >= _userControl1s.Count) break;
                }
                if (step == 0)
                {
                    if (GroupCombox.SelectedIndex >= 0
                        && GroupCombox.SelectedIndex < GroupCombox.Items.Count - 2)
                    {
                        GroupCombox.SelectedIndex++;
                       RoundCbx.SelectedIndex = 0;
                        AutoMatchStudent();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }
         /// <summary>
         /// 选择匹配
         /// </summary>
        private void ChooseMatchStudent()
        {
            ClearMatchStudent();
            if (listView1.SelectedItems.Count == 0) return;
            int step = 0;
            List<DbPersonInfos> dbPersonInfos = RunningTestingWindowSys.Instance.GetDBPersonInfo(GroupCombox);
             
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                try
                {
                    string idNumber = item.SubItems[3].Text;
                    List<ResultInfos> resultInfos = RunningTestingWindowSys.Instance.GetResultInfo(idNumber);
                    
                    if (resultInfos.Count != 0) continue;
                    DbPersonInfos dbPersonInfos1 = dbPersonInfos.Find(a => a.IdNumber == idNumber);
                    _userControl1s[step].p_IdNumber = idNumber;
                    _userControl1s[step].p_Name = dbPersonInfos1.Name;
                    _userControl1s[step].p_Score = "0 cm";
                    _userControl1s[step].p_Score1 = "0 kg";
                    _userControl1s[step].p_Score2 = "0";
                    _userControl1s[step].p_roundCbx_selectIndex = resultInfos.Count;
                    _userControl1s[step].p_stateCbx_selectIndex = 0;
                    step++;
                    if (step >= _userControl1s.Count) break;
                }
                catch (Exception ex)
                {
                    LoggerHelper.Debug(ex);
                }
            }
        }
        /// <summary>
        /// 清除当前匹配
        /// </summary>
        private void ClearMatchStudent()
        {
            int nlen = _userControl1s.Count;
            for (int i = 0; i < nlen; i++)
            {
                _userControl1s[i].p_IdNumber = "未分配";
                _userControl1s[i].p_Name = "未分配";
                _userControl1s[i].p_Score = "0";
                _userControl1s[i].p_roundCbx_selectIndex = 0;
                _userControl1s[i].p_stateCbx_selectIndex = 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        
        private void UpLoadCurrentGroupScore()
        {
            ControlHelper.ThreadInvokerControl(this, () =>
            {
                uiButton9.Text = "上传中";
                uiButton9.ForeColor = Color.Red;
            });
            string gn = GroupCombox.Text;
            StartUploadStuGroupScore(gn);
        }

        private void StartUploadStuGroupScore(string groupName)
        {
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(UploadStuGroupScoreThreadFun);
            Thread t = new Thread(ParStart);
            t.IsBackground = true;
            t.Start(groupName);
        }

        public void UploadStuGroupScoreThreadFun(Object obj)
        {
            if (RunningTestingWindowSys.Instance.UploadStuGroupScoreThreadFun(obj))
            {
                ControlHelper.ThreadInvokerControl(this, () =>
                {
                    uiButton9.Text = "上传本组";
                    uiButton9.ForeColor = Color.Black;
                    UIMessageBox.ShowSuccess("上传成功！！");
               
                });
            }
        }

        private void RunningTestWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var reader in SerialReaders)
            {
                if (reader != null && reader.IsComOpen())
                {
                    reader.CloseCom();
                }
            }
        }
    }
}

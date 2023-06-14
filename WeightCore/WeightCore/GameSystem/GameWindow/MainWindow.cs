﻿using HZH_Controls.Forms;
using HZH_Controls;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameWindowSys;

namespace WeightCore.GameSystem.GameWindow
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private   string   createTime = string .Empty;
        private string schoolName  = string .Empty;
       private  string groupName = string .Empty;

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string code = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = "德育龙体育测试系统" + code;
            MainWindowSys.Instance.UpdataTreeview(treeView1);
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            string txt = e.Node.Text;
            string fullpath = e.Node.FullPath;
            string[] paths = fullpath.Split('\\');
            if (e.Node.Level == 0)
            {
                createTime = paths[0];
            }
            else if (e.Node.Level == 1)
            {
                createTime = paths[0];
                schoolName = paths[1];

            }
            else if (e.Node.Level == 2)
            {
                createTime = paths[0];
                schoolName = paths[1];
                groupName = paths[2];
            }
            MainWindowSys.Instance.UpdataGroupDataView(createTime, schoolName, groupName, listView1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucNavigationMenu1_ClickItemed(object sender, EventArgs e)
        {
            string  name = ucNavigationMenu1.SelectItem.Text.Trim();
            if(string.IsNullOrEmpty(name)) return;
            else
            {
                switch(name)
                {
                    case "导入名单":
                        if (MainWindowSys.Instance.ShowPersonWindow())
                        {
                            HZH_Controls.ControlHelper.ThreadInvokerControl(this,()=>
                                MainWindowSys.Instance.UpdataTreeview(treeView1));
                        } 
                        break;
                    case "数据库初始化":

                        break;
                    case "系统参数设置":
                        SystemSettingWindowSys.Instance.ShowSystemSettingWindow();
                        break;
                    case "平台设置":
                        PlatFormSettingWindowSys.Instance.ShowPlatFormSettingWindow();
                        break;
                    case "上传成绩":
                        Thread newThread = new Thread(new ParameterizedThreadStart((o) =>
                        {
                            UpLoadingGradeToServer();
                        }));
                        newThread.IsBackground = true;
                        newThread.Start();
                        break;
                    case "导出成绩":
                        if (!string.IsNullOrEmpty(groupName))
                        {
                            if (ExportGradeWindowSys.Instance.SetExporWindowData(groupName))
                            {
                                UIMessageBox.ShowSuccess("导出成功");
                            }
                            else
                            {
                                UIMessageBox.ShowError("导出失败！！");
                            }
                        }
                        else
                        {
                             UIMessageBox.ShowWarning("请先确定好组号！！");
                        }
                        break;
                    case "启动测试":
                        MainWindowSys.Instance.ShowRunningTestWindow(createTime,schoolName);
                      
                        break;
                }
            }
        }
        int proMax = 0;
        int proVal= 0;
        /// <summary>
        /// 上传成绩
        /// </summary>
        private void UpLoadingGradeToServer()
        {
            if (treeView1.SelectedNode != null)
            {
                String path = treeView1.SelectedNode.FullPath;
                string[] fsp = path.Split('\\');
                string projectName = string.Empty;
                if (fsp.Length > 0)
                {
                    projectName = fsp[0];

                }
                if (string.IsNullOrEmpty(projectName))
                {
                    FrmTips.ShowTipsError(this, "请先选择上传的成绩项目！！");
                    return;
                }
                string outMes = MainWindowSys.Instance.UpLoadingGradeToServer(fsp, ref proMax, ref proVal, ucProcessLine1, timer1);
                var str = outMes.Trim();
                if (string.IsNullOrEmpty(outMes))
                {
                    MessageBox.Show("上传成功");
                }
                else
                {
                    MessageBox.Show(outMes);
                }

                if (!string.IsNullOrEmpty(projectName))
                {
                    ControlHelper.ThreadInvokerControl(this, () =>
                    {
                        MainWindowSys.Instance.UpdataGroupDataView(createTime, schoolName, groupName, listView1);
                    });
                }

            }
            else
            {
                UIMessageBox.ShowError("请先选择项目数据！！");
                return;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (proMax == 0 || proMax == proVal || proVal == 0)
            {
                return;
            }
            int upV = (int)(((double)proVal / (double)proMax) * 100);
            ControlHelper.ThreadInvokerControl(this, () =>
            {
                ucProcessLine1.Value = upV;
            });
        }
    }
}

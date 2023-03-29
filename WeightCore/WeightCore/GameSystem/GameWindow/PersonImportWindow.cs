using HZH_Controls.Forms;
using MiniExcelLibs;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameWindowSys;
using WeightCoreModel.GameModel;

namespace WeightCore.GameSystem.GameWindow
{
    public partial class PersonImportWindow : Form
    {
        public PersonImportWindow()
        {
            InitializeComponent();
        }
        string paths = string.Empty;
        bool isDeleteBeforeImport = false;
        bool isImport = false;
        private Dictionary<string, string> localVales = null;
        private void uiButton1_Click(object sender, EventArgs e)
        {
            PersonImportWindowSys.Instance.ShowPlatFormSettingWindow();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            PersonImportWindowSys.Instance.OpenLocalExcelFile(uiTextBox2);
            if (!string.IsNullOrEmpty(uiTextBox2.Text))
            {
                string path = uiTextBox2.Text;
                bool t_flag = true;
                string errorMsg = string.Empty;
                try
                {
                    var rows = MiniExcel.Query<InputData>(path).ToList();
                   // listView1.View = View.Details;
                    listView1.BeginUpdate();
                    listView1.Items.Clear();
                    int step = 1;
                    foreach (var row in rows)
                    {
                        ListViewItem li = new ListViewItem();
                        li.Text = step.ToString();
                        li.SubItems.Add(row.examTime);
                        li.SubItems.Add(row.School);
                        li.SubItems.Add(row.GradeName);
                        li.SubItems.Add(row.ClassName);
                        li.SubItems.Add(row.IdNumber);
                        li.SubItems.Add(row.Name);
                        li.SubItems.Add(row.Sex);
                        li.SubItems.Add(row.GroupName);
                        listView1.Items.Insert(listView1.Items.Count, li);
                        step++;
                    }
                    listView1.EndUpdate();
                    if (t_flag)
                    {
                        paths = path;
                        MessageBox.Show("读取成功");
                    }
                    else
                    {
                        listView1.Items.Clear();
                        paths = string.Empty;
                        UIMessageBox.ShowError($"读取失败,错误({errorMsg})");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    t_flag = false;
                    LoggerHelper.Debug(ex);
                    return;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton4_Click(object sender, EventArgs e)
        {
            if (PersonImportWindowSys.Instance.SaveTemplateExcel())
            {
                FrmTips.ShowTipsSuccess(this, "获取成功!!");
            }
            else
            {
                FrmTips.ShowTips(this, "获取失败！！");
                return;
            }
        }

        private void uiButton7_Click(object sender, EventArgs e)
        {
            isDeleteBeforeImport = true;
            if (string.IsNullOrEmpty(paths))
            {
                UIMessageBox.ShowError("路径错误！！");
                return;

            }
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(ExcelInputDataBase);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.IsBackground = true;
            thread.Start(paths);
        }

        private void ExcelInputDataBase(object obj)
        {
            HZH_Controls.ControlHelper.ThreadInvokerControl(this, () =>
            {
                 
                if (PersonImportWindowSys.Instance.ExcelInputDataBase(obj, isDeleteBeforeImport, uiLabel5))
                {
                    UIMessageBox.ShowSuccess("导入成功");
                    isImport = true;
                    
                }
                else
                {
                    UIMessageBox.ShowError("导入失败");
                    return;
                }
            });
        }
        private void uiButton2_Click(object sender, EventArgs e)
        {
            string groupNum = uiTextBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(groupNum))
            {
                UIMessageBox.ShowError("请确定你需要拉取的学生数据数目！！");
                return;
            }
            else
            {
                PersonImportWindowSys.Instance.LoadingServerData(groupNum, ref localVales, listView1, uiLabel5);
            }
        }

        private void PersonImportWindow_Load(object sender, EventArgs e)
        {
            PersonImportWindowSys.Instance.InitListViewHeader(listView1);
        }

        private void uiButton6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(paths))
            {
                UIMessageBox.ShowError("路径错误！！");
                return;
            }
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(ExcelInputDataBase);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.IsBackground = true;
            thread.Start(paths);
        }
    }
}

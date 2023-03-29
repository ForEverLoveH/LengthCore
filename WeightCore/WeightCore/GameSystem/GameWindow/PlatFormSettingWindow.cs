using HZH_Controls.Forms;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameModel;
using WeightCore.GameSystem.GameWindowSys;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WeightCoreModel.GameModel;

namespace WeightCore.GameSystem.GameWindow
{
    public partial class PlatFormSettingWindow : Form
    {
        public PlatFormSettingWindow()
        {
            InitializeComponent();
        }
        string MachineCode = String.Empty;
        string MachineCode1 = String.Empty;
        string ExamId = String.Empty;
        string Platform = String.Empty;
        string Platforms = String.Empty;
        public Dictionary<string, string> localValues = null;

        private void PlatFormSettingWindow_Load(object sender, EventArgs e)
        {
            PlatFormSettingWindowSys.Instance.LoadingInitData(ref MachineCode, ref MachineCode1 , ref ExamId, ref Platforms, ref Platform,  comboBox1,comboBox2,comboBox3,comboBox4, ref localValues);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            string url = comboBox2.Text;
            if (url == String.Empty)
            {
                UIMessageBox.ShowError("网址为空！！");
                return;
            }
            PlatFormSettingWindowSys.Instance.GetExamNum( comboBox3, url, localValues);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            if(GetEquipMentCode())
            {
                UIMessageBox.ShowSuccess("获取成功！！");
            }
            else
            {
                UIMessageBox.ShowError("获取失败！！");
                return;
            }
        }

        private bool GetEquipMentCode()
        {
            try
            {
                comboBox1.Items.Clear();
                string examId = comboBox3.Text;
                if (string.IsNullOrEmpty(examId))
                {
                    FrmTips.ShowTipsError(this, "考试id为空!");
                    return false;
                }
                if (examId.IndexOf('_') != -1)
                {
                    examId = examId.Substring(examId.IndexOf('_') + 1);
                }
                string url = comboBox2.Text;
                if (string.IsNullOrEmpty(url))
                {
                    FrmTips.ShowTipsError(this, "网址为空!");
                    return false;
                }
                url += RequestUrl.GetMachineCodeListUrl;
                RequestParameter RequestParameter = new RequestParameter();
                RequestParameter.AdminUserName = localValues["AdminUserName"];
                RequestParameter.TestManUserName = localValues["TestManUserName"];
                RequestParameter.TestManPassword = localValues["TestManPassword"];
                RequestParameter.ExamId = examId;
                //序列化
                string JsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestParameter);
                var formDatas = new List<FormItemModel>();
                //添加其他字段
                formDatas.Add(new FormItemModel()
                {
                    Key = "data",
                    Value = JsonStr
                });
                var httpUpload = new HttpUpload();
                string result = String.Empty;
                try
                {
                    result = HttpUpload.PostForm(url, formDatas);
                }
                catch (Exception ex)
                {
                    throw new Exception("请检查网络");
                }
                GetMachineCodeList upload_Result = JsonConvert.DeserializeObject<GetMachineCodeList>(result);
                if (upload_Result == null || upload_Result.results == null || upload_Result.results.Count == 0)
                {
                    string error = string.Empty;
                    try
                    {
                        error = upload_Result.Error;
                    }
                    catch (Exception)
                    {
                        error = string.Empty;
                    }
                    FrmTips.ShowTipsError(this, $"提交错误,错误码:[{error}]");
                    return false;
                }

                foreach (var item in upload_Result.results)
                {
                    string str = $"{item.title}_{item.MachineCode}";
                    comboBox1.Items.Add(str);
                    comboBox4.Items.Add(str);
                }
                return true ;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                MessageBox.Show(ex.Message);
                return false;   
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            if (PlatFormSettingWindowSys.Instance.SaveDataToDataBase( comboBox1, comboBox2, comboBox3,comboBox4))
            {
                DialogResult  = DialogResult.OK;
                this.Close();
            }
            else
            {
                DialogResult   = DialogResult.Cancel;
                this.Close();

            }
        }
    }
}

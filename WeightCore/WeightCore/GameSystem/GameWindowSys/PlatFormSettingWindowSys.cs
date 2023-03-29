using HZH_Controls.Forms;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameModel;
using WeightCore.GameSystem.GameWindow;
using WeightCoreModel.GameModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;

namespace WeightCore.GameSystem.GameWindowSys
{
    public  class PlatFormSettingWindowSys
    {
        public static PlatFormSettingWindowSys Instance;
        PlatFormSettingWindow PlatFormSettingWindow = null;
        IFreeSql freeSql = FreeSqlHelper.Sqlite;
        public void Awake()
        {
            Instance= this;
        }
        
        

        public void ShowPlatFormSettingWindow()
        {
            PlatFormSettingWindow = new PlatFormSettingWindow();
            PlatFormSettingWindow.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiComboBox3"></param>
        /// <param name="url"></param>
        /// <param name="localValues"></param>
        public void GetExamNum(System.Windows.Forms.ComboBox uiComboBox3, string url, Dictionary<string, string> localValues)
        {
            try
            {
                url += RequestUrl.GetExamListUrl;
                RequestParameter RequestParameter = new RequestParameter();
                RequestParameter.AdminUserName = localValues["AdminUserName"];
                RequestParameter.TestManUserName = localValues["TestManUserName"];
                RequestParameter.TestManPassword = localValues["TestManPassword"];
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
                result = HttpUpload.PostForm(url, formDatas,null,null,null,20000);
                GetExamList upload_Result = JsonConvert.DeserializeObject<GetExamList>(result);

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
                    UIMessageBox.ShowError($"提交错误,错误码:[{error}]");
                    return;
                }
                foreach (var item in upload_Result.results)
                {
                    string str = $"{item.title}_{item.exam_id}";
                    uiComboBox3.Items.Add(str);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                UIMessageBox.Show(ex.Message);
            }
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiComboBox1"></param>
        /// <param name="uiComboBox2"></param>
        /// <param name="uiComboBox3"></param>
        /// <returns></returns>
        public bool SaveDataToDataBase(System.Windows.Forms.ComboBox comboBox1, System.Windows.Forms.ComboBox comboBox2, System.Windows.Forms.ComboBox comboBox3, System.Windows.Forms.ComboBox comboBox4)
        {
            try
            {
                string Platform = comboBox2.Text;
                string ExamId = comboBox3.Text;
                string MachineCode = comboBox1.Text;
                string MachineCode1 = comboBox4.Text;
                int sum = 0;
                int result = freeSql.Update<LocalInfos>().Set(a => a.value == Platform).Where(a => a.key == "Platform").ExecuteAffrows();
                sum += result;
                result = freeSql.Update<LocalInfos>().Set(a => a.value == ExamId).Where(a => a.key == "ExamId").ExecuteAffrows();
                sum += result;
                result = freeSql.Update<LocalInfos>().Set(a => a.value == MachineCode).Where(a => a.key == "MachineCode").ExecuteAffrows();
                sum += result;
                if (sum == 3)
                {
                    UIMessageBox.ShowSuccess("保存成功");
                    return true;

                }
                else
                {
                    UIMessageBox.ShowError("更新失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                UIMessageBox.ShowError("更新失败");
                return false;
            }
        }

        public void LoadingInitData(ref string MachineCode, ref string MachineCode1, ref string ExamId, ref string Platform, ref string Platforms, System.Windows.Forms.ComboBox comboBox1, ComboBox comboBox2, ComboBox comboBox3, ComboBox comboBox4, ref Dictionary<string, string> localValues)
        {
            List<LocalInfos> localInfos = freeSql.Select<LocalInfos>().ToList();
            localValues = new Dictionary<string, string>();
            foreach (var li in localInfos)
            {
                localValues.Add(li.key, li.value);
                switch (li.key)
                {
                    case "MachineCode":
                        MachineCode = li.value;
                        break;
                    case "MachineCode1":
                        MachineCode1 = li.value;
                        break;
                    case "ExamId":
                        ExamId = li.value;
                        break;
                    case "Platform":
                        Platform = li.value;
                        break;
                    case "Platforms":
                        Platforms = li.value;
                        break;
                }
            }
            if (string.IsNullOrEmpty(MachineCode))
            {
                UIMessageBox.ShowError("设备码为空");
            }
            else
            {
                comboBox1.Text = MachineCode;
            }
            if (string.IsNullOrEmpty(MachineCode1))
            {
                UIMessageBox.ShowError("设备码1为空");
            }
            else
            {
                comboBox4.Text = MachineCode1;
            }
            if (string.IsNullOrEmpty(ExamId))
            {
                UIMessageBox.ShowError("考试id为空");
            }
            else
            {
                comboBox3.Text = ExamId;
            }
            if (string.IsNullOrEmpty(Platforms))
            {
                UIMessageBox.ShowError("平台码为空");
            }
            else
            {
                string[] Platformss = Platforms.Split(';');
                comboBox2.Items.Clear();
                foreach (var item in Platformss)
                {
                    comboBox2.Items.Add(item);
                }

            }
            if (string.IsNullOrEmpty(Platform))
            {
                UIMessageBox.ShowError("平台码为空");
            }
            else
            {
                comboBox2.Text = Platform;
            }
        }
    }
}

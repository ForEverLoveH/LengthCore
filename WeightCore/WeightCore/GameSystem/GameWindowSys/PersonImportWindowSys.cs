﻿using MiniExcelLibs;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameWindow;
using WeightCoreModel.GameModel;
using WeightCore.GameSystem.GameModel;
using Newtonsoft.Json;
using WeightCoreModel.MinExcelModel;
using HZH_Controls;

namespace WeightCore.GameSystem.GameWindowSys
{
    public class PersonImportWindowSys
    {

        public static PersonImportWindowSys Instance;
        private PersonImportWindow PersonImportWindow = null;
        private IFreeSql freeSql = FreeSqlHelper.Sqlite;
        public void Awake() { Instance = this; }

        public bool ShowPersonImportWindow()
        {
            PersonImportWindow = new PersonImportWindow();
            var s = PersonImportWindow.ShowDialog();
            if (s == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
        public bool LoadingServerData(string name, ref Dictionary<string, string> localValues, ListView listView1, UILabel uiLabel5)
        {
            LoadingWindow loadingWindow = new LoadingWindow();
            new Thread((ThreadStart)delegate()
            {
                loadingWindow.Show();
            }).Start();
            try
            {
                List<LocalInfos> localInfos = freeSql.Select<LocalInfos>().ToList();
                localValues = new Dictionary<string, string>();
                foreach (LocalInfos info in localInfos)
                {
                    localValues.Add(info.key, info.value);
                }

                RequestParameter RequestParameter = new RequestParameter();
                RequestParameter.AdminUserName = localValues["AdminUserName"];
                RequestParameter.TestManUserName = localValues["TestManUserName"];
                RequestParameter.TestManPassword = localValues["TestManPassword"];
                string ExamId0 = localValues["ExamId"];
                ExamId0 = ExamId0.Substring(ExamId0.IndexOf('_') + 1);
                string MachineCode0 = localValues["MachineCode"];
                MachineCode0 = MachineCode0.Substring(MachineCode0.IndexOf('_') + 1);
                RequestParameter.ExamId = ExamId0;
                RequestParameter.MachineCode = MachineCode0;
                RequestParameter.GroupNums = name + "";
                //序列化
                string JsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestParameter);
                string url = localValues["Platform"] + RequestUrl.GetGroupStudentUrl;
                var formDatas = new List<FormItemModel>();
                //添加其他字段
                formDatas.Add(new FormItemModel()
                {
                    Key = "data",
                    Value = JsonStr
                });
                var httpUpload = new HttpUpload();
                string result = string.Empty;
                try
                {
                    result = HttpUpload.PostForm(url, formDatas);
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("请检查网络");
                    LoggerHelper.Debug(ex);
                }

                //GetGroupStudent upload_Result = JsonConvert.DeserializeObject<GetGroupStudent>(result);
                string[] strs = GetGroupStudent.CheckJson(result);
                GetGroupStudent upload_Result = null;
                if (strs[0] == "1")
                {
                    upload_Result = JsonConvert.DeserializeObject<GetGroupStudent>(result);
                }
                else
                {
                    upload_Result = new GetGroupStudent();
                    upload_Result.Error = strs[1];
                }

                bool bFlag = false;
                if (upload_Result == null || upload_Result.Results == null || upload_Result.Results.groups.Count == 0)
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
                }
                else
                {
                    bFlag = true;
                }

                if (bFlag)
                {
                    DownlistOutputExcel(upload_Result, listView1, uiLabel5);
                }

                return true;
            }

            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }
            finally
            {
                loadingWindow.Invoke((EventHandler)delegate { loadingWindow.Close(); });
                loadingWindow.Dispose();
            }
        }

        private void DownlistOutputExcel(GetGroupStudent upload_Result, ListView listView1, UILabel uiLabel5)
        {
            try
            {
                List<GroupsItem> Groups = upload_Result.Results.groups;
                List<InputData> doc = new List<InputData>();
                int step = 1;

                listView1.BeginUpdate();
                listView1.Items.Clear();
                //listView1.View = View.Details;
                //序号	学校	 年级	班级 	姓名	 性别	准考证号	 组别名称
                foreach (var Group in Groups)
                {
                    string groupId = Group.GroupId;
                    string groupName = Group.GroupName;
                    foreach (var StudentInfo in Group.StudentInfos)
                    {
                        InputData idata = new InputData();
                        idata.Id = step;
                        idata.examTime = StudentInfo.examTime;
                        idata.School = StudentInfo.SchoolName;
                        idata.GradeName = StudentInfo.GradeName;
                        idata.ClassName = StudentInfo.ClassName;
                        idata.Name = StudentInfo.Name;
                        idata.Sex = StudentInfo.Sex;
                        idata.IdNumber = StudentInfo.IdNumber;
                        idata.GroupName = groupId;

                        ListViewItem li = new ListViewItem();
                        li.Text = step.ToString();
                        li.SubItems.Add(StudentInfo.examTime);
                        li.SubItems.Add(StudentInfo.SchoolName);
                        li.SubItems.Add(StudentInfo.GradeName);
                        li.SubItems.Add(StudentInfo.ClassName);
                        li.SubItems.Add(StudentInfo.IdNumber);
                        li.SubItems.Add(StudentInfo.Name);
                        li.SubItems.Add(StudentInfo.Sex);
                        li.SubItems.Add(groupId);
                        listView1.Items.Insert(listView1.Items.Count, li);
                        doc.Add(idata);
                        step++;
                    }
                }
                listView1.EndUpdate();
                String importpath = Application.StartupPath + $"\\模板\\下载名单\\";
                if (!Directory.Exists(importpath))
                {
                    Directory.CreateDirectory(importpath);
                }
                importpath = Path.Combine(importpath, $"downList{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
                ExcelUtils.MiniExcel_OutPutExcel(importpath, doc);
                //MessageBox.Show("下载完成");
               uiLabel5.Text = $"下载名单成功,共{step - 1}人";
               

            }
            catch (Exception exception)
            {
                LoggerHelper.Debug(exception);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveTemplateExcel()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xls files(*.xls)|*.xls|xlsx file(*.xlsx)|*.xlsx|All files(*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = $"导入模板{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            string path = Application.StartupPath + "\\excel\\output.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog.FileName;
                File.Copy(@"./模板/导入名单模板.xlsx", path);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiTextBox1"></param>

        public void OpenLocalExcelFile(UITextBox uiTextBox1)
        {
            string path = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;      //该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";     //弹窗的标题
            dialog.InitialDirectory = Application.StartupPath + "\\";    //默认打开的文件夹的位置
            dialog.Filter = "MicroSoft Excel文件(*.xlsx)|*.xlsx";       //筛选文件
            dialog.ShowHelp = true;     //是否显示“帮助”按钮
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.FileName;
            }
            if (!String.IsNullOrEmpty(path))
            {
                uiTextBox1.Text = path;
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isDeleteBeforeImport"></param>
        /// <param name="uiLabel5"></param>
        /// <returns></returns>
        public bool ExcelInputDataBase(object obj, bool isDeleteBeforeImport, UILabel uiLabel5)
        {
            LoadingWindow loading = new LoadingWindow();
            new Thread((ThreadStart)delegate ()
            {
                loading.Show();
            }).Start();
            bool isRes = false;
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                if (isDeleteBeforeImport)
                {
                    string[] datas = new string[] { "DbGroupInfos", "DbPersonInfos", "ResultInfos", "LogInfos" };
                    int result1 = freeSql.Delete<DbGroupInfos>().Where("1=1").ExecuteAffrows();
                    int result2 = freeSql.Delete<DbPersonInfos>().Where("1=1").ExecuteAffrows();
                    int result3 = freeSql.Delete<ResultInfos>().Where("1=1").ExecuteAffrows();
                    int result4 = freeSql.Update<LogInfos>().Set(a => a.State == -404).Where("1=1").ExecuteAffrows();
                }
                string path = obj as string;
                if (!String.IsNullOrEmpty(path))
                {
                    SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                    var rows = MiniExcel.Query<InputData>(path).ToList();
                    HashSet<string> set = new HashSet<string>();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        string[] examTime = rows[i].examTime.Split(' ');
                        set.Add(rows[i].GroupName + "#" + examTime[0]);
                    }
                    List<string> rolesList = new List<string>();
                    rolesList.AddRange(set);
                    freeSql.Select<DbGroupInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                    List<DbGroupInfos> insertDbGroupInfosList = new List<DbGroupInfos>();
                    for (int i = 0; i < rolesList.Count; i++)
                    {
                        maxSortId++;
                        string role = rolesList[i];
                        string[] roles = role.Split("#");
                        string groupName = roles[0];
                        string examTime = roles[1];
                        DbGroupInfos dbGroupInfos = new DbGroupInfos();
                        dbGroupInfos.Name = groupName;
                        dbGroupInfos.CreateTime = examTime;
                        dbGroupInfos.SortId = maxSortId;
                        dbGroupInfos.IsRemoved = 0;
                        dbGroupInfos.ProjectId = 0.ToString();
                        dbGroupInfos.IsAllTested = 0;
                        insertDbGroupInfosList.Add(dbGroupInfos);
                    }
                    int sy = freeSql.InsertOrUpdate<DbGroupInfos>().SetSource(insertDbGroupInfosList).IfExistsDoNothing().ExecuteAffrows();
                    freeSql.Select<DbPersonInfos>().Aggregate(x => x.Max(x.Key.SortId), out maxSortId);
                    List<DbPersonInfos> personInfos = new List<DbPersonInfos>();
                    foreach (var row in rows)
                    {
                        maxSortId++;
                        string personID = row.IdNumber.ToString();
                        string name = row.Name.ToString();
                        int sex = row.Sex == "男" ? 0 : 1;
                        string SchoolName = row.School;
                        string GradeName = row.GradeName;
                        string classNumber = row.ClassName;
                        string GroupName = row.GroupName;
                        string[] examTimes = row.examTime.Split(' ');
                        string examTime = examTimes[0];
                        DbPersonInfos dbPersonInfos = new DbPersonInfos();
                        dbPersonInfos.CreateTime = examTime;
                        dbPersonInfos.SortId = maxSortId;
                        dbPersonInfos.ProjectId = "0";
                        dbPersonInfos.SchoolName = SchoolName;
                        dbPersonInfos.GradeName = GradeName;
                        dbPersonInfos.ClassNumber = classNumber;
                        dbPersonInfos.GroupName = GroupName;
                        dbPersonInfos.Name = name;
                        dbPersonInfos.IdNumber = personID;
                        dbPersonInfos.Sex = sex;
                        dbPersonInfos.State = 0;
                        dbPersonInfos.FinalScore = -1;
                        dbPersonInfos.uploadState = 0;
                        personInfos.Add(dbPersonInfos);
                    }
                    int reslut = freeSql.InsertOrUpdate<DbPersonInfos>()
                   .SetSource(personInfos)
                   .IfExistsDoNothing()
                   .ExecuteAffrows();
                    if (reslut == 0) { isRes = false; }
                    else { isRes = true; }
                    sw.Stop();
                    string time = (sw.ElapsedMilliseconds / 1000).ToString("0.000") + "秒";
                    uiLabel5.Text = $"耗时：{time},实际插入:{reslut},重复：{rows.Count - reslut}";
                }
                if (isRes)

                    return true;
                else
                    return false; 
            }
            catch (Exception ex)
            {
                LoggerHelper .Debug (ex);
                return false;
            }
            finally
            {
                loading.Invoke((EventHandler)delegate { loading.Close(); });
                loading.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ShowPlatFormSettingWindow()
        {
            PlatFormSettingWindowSys.Instance.ShowPlatFormSettingWindow();
        }

        public void InitListViewHeader(ListView listView1)
        {
            listView1.View = View.Details;
            ColumnHeader[] Header = new ColumnHeader[100];
            int sp = 0;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "序号";
            Header[sp].Width = 80;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "时间";
            Header[sp].Width = 200;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "学校";
            Header[sp].Width = 220;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "年级";
            Header[sp].Width = 200;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "班级";
            Header[sp].Width = 200;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 200;
            sp++;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 150;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "性别";
            Header[sp].Width = 200;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组别名称";
            Header[sp].Width = 150;
            sp++;

            ColumnHeader[] Header1 = new ColumnHeader[sp];
            listView1.Columns.Clear();
            for (int i = 0; i < Header1.Length; i++)
            {
                Header1[i] = Header[i];
            }
            listView1.Columns.AddRange(Header1);
        }
    }
}

using HZH_Controls.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameModel;
using WeightCoreModel.GameModel;

namespace WeightCore.GameSystem.GameWindowSys
{
    public class MainWindowSys
    {
        public static MainWindowSys Instance;
        public void Awake()
        {
            Instance = this;
        }
        IFreeSql freeSql = FreeSqlHelper.Sqlite;

        /// <summary>
        /// 更新项目树数据
        /// </summary>
        /// <param name="treeView1"></param>
        public void UpdataTreeview(TreeView treeView1)
        {
            treeView1.Nodes.Clear();
            try
            {
                List<TreeViewModel> treeViewModels = new List<TreeViewModel>();
                treeView1.Nodes.Clear();
                var lists = freeSql.Select<DbPersonInfos>().Distinct().ToList(a => new { a.CreateTime, a.SchoolName, a.GroupName });
                // Console.WriteLine();
                foreach (var item in lists)
                {
                    TreeViewModel treeViewModel = treeViewModels.Find(a => a.CreateTime == item.CreateTime);
                    if (treeViewModel == null)
                    {
                        treeViewModels.Add(new TreeViewModel { CreateTime = item.CreateTime, schoolModels = new List<TreeViewSchoolModel>() });
                    }
                    treeViewModel = treeViewModels.Find(a => a.CreateTime == item.CreateTime);
                    if (treeViewModel != null)
                    {
                        TreeViewSchoolModel treeViewSchoolsModel = treeViewModel.schoolModels.Find(a => a.schoolName == item.SchoolName);
                        if (treeViewSchoolsModel == null)
                        {
                            treeViewModel.schoolModels.Add(new TreeViewSchoolModel()
                            {
                                schoolName = item.SchoolName,
                                Groups = new List<string> { item.GroupName }
                            });
                        }
                        else
                        {
                            treeViewSchoolsModel.Groups.Add(item.GroupName);
                        }
                    }
                }
                for (int i = 0; i < treeViewModels.Count; i++)
                {
                    TreeNode tn1 = new TreeNode(treeViewModels[i].CreateTime);
                    List<TreeViewSchoolModel> treeViewSchoolsModel = treeViewModels[i].schoolModels;
                    for (int j = 0; j < treeViewSchoolsModel.Count; j++)
                    {
                        TreeNode tn2 = new TreeNode(treeViewSchoolsModel[j].schoolName);
                        foreach (var group in treeViewSchoolsModel[j].Groups)
                        {
                            tn2.Nodes.Add(group);
                        }
                        tn1.Nodes.Add(tn2);
                    }
                    treeView1.Nodes.Add(tn1);

                }
            } catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createTime"></param>
        /// <param name="schoolName"></param>
        /// <param name="groupName"></param>
        /// <param name="listView1"></param>
        public void UpdataGroupDataView(string createTime, string schoolName, string groupName, ListView listView1)
        {
            if (string.IsNullOrEmpty(createTime) || string.IsNullOrEmpty(schoolName) || string.IsNullOrEmpty(groupName))
            {
                return;
            }
            else
            {
                listView1.Items.Clear();
                SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                List<DbPersonInfos> personInfos = freeSql.Select<DbPersonInfos>().Where(a => a.CreateTime == createTime && a.SchoolName == schoolName && a.GroupName == groupName).ToList();
                if (personInfos.Count != 0)
                {
                    int step = 0;
                    listView1.BeginUpdate();
                    InitListViewHeader(listView1, sportProjectInfos.RoundCount);
                    Font font = new Font(Control.DefaultFont, FontStyle.Bold);
                    bool isBestScore = sportProjectInfos.BestScoreMode == 0 ? true : false;
                    if (sportProjectInfos.BestScoreMode == 0)
                    {
                        isBestScore = true;
                    }
                    foreach (var dbperson in personInfos)
                    {
                        ListViewItem listViewItem = new ListViewItem()
                        {
                            UseItemStyleForSubItems = false,
                            Text = step.ToString(),
                        };
                        listViewItem.SubItems.Add(dbperson.CreateTime);
                        listViewItem.SubItems.Add(dbperson.SchoolName);
                        listViewItem.SubItems.Add(dbperson.GroupName);
                        listViewItem.SubItems.Add(dbperson.Name);
                        listViewItem.SubItems.Add(dbperson.IdNumber);
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId== dbperson.Id.ToString() && a.IsRemoved == 0).OrderBy(a => a.Id).ToList();
                        int resultRound = 0;
                        int allUpload = 0;
                        foreach (var res in resultInfos)
                        {
                            if (res.State != 1)
                            {
                                string sts = ResultStateType.Match(res.State);
                                listViewItem.SubItems.Add(sts);
                                listViewItem.SubItems[listViewItem.SubItems.Count - 1].ForeColor = Color.Red;
                            }
                            else
                            {
                                listViewItem.SubItems.Add(res.Result.ToString());
                                listViewItem.SubItems[listViewItem.SubItems.Count - 1].BackColor = Color.MediumSpringGreen;
                            }
                            listViewItem.SubItems[listViewItem.SubItems.Count - 1].Font = font;
                            if (res.uploadState == 1)
                            {
                                allUpload++;
                            }
                            resultRound++;
                            if (resultRound >= 3)
                            {
                                break;
                            }
                        }
                        for (int i = resultRound; i < 3; i++)
                        {
                            listViewItem.SubItems.Add("未测试");
                        }
                        if (allUpload == 3)
                        {
                            listViewItem.SubItems.Add("已上传");
                            listViewItem.SubItems[listViewItem.SubItems.Count - 1].ForeColor = Color.MediumSpringGreen;
                            listViewItem.SubItems[listViewItem.SubItems.Count - 1].Font = font;
                        }
                        else
                        {
                            listViewItem.SubItems.Add("未上传");
                            listViewItem.SubItems[listViewItem.SubItems.Count - 1].ForeColor = Color.Red;
                        }
                        step++;
                        listView1.Items.Insert(listView1.Items.Count, listViewItem);
                    }
                }
                listView1.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="roundCount"></param>
        private void InitListViewHeader(ListView listView1, int roundCount)
        {
            listView1.View = View.Details;

            ColumnHeader[] headers = new ColumnHeader[100];
            int sp = 0;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "序号";
            headers[sp].Width = 50;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "时间";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "学校";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "组别名称";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "姓名";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "准考证号";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "身高(cm)";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "体重(KG)";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "BMI";
            headers[sp].Width = 150;
            sp++;
            headers[sp] = new ColumnHeader();
            headers[sp].Text = "上传状态";
            headers[sp].Width = 150;
            sp++;
            ColumnHeader[] columnHeaders = new ColumnHeader[sp];
            listView1.Columns.Clear();
            for (int i = 0; i < columnHeaders.Length; i++)
            {
                columnHeaders[i] = headers[i];
            }
            listView1.Columns.AddRange(columnHeaders);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShowPersonWindow()
        {
            return PersonImportWindowSys.Instance.ShowPersonImportWindow();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fsp"></param>
        /// <param name="proMax"></param>
        /// <param name="proVal"></param>
        /// <param name="ucProcessLine1"></param>
        /// <param name="timer1"></param>
        /// <returns></returns>
        public string UpLoadingGradeToServer(object obj, ref int proMax, ref int proVal, UCProcessLine ucProcessLine1, Timer timer1)
        {
            try 
            {
                string cpuid = CPUHelper.GetCpuID();
                List<Dictionary<string, string>> successList = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> errorList = new List<Dictionary<string, string>>();
                Dictionary<string, string> localInfos = new Dictionary<string, string>();
                List<LocalInfos> list0 = freeSql.Select<LocalInfos>().ToList();
                foreach (var item in list0)
                {
                    localInfos.Add(item.key, item.value);
                }
                //组
                string groupName = obj as string;
                SportProjectInfos sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
                List<DbGroupInfos> dbGroupInfos = new List<DbGroupInfos>();
                ///查询本项目已考组
                if (!string.IsNullOrEmpty(groupName))
                {
                    //sql0 += $" AND Name = '{groupName}'";
                    dbGroupInfos = freeSql.Select<DbGroupInfos>().Where(a => a.Name == groupName).ToList();
                }
                else
                {
                    dbGroupInfos = freeSql.Select<DbGroupInfos>().ToList();
                }
                UploadResultsRequestParameter urrp = new UploadResultsRequestParameter();
                urrp.AdminUserName = localInfos["AdminUserName"];
                urrp.TestManUserName = localInfos["TestManUserName"];
                urrp.TestManPassword = localInfos["TestManPassword"];
                string MachineCode = localInfos["MachineCode"];
                string ExamId = localInfos["ExamId"];
                if (MachineCode.IndexOf('_') != -1)
                {
                    MachineCode = MachineCode.Substring(MachineCode.IndexOf('_') + 1);
                }
                if (ExamId.IndexOf('_') != -1)
                {
                    ExamId = ExamId.Substring(ExamId.IndexOf('_') + 1);
                }
                urrp.MachineCode = MachineCode;
                urrp.ExamId = ExamId;
                StringBuilder messageSb = new StringBuilder();
                StringBuilder logWirte = new StringBuilder();
                proMax = dbGroupInfos.Count;
                proVal = 0;
                ucProcessLine1.Visible = true;
                ucProcessLine1.Value = 0;
                timer1.Start();

                ///按组上传
                foreach (var gInfo in dbGroupInfos)
                {
                    proVal++;
                    List<DbPersonInfos> dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == gInfo.Name).ToList();
                    StringBuilder resultSb = new StringBuilder();
                    List<SudentsItem> sudentsItems = new List<SudentsItem>();
                    //IdNumber 对应Id
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    //取值模式
                    bool isBestScore = sportProjectInfos.BestScoreMode == 0;
                    foreach (var stu in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber == stu.IdNumber).ToList();
                        //无成绩的跳过
                        if (resultInfos.Count == 0) continue;
                        int state = -1;
                        string examTime = string.Empty;
                        double MaxScore = 99999;
                        if (isBestScore) MaxScore = 0;
                        foreach (var ri in resultInfos)
                        {
                            if (ri.State <= 0) continue;
                            ///异常状态
                            if (ri.State != 1)
                            {
                                if (isBestScore && MaxScore < 0)
                                {
                                    //取最大值
                                    MaxScore = 0;
                                    state = ri.State;
                                }
                                else if (!isBestScore && MaxScore > 99999)
                                {
                                    //取最小值
                                    MaxScore = 99999;
                                    state = ri.State;
                                }
                            }
                            else
                            {
                                if (isBestScore && MaxScore < ri.Result)
                                {
                                    //取最大值
                                    MaxScore = ri.Result;
                                    state = ri.State;
                                }
                                else if (!isBestScore && MaxScore > ri.Result)
                                {
                                    //取最小值
                                    MaxScore = ri.Result;
                                    state = ri.State;
                                }
                            }
                            examTime = ri.CreateTime;
                        }
                        if (state < 0) continue;
                        if (state != 1)
                        {
                            MaxScore = 0;
                        }
                        List<RoundsItem> roundsItems = new List<RoundsItem>();
                        RoundsItem rdi = new RoundsItem();
                        rdi.RoundId = 1;
                        rdi.State = ResultStateType.Match(state);
                        rdi.Time = examTime;
                        StringBuilder logSb = new StringBuilder();
                        try
                        {
                            List<LogInfos> logInfos = freeSql.Select<LogInfos>()
                                .Where(a => a.IdNumber == stu.IdNumber && a.State != -404)
                                .ToList();
                            logInfos.ForEach(item =>
                            {
                                string sbtxt = $"时间：{item.CreateTime},考号:{item.IdNumber},{item.Remark};";
                                logSb.Append(sbtxt);
                            });
                        }
                        catch (Exception)
                        {
                            //logSb.Clear();
                        }
                        rdi.Memo = logSb.ToString();
                        rdi.Ip = cpuid;
                        ///可以处理成绩
                        rdi.Result = MaxScore;
                        //string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
                        #region 查询文件
                        //成绩根目录
                        Dictionary<string, string> dic_images = new Dictionary<string, string>();
                        Dictionary<string, string> dic_viedos = new Dictionary<string, string>();
                        Dictionary<string, string> dic_texts = new Dictionary<string, string>();
                        string scoreRoot = Application.StartupPath + $"\\Scores\\{sportProjectInfos.Name}\\{stu.GroupName}\\";
                        DateTime.TryParse(examTime, out DateTime examTime_dt);
                        string dateStr = examTime_dt.ToString("yyyyMMdd");
                        string GroupNo = $"{dateStr}_{stu.GroupName}_{stu.IdNumber}_1";

                        if (Directory.Exists(scoreRoot))
                        {
                            List<DirectoryInfo> rootDirs = new DirectoryInfo(scoreRoot).GetDirectories().ToList();
                            string dirEndWith = $"_{stu.IdNumber}_{stu.Name}";
                            DirectoryInfo directoryInfo = rootDirs.Find(a => a.Name.EndsWith(dirEndWith));
                            if (directoryInfo != null)
                            {
                                string stuDir = Path.Combine(scoreRoot, directoryInfo.Name);
                                GroupNo = $"{dateStr}_{stu.GroupName}_{directoryInfo.Name}_1";
                                if (Directory.Exists(stuDir))
                                {
                                    int step = 1;
                                    FileInfo[] files = new DirectoryInfo(stuDir).GetFiles("*.jpg");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_images.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                    step = 1;
                                    files = new DirectoryInfo(stuDir).GetFiles("*.txt");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_texts.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                    step = 1;
                                    files = new DirectoryInfo(stuDir).GetFiles("*.mp4");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_viedos.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                }
                            }

                        }
                        #endregion
                        rdi.GroupNo = GroupNo;
                        rdi.Text = dic_texts;
                        rdi.Images = dic_images;
                        rdi.Videos = dic_viedos;
                        roundsItems.Add(rdi);
                        SudentsItem ssi = new SudentsItem();
                        ssi.SchoolName = stu.SchoolName;
                        ssi.GradeName = stu.GradeName;
                        ssi.ClassNumber = stu.ClassNumber;
                        ssi.Name = stu.Name;
                        ssi.IdNumber = stu.IdNumber;
                        ssi.Rounds = roundsItems;
                        sudentsItems.Add(ssi);
                        map.Add(stu.IdNumber, stu.Id.ToString());
                    }
                    if (sudentsItems.Count == 0) continue;
                    urrp.Sudents = sudentsItems;

                    //序列化json
                    string JsonStr = JsonConvert.SerializeObject(urrp);
                    string url = localInfos["Platform"] + RequestUrl.UploadResults;

                    var httpUpload = new HttpUpload();
                    var formDatas = new List<FormItemModel>();
                    //添加其他字段
                    formDatas.Add(new FormItemModel()
                    {
                        Key = "data",
                        Value = JsonStr
                    });

                    logWirte.AppendLine();
                    logWirte.AppendLine();
                    logWirte.AppendLine(JsonStr);

                    //上传学生成绩
                    string result = HttpUpload.PostForm(url, formDatas);
                    Upload_Result upload_Result = JsonConvert.DeserializeObject<Upload_Result>(result);
                    string errorStr = "null";
                    List<Dictionary<string, int>> result1 = upload_Result.Result;
                    foreach (var item in sudentsItems)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        //map
                        dic.Add("Id", map[item.IdNumber]);
                        dic.Add("IdNumber", item.IdNumber);
                        dic.Add("Name", item.Name);
                        dic.Add("uploadGroup", item.Rounds[0].GroupNo);
                        var value = 0;
                        result1.Find(a => a.TryGetValue(item.IdNumber, out value));
                        if (value == 1 || value == -4)
                        {
                            successList.Add(dic);
                            messageSb.AppendLine($"{gInfo.Name}组 考号:{item.IdNumber} 姓名:{item.Name}上传失败,错误内容:成绩已上传0000000000000000000000000000000000");
                        }
                        else if (value != 0)
                        {
                            errorStr = uploadResult.Match(value);
                            dic.Add("error", errorStr);
                            errorList.Add(dic);
                            messageSb.AppendLine($"{gInfo.Name}组 考号:{item.IdNumber} 姓名:{item.Name}上传失败,错误内容:{errorStr}");
                        }
                    }

                    #region 失败写入日志 
                    WriteDefaultLog(errorList,gInfo);
                    #endregion
                }
                #region 成功写入日志
                WriteSucessLog(successList);

                #endregion
                LoggerHelper.Monitor(logWirte.ToString());
                string outpitMessage = messageSb.ToString();
                return outpitMessage;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return ex.Message;
            }
            finally
            {
                ucProcessLine1.Visible = false;
                ucProcessLine1.Value = 0;
                timer1.Stop();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorList"></param>
        private void WriteDefaultLog(List<Dictionary<string, string>> errorList, DbGroupInfos gInfo)
        {
            string txtpath = Application.StartupPath + $"\\Log\\upload\\";
            if (!Directory.Exists(txtpath))
            {
                Directory.CreateDirectory(txtpath);
            }
            StringBuilder errorsb = new StringBuilder();
            errorsb.AppendLine($"失败:{errorList.Count}");
            errorsb.AppendLine("****************error***********************");
            foreach (var item in errorList)
            {
                errorsb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]} 错误:{item["error"]}");
            }
            errorsb.AppendLine("*****************error**********************");
            if (errorList.Count != 0)
            {
                string txtpath1 = Path.Combine(txtpath, $"error_{gInfo.Name}_upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath1, errorsb.ToString());
                errorList.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="successList"></param>
        private void WriteSucessLog(List<Dictionary<string, string>> successList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"成功:{successList.Count}");
            sb.AppendLine("******************success*********************");
            foreach (var item in successList)
            {
                freeSql.Update<DbPersonInfos>()
                    .Set(a => a.uploadGroup == "1")
                    .Where(a => a.Id == Convert.ToInt32(item["Id"]))
                    .ExecuteAffrows();
                freeSql.Update<ResultInfos>()
                    .Set(a => a.uploadState== 1)
                    .Where(a => a.PersonId == item["Id"])
                    .ExecuteAffrows(); ;
                sb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]}");
            }
            sb.AppendLine("*******************success********************");
            if (successList.Count != 0)
            {
                string txtpath = Application.StartupPath + $"\\Log\\upload\\";
                txtpath = Path.Combine(txtpath, $"upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath, sb.ToString());
                successList.Clear();
            }
        }

        public  void ShowRunningTestWindow(string createTime,string school)
        {
             RunningTestingWindowSys.Instance.ShowRunningTestingWindow(createTime,school );
        }
    }
}

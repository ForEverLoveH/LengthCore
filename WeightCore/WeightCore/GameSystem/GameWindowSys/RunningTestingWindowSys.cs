using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls;
using MiniExcelLibs;
using Newtonsoft.Json;
using Sunny.UI;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameModel;
using WeightCore.GameSystem.GameWindow;
using WeightCore.GameSystem.MyControl;
using WeightCoreModel.GameModel;

namespace WeightCore.GameSystem.GameWindowSys
{
    public class RunningTestingWindowSys
    {
        public static RunningTestingWindowSys Instance;
        private RunningTestWindow RunningTestWindow = null;
        private IFreeSql freeSql = FreeSqlHelper.Sqlite;
        public void Awake()
        {
            Instance = this;
        }
        public void ShowRunningTestingWindow()
        {
            RunningTestWindow = new RunningTestWindow();
            RunningTestWindow.Show();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SportProjectInfos LoadingSportProjectInfo()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShowRunningMachineSetWindow()
        {
           return RunningMachineSettingWindowSys.Instance.ShowRunningMachineSettingWindow();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetMachineCount()
        {
            return RunningMachineSettingWindowSys.Instance.GetMachineCount();
        }
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public string GetCurrentPortName()
        {
            return RunningMachineSettingWindowSys.Instance.GetPortName();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<LocalInfos> GetLocalInfo()
        {
            return freeSql.Select<LocalInfos>().ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView1"></param>
        /// <param name="groupCombox"></param>
        /// <returns></returns>
        public List<DbPersonInfos> UpDateListView(ListView listView1, ComboBox groupCombox)
        {
            List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
            try
            {
                listView1.Items.Clear();
                int index = groupCombox.SelectedIndex;
                string groupName = groupCombox.Text;
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                if (dbPersonInfos.Count == 0) return dbPersonInfos;
                int step = 1;
                listView1.BeginUpdate();
                Font font = new Font(Control.DefaultFont, FontStyle.Bold);
                foreach (var dbperson in dbPersonInfos)
                {
                    ListViewItem li = new ListViewItem();
                    li.UseItemStyleForSubItems = false;
                    li.Text = step.ToString();
                    li.SubItems.Add(dbperson.SchoolName);
                    li.SubItems.Add(dbperson.GradeName);
                    li.SubItems.Add(dbperson.IdNumber);
                    li.SubItems.Add(dbperson.Name);
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dbperson.Id.ToString() && a.IsRemoved == 0).OrderBy(a => a.SportItemType).ToList();
                    int resultRound = 0;
                    int allUpload = 0;
                    foreach (var res in resultInfos)
                    {
                        if (res.State != 1)
                        {
                            string st = ResultStateType.Match(res.State);
                            li.SubItems.Add(st);
                            li.SubItems[li.SubItems.Count-1].ForeColor =Color.Red;
                        }
                        else
                        {
                            li.SubItems.Add(res.Result.ToString());
                            li.SubItems[li.SubItems.Count - 1].BackColor = Color.MediumSpringGreen;
                        }
                        li.SubItems[li.SubItems.Count - 1].Font = font;
                        resultRound++;
                        if (res.uploadState == 1) allUpload++;
                        if (resultRound >= 3) break;
                    }
                    for (int i= resultRound; i < 3; i++)
                    {
                        li.SubItems.Add("未测试    ");
                    }
                    if (allUpload == 3)
                    {
                        li.SubItems.Add("已上传");
                        li.SubItems[li.SubItems.Count - 1].ForeColor = Color.MediumSpringGreen;
                        li.SubItems[li.SubItems.Count - 1].Font = font;
                    } 
                    else
                    {
                        li.SubItems.Add("未上传");
                        li.SubItems[li.SubItems.Count - 1].ForeColor = Color.Red;
                    }
                    step++;
                    listView1.Items.Insert(listView1.Items.Count, li);
                }
                listView1.EndUpdate();
                return dbPersonInfos;
            }
            catch (Exception exception)
            {
                listView1.Items.Clear();
                dbPersonInfos.Clear();
                LoggerHelper.Debug( exception);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCombox"></param>
        /// <param name="groupName"></param>
        public void RefreshGetGroup(ComboBox groupCombox,string groupName="")
        {
            try
            {
                List<string> lis = freeSql.Select<DbGroupInfos>().Distinct().ToList(a => a.Name);
                groupCombox.Items.Clear();
                AutoCompleteStringCollection lstsourece = new AutoCompleteStringCollection();
                foreach (var item in lis)
                {
                    groupCombox.Items.Add(item);
                    lstsourece.Add(item);
                }
                groupCombox.AutoCompleteCustomSource = lstsourece;
                if (!string.IsNullOrEmpty(groupName))
                {
                    int index = groupCombox.Items.IndexOf(groupName);
                    if (index > 0)
                    {
                        groupCombox.SelectedIndex = index;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggerHelper .Debug(exception);
                return; 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="st"></param>
        /// <param name="nowRound"></param>
        /// <returns></returns>
        public List<ResultInfos> GetResultInfo(string st, int nowRound)
        {
             return  freeSql.Select<ResultInfos>().Where(a => a.PersonId ==  st && a.IsRemoved == 0 && a.RoundId == nowRound + 1).OrderBy(a => a.Id).ToList();
        }

        public List<ResultInfos> GetResultInfo(string str)
        {
            return  freeSql.Select<ResultInfos>().Where(a => a.PersonIdNumber == str && a.IsRemoved == 0).OrderBy(a => a.Id).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupCombox"></param>
        /// <returns></returns>
        public List<DbPersonInfos> GetDBPersonInfo(ComboBox groupCombox)
        {
            return  freeSql.Select<DbPersonInfos>().Where(a => a.GroupName ==groupCombox.Text).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public  void WriteScoreIntoDb(ListView listView1, SportProjectInfos sportProjectInfos, ComboBox groupCombox, List<UserControl1> _userControl1s)
        {
            freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
            List<ResultInfos> insertResults = new List<ResultInfos>();
            List<DbPersonInfos> dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupCombox.Text).ToList();
            StringBuilder errorsb = new StringBuilder();
            foreach (var user in _userControl1s)
            {
                if (string.IsNullOrEmpty(user.p_IdNumber) || user.p_IdNumber == "未分配") continue;
                string sl = user.p_Score;
                string sls = user.p_Score1;
                string slsls= user.p_Score2;
                string idNumber = user.p_IdNumber;
                int state = user.p_stateCbx_selectIndex;
                int roundId = user.p_roundCbx_selectIndex;
                double.TryParse(user.p_Score, out double score);
                double.TryParse(user.p_Score1, out double score1);
                double.TryParse(user.p_Score2, out double score2);
                DbPersonInfos df = dbPersonInfos.Find(a => a.IdNumber == idNumber);
                if (state == 0)
                {
                    errorsb.AppendLine($"{df.IdNumber},{df.Name}未完成测试!!!");
                    continue;
                }
                bool t_flag = false;
                //检查轮次
                for (int i = roundId; i < sportProjectInfos.RoundCount; i++)
                {
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>()
                          .Where(a => a.PersonIdNumber == idNumber
                          && a.IsRemoved == 0
                          && a.RoundId == i)
                          .OrderBy(a => a.Id)
                          .ToList();
                    if (resultInfos.Count == 0)
                    {
                        t_flag = true;
                        maxSortId++;
                        ResultInfos rinfo = new ResultInfos();
                        rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        rinfo.SortId = maxSortId;
                        rinfo.IsRemoved = 0;
                        rinfo.PersonId = df.Id.ToString();
                        rinfo.SportItemType = 0;
                        rinfo.PersonName = df.Name;
                        rinfo.PersonIdNumber = df.IdNumber;
                        rinfo.RoundId = i + 1;
                        rinfo.Result = score;
                        rinfo.State = state;
                        insertResults.Add(rinfo);

                        maxSortId++;
                        rinfo = new ResultInfos();
                        rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        rinfo.SortId = maxSortId;
                        rinfo.IsRemoved = 0;
                        rinfo.PersonId = df.Id.ToString();
                        rinfo.SportItemType = 1;
                        rinfo.PersonName = df.Name;
                        rinfo.PersonIdNumber = df.IdNumber;
                        rinfo.RoundId = i + 1;
                        rinfo.Result = score1;
                        rinfo.State = state;
                        insertResults.Add(rinfo);

                        maxSortId++;
                        rinfo = new ResultInfos();
                        rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        rinfo.SortId = maxSortId;
                        rinfo.IsRemoved = 0;
                        rinfo.PersonId = df.Id.ToString();
                        rinfo.SportItemType = 2;
                        rinfo.PersonName = df.Name;
                        rinfo.PersonIdNumber = df.IdNumber;
                        rinfo.RoundId = i + 1;
                        rinfo.Result = score2;
                        rinfo.State = state;
                        insertResults.Add(rinfo);
                        break;
                    }
                }
                if (!t_flag)
                {
                    errorsb.AppendLine($"{df.IdNumber},{df.Name}轮次已满");
                }
            }
            int result = freeSql.InsertOrUpdate<ResultInfos>()
                                         .SetSource(insertResults)
                                         .IfExistsDoNothing()
                                         .ExecuteAffrows();
            if (errorsb.Length != 0) MessageBox.Show(errorsb.ToString());
            if (result > 0) UpDateListView(listView1, groupCombox);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool UploadStuGroupScoreThreadFun(object obj)
        {
            LoadingWindow loadingWindow = new LoadingWindow();
            try
            {
                new Thread((ThreadStart)delegate { loadingWindow.ShowDialog(); }).Start();
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

                UploadResultsRequestParameter urrp = new UploadResultsRequestParameter();
                urrp.AdminUserName = localInfos["AdminUserName"];
                urrp.TestManUserName = localInfos["TestManUserName"];
                urrp.TestManPassword = localInfos["TestManPassword"];
                string MachineCode = localInfos["MachineCode"];
                string MachineCode1 = localInfos["MachineCode1"];
                string ExamId = localInfos["ExamId"];
                if (MachineCode.IndexOf('_') != -1)
                {
                    MachineCode = MachineCode.Substring(MachineCode.IndexOf('_') + 1);
                }
                if (MachineCode1.IndexOf('_') != -1)
                {
                    MachineCode1 = MachineCode1.Substring(MachineCode1.IndexOf('_') + 1);
                }
                if (ExamId.IndexOf('_') != -1)
                {
                    ExamId = ExamId.Substring(ExamId.IndexOf('_') + 1);
                }

                urrp.MachineCode = MachineCode;
                urrp.ExamId = ExamId;
                StringBuilder messageSb = new StringBuilder();
                StringBuilder logWirte = new StringBuilder();

                ///按组上传
                foreach (var gInfo in dbGroupInfos)
                {
                    List<DbPersonInfos> dbPersonInfos =
                        freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == gInfo.Name).ToList();

                    for (int i = 0; i < 2; i++)
                    {
                        StringBuilder resultSb = new StringBuilder();
                        List<SudentsItem> sudentsItems = new List<SudentsItem>();
                        //IdNumber 对应Id
                        Dictionary<string, string> map = new Dictionary<string, string>();
                        foreach (var stu in dbPersonInfos)
                        {
                            ResultInfos ri = freeSql.Select<ResultInfos>()
                                .Where(a => a.PersonIdNumber == stu.IdNumber && a.SportItemType == i).ToOne();
                            ///无成绩的跳过
                            if (ri == null) continue;
                            if (ri.uploadState == 1) continue;
                            string examTime = ri.CreateTime;
                            int state = ri.State;
                            double MaxScore = ri.Result;
                            if (state > 1) MaxScore = 0;
                            List<RoundsItem> roundsItems = new List<RoundsItem>();
                            RoundsItem rdi = new RoundsItem();
                            rdi.RoundId = 1;
                            rdi.State = ResultStateType.Match(state);
                            rdi.Time = examTime;
                            rdi.Result = MaxScore;
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

                            #region 查询文件

                            //成绩根目录
                            Dictionary<string, string> dic_images = new Dictionary<string, string>();
                            Dictionary<string, string> dic_viedos = new Dictionary<string, string>();
                            Dictionary<string, string> dic_texts = new Dictionary<string, string>();
                            string scoreRoot = Application.StartupPath +
                                               $"\\Scores\\{sportProjectInfos.Name}\\{stu.GroupName}\\";
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

                            #endregion 查询文件
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
                        if (i == 0)
                        {
                            urrp.MachineCode = MachineCode;
                        }
                        else
                        {
                            urrp.MachineCode = MachineCode1;
                        }

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
                            }
                            else if (value != 0)
                            {
                                errorStr = uploadResult.Match(value);
                                dic.Add("error", errorStr);
                                errorList.Add(dic);
                                messageSb.AppendLine(
                                    $"{gInfo.Name}组 考号:{item.IdNumber} 姓名:{item.Name}上传失败,错误内容:{errorStr}");
                            }
                        }
                        WriteDefaultLog(errorList,gInfo);
                    }
                }
                WriteSucessLog(successList);
                LoggerHelper.Monitor(logWirte.ToString());
                string outpitMessage = messageSb.ToString();
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }
            finally
            {

                try
                {
                    //importpath=string.Empty;
                    loadingWindow.Invoke((EventHandler)delegate { loadingWindow.Close(); });
                    loadingWindow.Dispose();
                }
                catch (Exception ex)
                {
                    LoggerHelper.Debug(ex);
                }


            }
        }

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
                string txtpath1 = Path.Combine(txtpath,
                    $"error_{gInfo.Name}_upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                File.WriteAllText(txtpath1, errorsb.ToString());
                errorList.Clear();
            }
        }


        /// <summary>
        /// 写入成功日志
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
                    .Set(a => a.uploadState == 1)
                    .Where(a => a.PersonId == item["Id"])
                    .ExecuteAffrows();
                ;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roundCount"></param>
        /// <param name="listView1"></param>
        public void InitListViewHead(int roundCount, ListView listView1)
        {
            listView1.View = View.Details;
            ColumnHeader[] Header = new ColumnHeader[100];
            int sp = 0;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "序号";
            Header[sp].Width = 40;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "学校";
            Header[sp].Width = 200;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组号";
            Header[sp].Width = 100;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 160;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 130;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "身高(cm)";
            Header[sp].Width = 120;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "体重(kg)";
            Header[sp].Width = 120;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "BMI";
            Header[sp].Width = 120;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "上传状态";
            Header[sp].Width = 120;
            sp++;

            ColumnHeader[] Header1 = new ColumnHeader[sp];
            listView1.Columns.Clear();
            for (int i = 0; i < Header1.Length; i++)
            {
                Header1[i] = Header[i];
            }
            listView1.Columns.AddRange(Header1);
        }

        public void UpDataGroupData( ComboBox groupCombox,string groupname="")
        {
            try
            {
                List<string> list = freeSql.Select<DbGroupInfos>().Distinct().ToList(a => a.Name);
                groupCombox.Items.Clear();
                AutoCompleteStringCollection lstsourece = new AutoCompleteStringCollection();
                foreach (var item in list)
                {
                    groupCombox.Items.Add(item);
                    lstsourece.Add(item);
                }
                groupCombox.AutoCompleteCustomSource = lstsourece;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                groupCombox.Items.Clear();
                groupCombox.AutoCompleteCustomSource = null;
            }

            try
            {
                if (!string.IsNullOrEmpty(groupname))
                {
                    int index = groupCombox.Items.IndexOf(groupname);
                    if (index >= 0)
                    {
                        groupCombox.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        

        public void PrintScore(string groupName,SportProjectInfos sportProjectInfos)
        {
            try
            {
                if (string.IsNullOrEmpty(groupName)) throw new Exception("未选择组");
                string path = Application.StartupPath + "\\Data\\PrintExcel\\";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, $"PrintExcel_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
                List<Dictionary<string, string>> ldic = new List<Dictionary<string, string>>();
                //序号 项目名称    组别名称 姓名  准考证号 考试状态    第1轮 第2轮 最好成绩
                List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
                dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                List<OutPutPrintExcelData> outPutExcelDataList = new List<OutPutPrintExcelData>();
                int step = 1;
                bool isBestScore = false;
                if (sportProjectInfos.BestScoreMode == 0) isBestScore = true;
                foreach (var dpInfo in dbPersonInfos)
                {
                    List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId == dpInfo.Id.ToString()).OrderBy(a => a.IsRemoved).ToList();
                    OutPutPrintExcelData opd = new OutPutPrintExcelData();
                    opd.Id = step;
                    opd.Name = dpInfo.Name;
                    opd.Sex = dpInfo.Sex == 0 ? "男" : "女";
                    opd.ExamNum = dpInfo.IdNumber;
                    opd.groupName = dpInfo.GroupName;
                    int state = -1;
                    foreach (var ri in resultInfos)
                    {
                        state = ri.State;
                        switch (ri.SportItemType)
                        {
                            case 0:
                                if (state > 1)
                                {
                                    opd.Result1 = ResultStateType.Match(state); ;
                                }
                                else
                                {
                                    opd.Result1 = ri.Result.ToString();
                                }
                                break;

                            case 1:
                                if (state > 1)
                                {
                                    opd.Result2 = ResultStateType.Match(state); ;
                                }
                                else
                                {
                                    opd.Result2 = ri.Result.ToString();
                                }
                                break;

                            case 2:
                                if (state > 1)
                                {
                                    opd.Result3 = ResultStateType.Match(state); ;
                                }
                                else
                                {
                                    opd.Result3 = ri.Result.ToString();
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    outPutExcelDataList.Add(opd);
                    step++;
                }
                MiniExcel.SaveAs(path, outPutExcelDataList);
                if (File.Exists(path))
                {
                    try
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.FileName = path;
                        p.StartInfo.Verb = "print";
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Debug(ex);
                        throw new Exception("打印机异常");
                    }
                }
                else
                {
                    throw new Exception("导出失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LoggerHelper.Debug(ex);
            }
        }

        public void SetGradeError(ListView listView1,string stateStr, int _nowRound,ComboBox comboBox)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
                int state = ResultStateType.ResultStateToInt(stateStr);
                int result = freeSql.Update<ResultInfos>().Set(a => a.State == state)
                    .Where(a => a.PersonIdNumber == idnumber && a.RoundId == _nowRound + 1).ExecuteAffrows();
                if (result == 0)
                {
                    freeSql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                    List<ResultInfos> insertResults = new List<ResultInfos>();
                    DbPersonInfos dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.IdNumber == idnumber).ToOne();

                    maxSortId++;
                    ResultInfos rinfo = new ResultInfos();
                    rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    rinfo.SortId = maxSortId;
                    rinfo.IsRemoved = 0;
                    rinfo.PersonId = dbPersonInfos.Id.ToString();
                    rinfo.SportItemType = 0;
                    rinfo.PersonName = dbPersonInfos.Name;
                    rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                    rinfo.RoundId = _nowRound + 1;
                    rinfo.Result = 0;
                    rinfo.State = state;
                    insertResults.Add(rinfo);
                    result = freeSql.InsertOrUpdate<ResultInfos>()
                                                 .SetSource(insertResults)
                                                 .IfExistsDoNothing().ExecuteAffrows();
                }

                if (result > 0)
                {
                    UpDateListView(listView1, comboBox);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        
    }
}

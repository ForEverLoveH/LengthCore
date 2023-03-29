using MiniExcelLibs;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameWindow;
using WeightCoreModel.GameModel;

namespace WeightCore.GameSystem.GameWindowSys
{
    public class ExportGradeWindowSys
    {
        public static ExportGradeWindowSys Instance;
        IFreeSql freeSql = FreeSqlHelper.Sqlite;
        ExportGradeWindow ExportGradeWindow = null;
        public void Awake()
        {
            Instance =this;
        }

        public  SportProjectInfos LoadingInitData()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sportProjectInfos"></param>
        /// <param name="isOnlyGroup"></param>
        /// <param name="groupName"></param>
        /// <param name="isAllTest"></param>
        /// <returns></returns>
        public  bool OutPutScore(SportProjectInfos sportProjectInfos, bool isOnlyGroup, string groupName, bool isAllTest)
        {
            bool isResult = false;
            LoadingWindow loading = new LoadingWindow();
            try
            {
                if (sportProjectInfos == null)
                {
                    UIMessageBox.ShowError("项目设置有误！！");
                    return false;
                }
                SaveFileDialog saveImageDialog = new SaveFileDialog();
                saveImageDialog.Title = "导出成绩";
                saveImageDialog.Filter = "xlsx file(*.xlsx)|*.xlsx";
                saveImageDialog.RestoreDirectory = true;
                string path = Application.StartupPath + $"\\excel\\output{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                //saveImageDialog.FileName = $"output_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                if (isOnlyGroup)
                {
                    saveImageDialog.FileName = $"{sportProjectInfos.Name}_{groupName}组成绩.xlsx";
                }
                else
                {
                    saveImageDialog.FileName = $"{sportProjectInfos.Name}_成绩.xlsx";
                }
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    new Thread((ThreadStart)delegate ()
                    {
                        loading.Show();
                    }).Start();
                    path = saveImageDialog.FileName;
                    List<Dictionary<string, string>> ldic = new List<Dictionary<string, string>>();
                    List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
                    if (isOnlyGroup)
                    {
                        dbPersonInfos = freeSql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                    }
                    else
                    {
                        dbPersonInfos = freeSql.Select<DbPersonInfos>().ToList();
                    }
                    List<OutPutExcelData> outPutExcelDatas = new List<OutPutExcelData>();
                    int step = 1;
                    bool isBestScore = sportProjectInfos.BestScoreMode == 0;
                    foreach (var dpInfo in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = freeSql.Select<ResultInfos>().Where(a => a.PersonId== dpInfo.Id.ToString() && a.IsRemoved == 0).ToList();
                        if (resultInfos.Count == 0 && !isAllTest) continue;
                        OutPutExcelData opd = new OutPutExcelData();
                        opd.Id = step;
                        opd.examTime = dpInfo.CreateTime;
                        opd.School = dpInfo.SchoolName;
                        opd.GradeName = dpInfo.GradeName;
                        opd.ClassName = dpInfo.ClassNumber;
                        opd.Name = dpInfo.Name;
                        opd.Sex = dpInfo.Sex == 0 ? "男" : "女";
                        opd.IdNumber = dpInfo.IdNumber;
                        opd.GroupName = dpInfo.GroupName;
                        int state = 0;
                        double MaxScore = 99999;
                        if (isBestScore) MaxScore = 0;
                        foreach (var ri in resultInfos)
                        {
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
                            else if (ri.State> 0)
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
                        }
                        if (state < 0) continue;
                        if (state != 1)
                        {
                            MaxScore = 0;
                            opd.Result = ResultStateType.Match(state);
                        }
                        else
                        {
                            opd.Result = MaxScore.ToString();
                        }
                        outPutExcelDatas.Add(opd);
                        step++;
                    }
                    //result = ExcelUtils.OutPutExcel(ldic, path);
                    MiniExcel.SaveAs(path, outPutExcelDatas);
                    isResult = true;
                }
                return isResult;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }
            finally
            {
                loading.Invoke((EventHandler)delegate { loading.Close(); });
                loading.Dispose();
            }
        }

        public  bool  SetExporWindowData(string groupName)
        {
             
                ExportGradeWindow = new ExportGradeWindow();
                ExportGradeWindow.groupName = groupName;
                var b=  ExportGradeWindow.ShowDialog();
                if (b==DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
             
        }
    }
}

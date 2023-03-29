using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using WeightCore.GameSystem.GameWindowSys;
using WeightCoreModel.GameModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WeightCore.GameSystem.GameWindow
{
    public partial class FromRoundWindow : HZH_Controls.Forms.FrmWithOKCancel2
    {
        public FromRoundWindow()
        {
            InitializeComponent();
        }

        public int mode = -1;

        private SportProjectInfos sportProjectInfos = null;
        public DbPersonInfos dbPersonInfos = null;
        public string _idnumber = string.Empty;
        bool isNoExam = false;
        IFreeSql fsql = FreeSqlHelper.Sqlite;

        private void FromRoundWindow_Load(object sender, EventArgs e)
        {
            if (mode == 0) this.Title = "轮次清空";
            else if (mode == 1) this.Title = "修正成绩";
            if (!string.IsNullOrEmpty(_idnumber))
            {
                dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.IdNumber == _idnumber).ToOne();
            }

            sportProjectInfos = fsql.Select<SportProjectInfos>().ToOne();
            if (sportProjectInfos != null)
            {
                int roundTotal = sportProjectInfos.RoundCount;
                for (int i = 0; i < roundTotal; i++)
                {
                    comboBox1.Items.Add($"第{i + 1}轮");
                }

                if (roundTotal > 0) comboBox1.SelectedIndex = 0;
            }

            if (dbPersonInfos != null)
            {
                textBox1.Text = dbPersonInfos.IdNumber;
                textBox2.Text = dbPersonInfos.Name;
                
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int roundid = comboBox1.SelectedIndex + 1;
            if (roundid <= 0) return;
            List<ResultInfos> resultInfos = fsql.Select<ResultInfos>()
                .Where(a => a.PersonIdNumber == _idnumber)
                .Where(a => a.RoundId == roundid)
                .Where(a => a.IsRemoved == 0)
                .ToList();
            isNoExam = false;
            textBox3.Text = "";
            if (resultInfos.Count == 0)
            {
                isNoExam = true;
                MessageBox.Show("该学生本轮未参加考试");
                return;
            }
            List<String> list = new List<String>();
            foreach (var ri in resultInfos)
            {
                if (ri.IsRemoved == 0)
                {
                    list.Add(ri.Result.ToString());
                    // textBox3.Text = ri.Result.ToString();
                }
            }
                if (list.Count == 3)
                {
                    textBox3.Text = list[0];
                    textBox4.Text= list[1];
                    textBox5.Text= list[2];
                }
                else
                {
                    MessageBox.Show("数据错误！！");
                }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromRoundWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
           if (base.DialogResult == DialogResult.OK)
           {
               int roundid = comboBox1.SelectedIndex + 1;
               if (mode == 0)
               {
                   int result = fsql.Delete<ResultInfos>()
                       .Where(a => a.PersonIdNumber == _idnumber)
                       .Where(a => a.RoundId == roundid)
                       .ExecuteAffrows();
                   if (result == 1) UIMessageBox.ShowSuccess("删除成功");
               }
               else if (mode == 1)
               {
                   double.TryParse(textBox3.Text, out double fhl);

                   if (isNoExam)
                   {
                       List<ResultInfos> insertResults = new List<ResultInfos>();
                       fsql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                       maxSortId++;
                       ResultInfos rinfo = new ResultInfos();
                       rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                       rinfo.SortId = maxSortId;
                       rinfo.PersonId = dbPersonInfos.Id.ToString();
                       rinfo.SportItemType = 0;
                       rinfo.PersonName = dbPersonInfos.Name;
                       rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                       rinfo.RoundId = roundid;
                       rinfo.State = 1;
                       rinfo.IsRemoved = 0;
                       rinfo.Result = fhl;
                       insertResults.Add(rinfo);

                       int result = fsql.InsertOrUpdate<ResultInfos>()
                           .SetSource(insertResults)
                           .IfExistsDoNothing()
                           .ExecuteAffrows();
                       if (result == 1) UIMessageBox.ShowSuccess("修改成功");
                   }
                   else
                   {

                       int result = fsql.Update<ResultInfos>()
                           .Set(a => a.Result == fhl)
                           .Where(a => a.PersonIdNumber == _idnumber)
                           .Where(a => a.RoundId == roundid)
                           .Where(a => a.IsRemoved == 0)
                           .ExecuteAffrows();
                  
                       if (result == 1) UIMessageBox.ShowSuccess("修改成功");
                   }
               }
           }
        }
    }
}

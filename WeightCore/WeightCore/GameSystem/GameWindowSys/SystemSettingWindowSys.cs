using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightCore.GameSystem.GameWindow;
using WeightCoreModel.GameModel;

namespace WeightCore.GameSystem.GameWindowSys
{
    public  class SystemSettingWindowSys
    {
        public static SystemSettingWindowSys Instance;
        SystemSettingWindow systemSettingWindow= null;
        IFreeSql freeSql = FreeSqlHelper.Sqlite;
        public void Awake()
        { 
            Instance= this;
        }

        public void ShowSystemSettingWindow()
        {
             systemSettingWindow= new SystemSettingWindow();
             systemSettingWindow.Show();
        }

        public void LoadingInitData(UITextBox uiTextBox1, UIComboBox uiComboBox1, UIComboBox uiComboBox2, UIComboBox uiComboBox3, UIComboBox uiComboBox4, ref SportProjectInfos sportProjectInfos)
        {
            sportProjectInfos = freeSql.Select<SportProjectInfos>().ToOne();
            if (sportProjectInfos != null)
            {
                uiTextBox1.Text = sportProjectInfos.Name;
                uiComboBox1.SelectedIndex = sportProjectInfos.RoundCount;
                uiComboBox2.SelectedIndex = sportProjectInfos.BestScoreMode;
                uiComboBox3.SelectedIndex = sportProjectInfos.TestMethod;
                uiComboBox4.SelectedIndex = sportProjectInfos.FloatType;
            }
            else
            {
                return;
            }
        }

        public void UpdataRoundCount(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.RoundCount == index).Where("1=1").ExecuteAffrows();
        }

        public void UpdataBestScoreMode(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.BestScoreMode == index).Where("1=1").ExecuteAffrows();
        }

        public void UpDataTestMethod(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.TestMethod == index).Where("1=1").ExecuteAffrows();
        }

        public void UpdataFloatType(int index)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.FloatType == index).Where("1=1").ExecuteAffrows();
        }

        public  void SetFrmClosedData(string name)
        {
            freeSql.Update<SportProjectInfos>().Set(a => a.Name == name).Where("1=1").ExecuteAffrows();
        }
    }
}

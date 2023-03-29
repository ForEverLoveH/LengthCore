using Serilog.Filters;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameWindow;
using WeightCoreModel.GameModel;

namespace WeightCore.GameSystem.GameWindowSys
{
    public class RunningMachineSettingWindowSys
    {
        public static RunningMachineSettingWindowSys Instance;
        private RunningMachineSettingWindow runningMachineSettingWindow = null;
        public bool  ShowRunningMachineSettingWindow()
        {
            runningMachineSettingWindow = new RunningMachineSettingWindow();
            var dis=  runningMachineSettingWindow.ShowDialog();
            if(dis==System.Windows.Forms.DialogResult.OK)
            {
                return true ; 
            }
            else
            {
                return false ;
            }
        }
        public void Awake()
        {
            Instance = this;
        }
        private static int machine = 0;
        private static string protNames = string.Empty;

        public  void SaveData(UIComboBox uiComboBox2, UIComboBox uiComboBox1, ref int machineCount, ref string portName)
        {
            int.TryParse(uiComboBox1.Text, out machineCount);
            if (machineCount == 0) { machineCount = 5; }

            portName = uiComboBox2.Text;
            machine = machineCount;
            protNames = portName;
        }
        public int GetMachineCount()
        {
            return machine;
        }

        public string GetPortName()
        {
            return protNames;
        }

        
        
    }
}

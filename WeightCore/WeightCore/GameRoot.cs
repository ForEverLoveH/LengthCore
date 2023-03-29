using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameHelper;
using WeightCore.GameSystem.GameWindow;
using WeightCore.GameSystem.GameWindowSys;

namespace WeightCore
{
    public  class GameRoot
    {
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern void SetForegroundWindow(IntPtr mainWindowHandle);
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private static WriteLoggHelper WriteLoggHelper  = new WriteLoggHelper();
        private static MainWindowSys MainWindowSys  = new MainWindowSys();
        private static PersonImportWindowSys PersonImportWindowSys = new PersonImportWindowSys();
       // private static LoadingWindowSys LoadingWindowSys = new LoadingWindowSys ();
        private static SystemSettingWindowSys  SystemSettingWindowSys = new SystemSettingWindowSys();
        private  static PlatFormSettingWindowSys PlatFormSettingWindowSys = new PlatFormSettingWindowSys();
        
        private static RunningMachineSettingWindowSys RunningMachineSettingWindowSys = new RunningMachineSettingWindowSys();
        private static RunningTestingWindowSys RunningTestingWindowSys= new RunningTestingWindowSys();
        private static ExportGradeWindowSys ExportGradeWindowSys = new ExportGradeWindowSys();
        public void StartGame()
        {
            Awake();
        }

        private void Awake()
        {
            WriteLoggHelper.Awake();
            MainWindowSys.Awake();
            PersonImportWindowSys  .Awake();
            //LoadingWindowSys.Awake();
            SystemSettingWindowSys.Awake();
            PlatFormSettingWindowSys.Awake();
            RunningTestingWindowSys.Awake();
            RunningMachineSettingWindowSys.Awake();
           // FromRoundWindowSys.Awake();
            ExportGradeWindowSys.Awake();   
        }

        public static string GetExceptionMsg(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" ");
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
                sb.AppendLine("【异常方法】：" + ex.TargetSite);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + ex.Message);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }
    }
}

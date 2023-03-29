using MiniExcelLibs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightCoreModel.GameModel
{
    public class OutPutPrintExcelData
    {
        [ExcelColumnName("序号",null)]
        public int Id { get; set; }
        [ExcelColumnName("姓名", null)]
        public string Name { get; set; }
        [ExcelColumnName("性别", null)]
        public string Sex{ get; set; }
        [ExcelColumnName("准考证号", null)]
        public string ExamNum { get; set; }
        [ExcelColumnName("组别", null)]
        public string groupName{ get; set; }
        [ExcelColumnName("身高(CM)", null)]
        public  string Result1{ get; set; }
        [ExcelColumnName("体重(KG)", null)]
        public string Result2 { get; set; }
        [ExcelColumnName("BMI", null)]
        public string Result3 { get; set; }


    }
}

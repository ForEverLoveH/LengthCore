using System.Collections.Generic;

namespace WeightCoreModel.GameModel
{
    public class GroupItems
    {
        public string GroupID
        {
            get;
            set;
        }
        public  string GroupName { get; set; }
        public List<StudentInfosItem> StudentInfos { get; set; }
    }

    
}
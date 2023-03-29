using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeightCore.GameSystem.GameHelper
{
    public class ListViewHelper
    {
        public static void AutoResizeColumWidths(ListView listView)
        {
            int col = listView.Columns.Count;
            int width = 0;
            Graphics graphics = listView.CreateGraphics();
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            for (int i = 0; i < col; i++)
            {
                string st = listView.Columns[i].Text;
                width = listView.Columns[i].Width;
                foreach (ListViewItem Item in listView.Items)
                {
                    st = Item.SubItems[i].Text;
                    int wids = (int)graphics.MeasureString(st, listView.Font).Width;
                    if (wids > width)
                    {
                        width = wids;
                    }
                    if (width <= 150)
                    {
                        listView.Columns[i].Width = width;
                    }
                    else
                    {
                        listView.Columns[i].Width = 100;
                    }
                }
            }
        }
    }
}

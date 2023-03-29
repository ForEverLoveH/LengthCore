namespace WeightCore.GameSystem.MyControl
{
    partial class UserControl1
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.state = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolState = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mScore1 = new Sunny.UI.UILabel();
            this.mScore2 = new Sunny.UI.UILabel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.mIdNumber = new Sunny.UI.UILabel();
            this.mName = new Sunny.UI.UILabel();
            this.mScore = new Sunny.UI.UILabel();
            this.roundCbx = new System.Windows.Forms.ComboBox();
            this.stateCbx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mtitle = new Sunny.UI.UILabel();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.state, this.toolState });
            this.statusStrip1.Location = new System.Drawing.Point(0, 264);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(197, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // state
            // 
            this.state.Name = "state";
            this.state.Size = new System.Drawing.Size(0, 17);
            // 
            // toolState
            // 
            this.toolState.Name = "toolState";
            this.toolState.Size = new System.Drawing.Size(68, 17);
            this.toolState.Text = "设备未连接";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mScore1);
            this.panel1.Controls.Add(this.mScore2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.mIdNumber);
            this.panel1.Controls.Add(this.mName);
            this.panel1.Controls.Add(this.mScore);
            this.panel1.Controls.Add(this.roundCbx);
            this.panel1.Controls.Add(this.stateCbx);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.mtitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(197, 264);
            this.panel1.TabIndex = 3;
            // 
            // mScore1
            // 
            this.mScore1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mScore1.Location = new System.Drawing.Point(66, 151);
            this.mScore1.Name = "mScore1";
            this.mScore1.Size = new System.Drawing.Size(100, 17);
            this.mScore1.TabIndex = 14;
            this.mScore1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mScore1.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // mScore2
            // 
            this.mScore2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mScore2.Location = new System.Drawing.Point(66, 177);
            this.mScore2.Name = "mScore2";
            this.mScore2.Size = new System.Drawing.Size(100, 17);
            this.mScore2.TabIndex = 13;
            this.mScore2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mScore2.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(13, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 14);
            this.label7.TabIndex = 12;
            this.label7.Text = "BMI:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(13, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "体重：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mIdNumber
            // 
            this.mIdNumber.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mIdNumber.Location = new System.Drawing.Point(66, 39);
            this.mIdNumber.Name = "mIdNumber";
            this.mIdNumber.Size = new System.Drawing.Size(100, 17);
            this.mIdNumber.TabIndex = 10;
            this.mIdNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mIdNumber.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // mName
            // 
            this.mName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.mName.Location = new System.Drawing.Point(66, 71);
            this.mName.Name = "mName";
            this.mName.Size = new System.Drawing.Size(100, 23);
            this.mName.TabIndex = 9;
            this.mName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mName.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // mScore
            // 
            this.mScore.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mScore.Location = new System.Drawing.Point(66, 117);
            this.mScore.Name = "mScore";
            this.mScore.Size = new System.Drawing.Size(100, 21);
            this.mScore.TabIndex = 8;
            this.mScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mScore.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // roundCbx
            // 
            this.roundCbx.FormattingEnabled = true;
            this.roundCbx.Location = new System.Drawing.Point(65, 206);
            this.roundCbx.Name = "roundCbx";
            this.roundCbx.Size = new System.Drawing.Size(101, 20);
            this.roundCbx.TabIndex = 7;
            // 
            // stateCbx
            // 
            this.stateCbx.FormattingEnabled = true;
            this.stateCbx.Items.AddRange(new object[] { "未测试", "已测试", "中退", "缺考", "犯规", "弃权" });
            this.stateCbx.Location = new System.Drawing.Point(65, 240);
            this.stateCbx.Name = "stateCbx";
            this.stateCbx.Size = new System.Drawing.Size(101, 20);
            this.stateCbx.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(8, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "状态：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 14);
            this.label4.TabIndex = 4;
            this.label4.Text = "轮次：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(8, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "身高：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "姓名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "考号：";
            // 
            // mtitle
            // 
            this.mtitle.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.mtitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mtitle.Location = new System.Drawing.Point(0, 0);
            this.mtitle.Name = "mtitle";
            this.mtitle.Size = new System.Drawing.Size(197, 23);
            this.mtitle.TabIndex = 0;
            this.mtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mtitle.ZoomScaleRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(197, 286);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label mScore3;
        private System.Windows.Forms.ToolStripStatusLabel state;
        private System.Windows.Forms.ToolStripStatusLabel toolState;
        private System.Windows.Forms.Panel panel1;
        private Sunny.UI.UILabel mScore1;
        private Sunny.UI.UILabel mScore2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private Sunny.UI.UILabel mIdNumber;
        private Sunny.UI.UILabel mName;
        private Sunny.UI.UILabel mScore;
        private System.Windows.Forms.ComboBox roundCbx;
        private System.Windows.Forms.ComboBox stateCbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Sunny.UI.UILabel mtitle;
    }
}

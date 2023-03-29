namespace PL_XKDemo.UserControls
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.mtitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mScore2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.mScore1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mIdNumber = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.stateCbx = new System.Windows.Forms.ComboBox();
            this.roundCbx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.mScore = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolState = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mtitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 46);
            this.panel1.TabIndex = 0;
            // 
            // mtitle
            // 
            this.mtitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.mtitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtitle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.mtitle.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mtitle.Location = new System.Drawing.Point(0, 0);
            this.mtitle.Name = "mtitle";
            this.mtitle.Size = new System.Drawing.Size(269, 46);
            this.mtitle.TabIndex = 2;
            this.mtitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.mScore2);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.mScore1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.mIdNumber);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.stateCbx);
            this.panel2.Controls.Add(this.roundCbx);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.mScore);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.mName);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.statusStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 46);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(269, 275);
            this.panel2.TabIndex = 1;
            // 
            // mScore2
            // 
            this.mScore2.AutoSize = true;
            this.mScore2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mScore2.ForeColor = System.Drawing.Color.Red;
            this.mScore2.Location = new System.Drawing.Point(88, 142);
            this.mScore2.Name = "mScore2";
            this.mScore2.Size = new System.Drawing.Size(0, 22);
            this.mScore2.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(16, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 22);
            this.label7.TabIndex = 13;
            this.label7.Text = "BMI：";
            // 
            // mScore1
            // 
            this.mScore1.AutoSize = true;
            this.mScore1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mScore1.ForeColor = System.Drawing.Color.Red;
            this.mScore1.Location = new System.Drawing.Point(88, 110);
            this.mScore1.Name = "mScore1";
            this.mScore1.Size = new System.Drawing.Size(0, 22);
            this.mScore1.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(16, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 22);
            this.label3.TabIndex = 11;
            this.label3.Text = "体重：";
            // 
            // mIdNumber
            // 
            this.mIdNumber.AutoSize = true;
            this.mIdNumber.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mIdNumber.Location = new System.Drawing.Point(87, 9);
            this.mIdNumber.Name = "mIdNumber";
            this.mIdNumber.Size = new System.Drawing.Size(0, 22);
            this.mIdNumber.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(15, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 22);
            this.label8.TabIndex = 9;
            this.label8.Text = "考号：";
            // 
            // stateCbx
            // 
            this.stateCbx.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stateCbx.FormattingEnabled = true;
            this.stateCbx.Items.AddRange(new object[] {
            "未测试",
            "已测试",
            "中退",
            "缺考",
            "犯规",
            "弃权"});
            this.stateCbx.Location = new System.Drawing.Point(79, 218);
            this.stateCbx.Name = "stateCbx";
            this.stateCbx.Size = new System.Drawing.Size(187, 30);
            this.stateCbx.TabIndex = 8;
            // 
            // roundCbx
            // 
            this.roundCbx.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.roundCbx.FormattingEnabled = true;
            this.roundCbx.Location = new System.Drawing.Point(79, 176);
            this.roundCbx.Name = "roundCbx";
            this.roundCbx.Size = new System.Drawing.Size(187, 30);
            this.roundCbx.TabIndex = 7;
            this.roundCbx.SelectedIndexChanged += new System.EventHandler(this.roundCbx_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(14, 222);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 22);
            this.label5.TabIndex = 6;
            this.label5.Text = "状态：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(15, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 22);
            this.label6.TabIndex = 5;
            this.label6.Text = "轮次：";
            // 
            // mScore
            // 
            this.mScore.AutoSize = true;
            this.mScore.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mScore.ForeColor = System.Drawing.Color.Red;
            this.mScore.Location = new System.Drawing.Point(87, 76);
            this.mScore.Name = "mScore";
            this.mScore.Size = new System.Drawing.Size(0, 22);
            this.mScore.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(15, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 22);
            this.label4.TabIndex = 3;
            this.label4.Text = "身高：";
            // 
            // mName
            // 
            this.mName.AutoSize = true;
            this.mName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mName.Location = new System.Drawing.Point(87, 43);
            this.mName.Name = "mName";
            this.mName.Size = new System.Drawing.Size(0, 22);
            this.mName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "姓名：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 253);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(269, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolState
            // 
            this.toolState.ForeColor = System.Drawing.Color.Red;
            this.toolState.Name = "toolState";
            this.toolState.Size = new System.Drawing.Size(68, 17);
            this.toolState.Text = "设备未连接";
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(269, 321);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolState;
        private System.Windows.Forms.Label mtitle;
        private System.Windows.Forms.ComboBox stateCbx;
        private System.Windows.Forms.ComboBox roundCbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label mScore;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label mName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label mIdNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label mScore1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label mScore2;
        private System.Windows.Forms.Label label7;
    }
}

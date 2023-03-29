﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeightCore.GameSystem.GameWindowSys;

namespace WeightCore.GameSystem.GameWindow
{
    public partial class RunningMachineSettingWindow : Form
    {
        public RunningMachineSettingWindow()
        {
            InitializeComponent();
        }
        public int machineCount = 0;
        public string portName = string.Empty;
        private void uiButton1_Click(object sender, EventArgs e)
        {
            RunningMachineSettingWindowSys.Instance.SaveData(uiComboBox2, uiComboBox1, ref machineCount, ref portName);
            DialogResult = DialogResult.OK;
            this.Close();
        }

        
    }        
}


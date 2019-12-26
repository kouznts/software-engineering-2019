﻿using System;
using System.Windows.Forms;

namespace GasStationMs.App.Forms
{
    public partial class CreatingTopologyForm : Form
    {
        private string currFilePath;

        public CreatingTopologyForm()
        {
            InitializeComponent();

            btnOpenConstructorForm.Enabled = false;

            SetupNudsSettings();
        }

        private void SetupNudsSettings()
        {
            nudChooseColsCount.Minimum = Topology.Topology.MinColsCount;
            nudChooseColsCount.Maximum = Topology.Topology.MaxColsCount;

            nudChooseRowsCount.Minimum = Topology.Topology.MinRowsCount;
            nudChooseRowsCount.Maximum = Topology.Topology.MaxRowsCount;
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            string dotExt = Topology.Topology.DotExt;
            string filter = " " + dotExt + "|" + "*" + dotExt;

            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = dotExt,
                Filter = filter
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                currFilePath = sfd.FileName;
                btnOpenConstructorForm.Enabled = true;
            }
        }

        private void btnOpenConstructorForm_Click(object sender, EventArgs e)
        {

        }
    }
}

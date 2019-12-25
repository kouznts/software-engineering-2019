﻿using System;
using System.Linq;
using System.Windows.Forms;
using GasStationMs.App.DistributionLaws;
using GasStationMs.App.Models;

namespace GasStationMs.App
{
    public partial class DistributionLawsForm : Form
    {
        public DistributionLawsForm()
        {
            InitializeComponent();

            nudDeterminedFlow.Minimum = TrafficFlow.MinTimeBetweenCarsInSeconds;
            nudDeterminedFlow.Maximum = TrafficFlow.MaxTimeBetweenCarsInSeconds;
        }
        #region Отображение ЗР

        private void distributionLaw_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (distributionLaw.SelectedIndex)
            {
                case 0:
                    uniformDistributionTimeLabel.Visible = true;
                    uniformDistributionTime.Visible = true;
                    uniformDistributionPanel.Visible = true;


                    break;

                case 1:
                    normalDistributionDispersion.Visible = true;
                    normalDistributionPredicted.Visible = true;
                    normalDistributionDispersionLabel.Visible = true;
                    normalDistributionPredictedLabel.Visible = true;
                    normalDistributionPanel.Visible = true;


                    break;

                case 2:
                    exponentialDistributionLambda.Visible = true;
                    exponentialDistributionLambdaLabel.Visible = true;
                    exponentialDistributionPanel.Visible = true;


                    break;

                default:
                    break;
            }
        }
        #endregion

        #region radiobuttons
        bool _isCheckedradioButtonDeterminedFlow = false;
        bool _isCheckedradioButtonRandomFlow = false;

        private void radioButtonDeterminedFlow_CheckedChanged(object sender, EventArgs e)
        {
            _isCheckedradioButtonDeterminedFlow = determinedFlow.Checked;
        }

        private void radioButtonDeterminedFlow_Click(object sender, EventArgs e)
        {
            if (determinedFlow.Checked && !_isCheckedradioButtonDeterminedFlow)
                determinedFlow.Checked = false;
            else
            {
                determinedFlow.Checked = true;
                _isCheckedradioButtonDeterminedFlow = false;
            }
        }

        private void radioButtonRandomFlow_CheckedChanged(object sender, EventArgs e)
        {
            _isCheckedradioButtonRandomFlow = randomFlow.Checked;
        }

        private void radioButtonRandomFlow_Click(object sender, EventArgs e)
        {
            if (randomFlow.Checked && !_isCheckedradioButtonRandomFlow)
                randomFlow.Checked = false;
            else
            {
                randomFlow.Checked = true;
                _isCheckedradioButtonRandomFlow = false;
            }
        }
        #endregion

        #region

        public IDistributionLaw Generator;
        private void buttonToModelling_Click(object sender, EventArgs e)
        {
            if (Controls.OfType<RadioButton>().Any(x => x.Checked))
            {
                var flow = Controls.OfType<RadioButton>().FirstOrDefault(x => x.Checked);

                if (flow.Name == "randomFlow")
                {
                    switch (distributionLaw.SelectedIndex)
                    {
                        case 0:
                            Generator = new UniformDistribution(0.1, (double)uniformDistributionTime.Value);
                            break;

                        case 1:
                            Generator = new NormalDistribution((double)normalDistributionPredicted.Value, (double)normalDistributionDispersion.Value);
                            break;

                        case 2:
                            Generator = new ExponentialDistribution((double)exponentialDistributionLambda.Value);
                            break;


                        default:
                            break;
                    }

                }
                else
                {
                    Generator = new DeterminedDistribution((double)nudDeterminedFlow.Value);
                }
                label4.Text = Generator.GetRandNumber().ToString();
            }

        }

        #endregion


        #region Методы обязательные для форм
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void exponentialDistributionLambdaLabel_Click(object sender, EventArgs e)
        {

        }

        private void exponentialDistributionLambda_ValueChanged(object sender, EventArgs e)
        {

        }
        private void label4_Click_1(object sender, EventArgs e)
        {

        }
        #endregion


    }
}

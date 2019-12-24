﻿using GasStationMs.App.Forms;
using System;
using System.Windows.Forms;
using GasStationMs.App.Modeling.Models.Views;

namespace GasStationMs.App.Modeling.Models.PictureBoxes
{
    internal class CarPictureBox : MoveablePictureBox
    {
        public CarPictureBox(ModelingForm modelingForm, CarView carView)
        {
            Tag = carView;
            Image = Properties.Resources.car_32x17__left;
            Location = DestinationPointsDefiner.SpawnPoint;
            SizeMode = PictureBoxSizeMode.AutoSize;

            IsGoesFilling = false;

            MouseClick += new MouseEventHandler(ClickEventProvider.CarPictureBox_Click);

            modelingForm.PlaygroundPanel.Controls.Add(this);
            BringToFront();
        }
    }
}
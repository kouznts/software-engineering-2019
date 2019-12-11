﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GasStationMs.App.Modeling.Models
{
    public class CarView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TankVolume { get; set; }
        public int FuelRemained { get; set; }

        public FuelView Fuel { get; set; }
        //public int? FuelTypeId { get; set; }
        //public string FuelTypeName { get; set; }

        public bool IsTruck { get; set; }
        public bool IsGoesFilling { get; set; }

        private List<Point> _destinationPoints;
        public PictureBox DestinationSpot;

        public CarView(int id, string name, int tankVolume, int fuelRemained,
            FuelView fuelView, bool isTruck, bool isGoesFilling)
        {
            Id = id;
            Name = name;
            TankVolume = tankVolume;
            FuelRemained = fuelRemained;
            Fuel = fuelView;
            IsTruck = isTruck;
            IsGoesFilling = isGoesFilling;

            _destinationPoints = new List<Point>();
        }

        public PictureBox CreateDestinationSpot(Point destPoint)
        {
            return this.DestinationSpot = new PictureBox()
            {
                Size = new Size(5, 5),
                Location = destPoint,
                Visible = true,
                BackColor = Color.DarkRed
            };
        }
        public void AddDestinationPoint(Point destPoint)
        {
            _destinationPoints.Add(destPoint);
        }

        public Point GetDestinationPoint()
        {
            return _destinationPoints.Count == 0 ? new Point(-1, -1) : _destinationPoints.Last();
        }

        public void RemoveDestinationPoint(Form form)
        {
            if (_destinationPoints.Count == 0)
            {
                return;
            }

            var lastAssignedPoint = _destinationPoints.Last();
            _destinationPoints.Remove(lastAssignedPoint);

            DeleteDestinationSpot(form);
        }

        private void DeleteDestinationSpot(Form form)
        {
            form.Controls.Remove(DestinationSpot);
            DestinationSpot.Dispose();
            DestinationSpot = null;
        }
    }
}
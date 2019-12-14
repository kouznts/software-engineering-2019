using System.Collections.Generic;
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

        public FuelModel Fuel { get; set; }
        //public int? FuelTypeId { get; set; }
        //public string FuelTypeName { get; set; }

        public bool IsTruck { get; set; }
        public bool IsGoesFilling { get; set; }
        public bool IsOnStation { get; set; }
        public bool IsFilled { get; set; }
        public bool IsFuelDispenserChosen { get; set; }
        public bool IsBypassingObject { get; set; }
        public PictureBox ChosenFuelDispenser { get; set; }

        public PictureBox SafeArea { get; set; }

        private List<Point> _destinationPoints;
        public PictureBox DestinationSpot;

        public CarView(int id, string name, int tankVolume, int fuelRemained,
            FuelModel fuelView, bool isTruck, bool isGoesFilling)
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
            var spotSize = 5;
            this.DestinationSpot = new PictureBox()
            {
                Size = new Size(spotSize, spotSize),
                Location = destPoint,
                Visible = true,
                BackColor = Color.DarkRed
            };

            this.DestinationSpot.BringToFront();

            return DestinationSpot;
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

        public void DeleteDestinationSpot(Form form)
        {
            form.Controls.Remove(DestinationSpot);
            DestinationSpot.Dispose();
            DestinationSpot = null;
        }

        public bool HasDestPoints()
        {
            return _destinationPoints.Count > 0;
        }

        public void CreateSafeArea(PictureBox car, int carSpeed)
        {
            var safeAreaSizeX = car.Width + 2 * carSpeed;
            var safeAreaSizeY = car.Height + 2 * carSpeed;
            SafeArea = new PictureBox();
            SafeArea.Tag = "Safe area of car " + Id;
            SafeArea.BackColor = Color.Azure;;
            SafeArea.Left = car.Left - carSpeed;
            SafeArea.Top = car.Top - carSpeed;
            SafeArea.Size = new Size(safeAreaSizeX, safeAreaSizeY);
        }
    }
}

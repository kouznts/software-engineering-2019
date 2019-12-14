using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GasStationMs.App.Modeling.Models;

namespace GasStationMs.App
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public partial class ModelingForm : Form
    {
        private int _timerTicksCount = 0;

        private int _carSpeedNoFilling = 4;
        private int _carSpeedFilling = 3;

        //private bool _isGoHorizontal;
        //private bool _isGoVertical;

        private bool _paused;
        private readonly Random _rnd = new Random();

        #region TopologyElements

        private PictureBox _cashCounter;
        private PictureBox _enter;
        private PictureBox _exit;
        private List<PictureBox> _fuelDispensersList = new List<PictureBox>();
        private List<PictureBox> _fuelTanksList = new List<PictureBox>();

        #endregion /TopologyElements

        #region DestinationPoints

        private Dictionary<PictureBox, Point> _fuelDispensersDestPoints = new Dictionary<PictureBox, Point>();
        private List<Point> _predeterminedPoints = new List<Point>();

        private int _fuelingPointDeltaX = 5;
        private int _fuelingPointDeltaY = 10;

        private int _noFillingHorizontalLine;
        private int _filledHorizontalLine;

        private int _rightPlaygroundBorder;
        private int _leftPlaygroundBorder;
        private int _leftCarDestroyingEdge;

        // Spawning/destroying car destination points
        private Point _spawnPoint;
        private Point _leavePointNoFilling;
        private Point _leavePointFilled;

        // Destination points to enter/leave gas station
        private Point _enterCenter;
        private Point _exitCenter;

        private Point _enterPoint1;
        private Point _enterPoint2;
        private Point _enterPoint3;

        private Point _exitPoint1;
        private Point _exitPoint2;

        #endregion /DestinationPoints

        public ModelingForm()
        {
            InitializeComponent();

            this.Controls.Remove(pictureBoxCashCounter);
            this.Controls.Remove(pictureBoxCar);
            this.Controls.Remove(pictureBoxEnter);
            this.Controls.Remove(pictureBoxExit);
            this.Controls.Remove(pictureBoxFuelDispenser1);
            this.Controls.Remove(pictureBoxFuelDispenser2);
            this.Controls.Remove(pictureBoxFuelTank1);
            this.Controls.Remove(pictureBoxFuelTank2);

            this.DoubleBuffered = true;

            MapTopology();
        }

        private void TimerModeling_Tick(object sender, EventArgs e)
        {
            _timerTicksCount++;

            //if (!_paused)
            //{
            //    //return;
            //    SpawnCar();
            //    _paused = true;
            //}

            if (_timerTicksCount % 15 == 0)
            {
                SpawnCar();
            }

            #region LoopingControls

            foreach (Control c in this.Controls)
            {
                if (!(c is PictureBox) || c.Tag == null)
                {
                    continue;
                }

                var pictureBox = (PictureBox) c;

                // Car
                if (pictureBox.Tag is CarView)
                {
                    var car = pictureBox;
                    var carView = (CarView) car.Tag;

                    RouteCar(car);

                    MoveCarToDestination(car);
                }
            }

            #endregion /LoopingControls
        }

        #region CarLogic

        private void SpawnCar( /*CarModel carModel*/)
        {
            var carView = CreateCarView( /*caeModel*/);

            // Some Distribution law here
            if (_rnd.NextDouble() >= 0.5)
            {
                carView.IsGoesFilling = true;
            }
            //carView.IsGoesFilling = true;


            if (!carView.IsGoesFilling)
            {
                carView.AddDestinationPoint(_leavePointNoFilling);
            }

            CreateCarPictureBox(carView);
        }

        private void RouteCar(PictureBox car)
        {
            var carView = (CarView) car.Tag;

            if (!carView.IsGoesFilling)
            {
                return;
            }

            var isOnStation = carView.IsOnStation;
            var isFilled = carView.IsFilled;
            var isFuelDispenserChosen = carView.IsFuelDispenserChosen;

            // New car
            if (!isOnStation && !isFilled && !carView.HasDestPoints())
            {
                GoToEnter(carView);
            }

            // Just entered the station
            if (isOnStation && !isFuelDispenserChosen)
            {
                ChooseFuelDispenser(carView);
            }

            // After filling 
            if (isOnStation && isFilled)
            {
                GoToExit(carView);
                carView.IsOnStation = false;
            }
        }

        private void MoveCarToDestination(PictureBox car)
        {
            var carView = (CarView) car.Tag;
            //if (carView.IsBypassingObject)
            //{
            //    var x = 1;
            //}

            var destPoint = carView.GetDestinationPoint();
            PictureBox destSpot = carView.DestinationSpot;

            var carSpeed = carView.IsGoesFilling ? _carSpeedFilling : _carSpeedNoFilling;

            #region MotionLogic


            destPoint = MoveCar(car, destPoint, carSpeed);


            #endregion /MotionLogic


            if (carView.DestinationSpot == null)
            {
                destSpot = carView.CreateDestinationSpot(destPoint);
                this.Controls.Add(destSpot);
            }

            if (car.Bounds.IntersectsWith(destSpot.Bounds))
            {
                carView.RemoveDestinationPoint(this);

                carView.IsBypassingObject = false;
                //_isGoHorizontal = false;
                //_isGoVertical = false;

                if (destPoint.Equals(_enterPoint3))
                {
                    carView.IsOnStation = true;
                }

                foreach (var fuelDispensersDestPoint in _fuelDispensersDestPoints.Values)
                {
                    if (destPoint.Equals(fuelDispensersDestPoint))
                    {
                        //StartFilling();
                        //test
                        carView.IsFilled = true;
                    }
                }

                if (destPoint.Equals(_exitPoint1))
                {
                    carView.IsOnStation = false;
                }

                if (destPoint.Equals(_leavePointNoFilling) || destPoint.Equals(_leavePointFilled))
                {
                    this.Controls.Remove(car);
                    car.Dispose();
                }
            }
        }

        private Point MoveCar(PictureBox car, Point destPoint, int carSpeed)
        {
            var isHorizontalMoving = false;
            var isVerticalMoving = false;
            var carView = (CarView) car.Tag;
            if (!carView.IsBypassingObject)
            {
                if (!carView.IsFilled)
                {
                    // Go Up
                    if (car.Top >= destPoint.Y && !isHorizontalMoving)
                    {
                        car.Top -= carSpeed;
                        //car.Image = Properties.Resources.car_32x17__up;
                        isVerticalMoving = true;
                        destPoint = PreventIntersection(car, Direction.Up);
                    }

                    // Go Down
                    if (car.Bottom <= destPoint.Y && !isHorizontalMoving)
                    {
                        car.Top += carSpeed;
                        //car.Image = Properties.Resources.car_64x34__down;
                        isVerticalMoving = true;
                        destPoint = PreventIntersection(car, Direction.Down);
                    }

                    // Go left
                    if (car.Left >= destPoint.X && !isVerticalMoving)
                    {
                        car.Left -= carSpeed;
                        //car.Image = Properties.Resources.car_32x17__left;
                        destPoint = PreventIntersection(car, Direction.Left);
                    }

                    // Go Right
                    if (car.Right <= destPoint.X && !isVerticalMoving)
                    {
                        car.Left += carSpeed;
                        //car.Image = Properties.Resources.car_32x17__right;
                    }
                }
                else
                {
                    // Go left
                    if (car.Left >= destPoint.X)
                    {
                        car.Left -= carSpeed;
                        //car.Image = Properties.Resources.car_32x17__left;
                        isHorizontalMoving = true;
                        destPoint = PreventIntersection(car, Direction.Left);
                    }

                    // Go Right
                    if (car.Right <= destPoint.X)
                    {
                        car.Left += carSpeed;
                        //car.Image = Properties.Resources.car_32x17__right;
                        isHorizontalMoving = true;
                    }

                    // Go Up
                    if (car.Top >= destPoint.Y && !isHorizontalMoving)
                    {
                        car.Top -= carSpeed;
                        //car.Image = Properties.Resources.car_32x17__up;
                        destPoint = PreventIntersection(car, Direction.Up);
                    }

                    // Go Down
                    if (car.Bottom <= destPoint.Y && !isHorizontalMoving)
                    {
                        car.Top += carSpeed;
                        //car.Image = Properties.Resources.car_32x17__down;
                        destPoint = PreventIntersection(car, Direction.Down);
                    }
                }
            }
            else
            {
                // Go left
                if (car.Left >= destPoint.X)
                {
                    car.Left -= carSpeed;
                    //car.Image = Properties.Resources.car_32x17__left;
                    destPoint = PreventIntersection(car, Direction.Left);
                }

                // Go Right
                if (car.Right <= destPoint.X)
                {
                    car.Left += carSpeed;
                    //car.Image = Properties.Resources.car_32x17__right;
                }

                // Go Up
                if (car.Top >= destPoint.Y)
                {
                    car.Top -= carSpeed;
                    //car.Image = Properties.Resources.car_32x17__up;
                    destPoint = PreventIntersection(car, Direction.Up);
                }

                // Go Down
                if (car.Bottom <= destPoint.Y)
                {
                    car.Top += carSpeed;
                    //car.Image = Properties.Resources.car_32x17__down;

                    destPoint = PreventIntersection(car, Direction.Down);
                }
            }

            return destPoint;
        }

        #region MoveCar1

        //private Point MoveCar1(PictureBox car, Point destPoint, int carSpeed)
        //{
        //    var isHorizontalMoving = false;
        //    var isVerticalMoving = false;
        //    var carView = (CarView)car.Tag;
        //    if (!carView.IsBypassingObject)
        //    {
        //        if (!carView.IsFilled)
        //        {
        //            // Go Up
        //            if (car.Top >= destPoint.Y && !isHorizontalMoving)
        //            {
        //                car.Top -= carSpeed;
        //                car.Image = Properties.Resources.car_32x17__up;
        //                isVerticalMoving = true;
        //                destPoint = PreventIntersection(car, 0, destPoint);
        //            }

        //            // Go Down
        //            if (car.Bottom <= destPoint.Y && !isHorizontalMoving)
        //            {
        //                car.Top += carSpeed;
        //                car.Image = Properties.Resources.car_64x34__down;
        //                isVerticalMoving = true;
        //            }

        //            // Go left
        //            if (car.Left >= destPoint.X && !isVerticalMoving)
        //            {
        //                car.Left -= carSpeed;
        //                car.Image = Properties.Resources.car_32x17__left;
        //                destPoint = PreventIntersection(car, 3, destPoint);
        //            }

        //            // Go Right
        //            if (car.Right <= destPoint.X && !isVerticalMoving)
        //            {
        //                car.Left += carSpeed;
        //                car.Image = Properties.Resources.car_32x17__right;
        //            }
        //        }
        //        else
        //        {
        //            // Go left
        //            if (car.Left >= destPoint.X)
        //            {
        //                car.Left -= carSpeed;
        //                car.Image = Properties.Resources.car_32x17__left;
        //                isHorizontalMoving = true;
        //                destPoint = PreventIntersection(car, 3, destPoint);
        //            }

        //            // Go Right
        //            if (car.Right <= destPoint.X)
        //            {
        //                car.Left += carSpeed;
        //                car.Image = Properties.Resources.car_32x17__right;
        //                isHorizontalMoving = true;
        //            }

        //            // Go Up
        //            if (car.Top >= destPoint.Y && !isHorizontalMoving)
        //            {
        //                car.Top -= carSpeed;
        //                car.Image = Properties.Resources.car_32x17__up;
        //                destPoint = PreventIntersection(car, 0, destPoint);
        //            }

        //            // Go Down
        //            if (car.Bottom <= destPoint.Y && !isHorizontalMoving)
        //            {
        //                car.Top += carSpeed;
        //                car.Image = Properties.Resources.car_32x17__down;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Go left
        //        if (car.Left >= destPoint.X && !_isGoVertical)
        //        {
        //            car.Left -= carSpeed;
        //            car.Image = Properties.Resources.car_32x17__left;
        //            destPoint = PreventIntersection(car, 3, destPoint);
        //            _isGoHorizontal = true;
        //            _isGoVertical = false;

        //            return destPoint;

        //        }

        //        // Go Right
        //        if (car.Right <= destPoint.X && !_isGoVertical)
        //        {
        //            car.Left += carSpeed;
        //            car.Image = Properties.Resources.car_32x17__right;
        //            isHorizontalMoving = true;
        //            _isGoVertical = false;

        //            return destPoint;

        //        }

        //        // Go Up
        //        if (car.Top >= destPoint.Y && !_isGoHorizontal)
        //        {
        //            car.Top -= carSpeed;
        //            car.Image = Properties.Resources.car_32x17__up;

        //            destPoint = PreventIntersection(car, 0, destPoint);

        //            _isGoHorizontal = false;
        //            _isGoVertical = true;

        //            return destPoint;

        //        }

        //        // Go Down
        //        if (car.Bottom <= destPoint.Y && !_isGoHorizontal)
        //        {
        //            car.Top += carSpeed;
        //            car.Image = Properties.Resources.car_32x17__down;
        //            _isGoHorizontal = false;
        //            _isGoVertical = true;

        //            return destPoint;

        //        }

        //        _isGoVertical = false;
        //        _isGoHorizontal = false;
        //    }

        //    return destPoint;
        //}

        #endregion /MoveCar1

        private void ChooseFuelDispenser(CarView car)
        {
            PictureBox optimalFuelDispenser = _fuelDispensersList[0];
            FuelDispenserView fuelDispenserView = (FuelDispenserView) optimalFuelDispenser.Tag;
            var minQueue = fuelDispenserView.CarsInQueue;

            // Looking for Fuel Dispenser with minimal queue
            foreach (var fuelDispenser in _fuelDispensersList)
            {
                fuelDispenserView = (FuelDispenserView) fuelDispenser.Tag;
                if (fuelDispenserView.CarsInQueue < minQueue)
                {
                    minQueue = fuelDispenserView.CarsInQueue;
                    optimalFuelDispenser = fuelDispenser;
                }
            }

            car.ChosenFuelDispenser = optimalFuelDispenser;
            fuelDispenserView = (FuelDispenserView) optimalFuelDispenser.Tag;
            fuelDispenserView.CarsInQueue++;
            car.IsFuelDispenserChosen = true;

            var destPointX = optimalFuelDispenser.Left + _fuelingPointDeltaX;
            var destPointY = optimalFuelDispenser.Bottom + _fuelingPointDeltaY;
            var destPoint = new Point(destPointX, destPointY);

            // The main point of fueling
            car.AddDestinationPoint(destPoint);

            // Additional points for better graphics
            destPointX = optimalFuelDispenser.Right + _fuelingPointDeltaX - 1;
            destPoint = new Point(destPointX, destPointY + 30);
            car.AddDestinationPoint(destPoint);
        }

        private void GoToEnter(CarView car)
        {
            car.AddDestinationPoint(_enterPoint3);
            car.AddDestinationPoint(_enterPoint2);
            car.AddDestinationPoint(_enterPoint1);
        }

        private void GoToExit(CarView car)
        {
            car.AddDestinationPoint(_leavePointFilled);
            car.AddDestinationPoint(_exitPoint2);
            car.AddDestinationPoint(_exitPoint1);
        }

        private Point PreventIntersection(PictureBox activeCar, Direction direction)
        {
            var activeCarView = (CarView) activeCar.Tag;
            var destPoint = activeCarView.GetDestinationPoint();

            foreach (Control c in this.Controls)
            {
                if (!(c is PictureBox) || c.Tag == null || c == activeCar)
                {
                    continue;
                }

                var pictureBox = (PictureBox) c;

                if (!activeCar.Bounds.IntersectsWith(pictureBox.Bounds))
                {
                    continue;
                }

                // Another Car
                if (pictureBox.Tag is CarView)
                {
                    var anotherCar = pictureBox;

                    switch (direction)
                    {
                        case Direction.Up:
                        {
                            activeCar.Top = anotherCar.Bottom;

                            break;
                        }

                        case Direction.Right:
                        {
                            activeCar.Left = anotherCar.Left - activeCar.Width;

                            break;
                        }

                        case Direction.Down:
                        {
                            activeCar.Top = anotherCar.Top - activeCar.Height;

                            break;
                        }

                        case Direction.Left:
                        {
                            activeCar.Left = anotherCar.Right;

                            break;
                        }
                    }
                }


                // Fuel Dispenser
                if (pictureBox.Tag is FuelDispenserView || pictureBox.Tag is CashCounterView)
                {
                    var fuelDispenser = pictureBox;

                    bool bypassFromLeft = false;
                    bool bypassFromRight = false;
                    bool bypassFromBottom = false;
                    bool bypassFromTop = false;

                    int newDestX;
                    int newDestY;

                    Point newDestinationPoint1;
                    Point newDestinationPoint2;
                    Point newDestinationPoint3;

                    switch (direction)
                    {
                        case Direction.Up:
                        {
                            activeCar.Top = fuelDispenser.Bottom;

                            if (!activeCarView.IsBypassingObject)
                            {
                                activeCarView.IsBypassingObject = true;
                                // choose where to bypass 
                                newDestX = destPoint.X < activeCar.Left
                                    ? fuelDispenser.Left - (activeCar.Width + 5)
                                    : fuelDispenser.Right + (activeCar.Width + 5);


                                if (destPoint.X < activeCar.Left)
                                {
                                    newDestX = fuelDispenser.Left - (activeCar.Width + 5);
                                    bypassFromLeft = true;
                                }
                                else
                                {
                                    newDestX = fuelDispenser.Right + (activeCar.Width + 5);
                                    bypassFromRight = true;
                                }

                                newDestY = fuelDispenser.Bottom + 10;


                                newDestinationPoint1 = new Point(newDestX,
                                    newDestY);

                                newDestY = fuelDispenser.Top + activeCar.Height + 10;
                                newDestinationPoint2 = new Point(newDestX,
                                    newDestY);

                                activeCarView.DeleteDestinationSpot(this);
                                activeCarView.AddDestinationPoint(newDestinationPoint2);
                                activeCarView.AddDestinationPoint(newDestinationPoint1);
                            }

                            break;
                        }

                        case Direction.Right:
                        {
                            break;
                        }

                        case Direction.Down:
                        {
                            activeCar.Top = fuelDispenser.Top - activeCar.Height;

                            if (!activeCarView.IsBypassingObject)
                            {
                                activeCarView.IsBypassingObject = true;


                                // choose where to bypass 
                                newDestX = fuelDispenser.Right + 10;

                                if (destPoint.X <= fuelDispenser.Left + fuelDispenser.Width / 2)
                                {
                                    newDestX = fuelDispenser.Left - (activeCar.Width + 5);
                                    bypassFromLeft = true;
                                }
                                else
                                {
                                    newDestX = fuelDispenser.Right + (activeCar.Width + 5);
                                    bypassFromRight = true;
                                }

                                newDestY = fuelDispenser.Top - 10;

                                newDestinationPoint1 = new Point(newDestX,
                                    newDestY);

                                newDestY = fuelDispenser.Bottom + activeCar.Height + 10;
                                newDestinationPoint2 = new Point(newDestX,
                                    newDestY);

                                activeCarView.DeleteDestinationSpot(this);
                                activeCarView.AddDestinationPoint(newDestinationPoint2);
                                activeCarView.AddDestinationPoint(newDestinationPoint1);
                            }

                            break;
                        }

                        case Direction.Left:
                        {
                            activeCar.Left = fuelDispenser.Right;

                            if (!activeCarView.IsBypassingObject)
                            {
                                activeCarView.IsBypassingObject = true;

                                // choose where to bypass 
                                newDestX = fuelDispenser.Right + 10;

                                if (destPoint.Y <= fuelDispenser.Bottom)
                                {
                                    newDestY = fuelDispenser.Top - (activeCar.Width + 5);
                                    bypassFromTop = true;
                                }
                                else
                                {
                                    newDestY = fuelDispenser.Bottom + (activeCar.Width + 1);
                                    bypassFromBottom = true;
                                }

                                newDestinationPoint1 = new Point(newDestX,
                                    newDestY);

                                newDestX = fuelDispenser.Left + fuelDispenser.Width / 2;
                                newDestinationPoint2 = new Point(newDestX,
                                    newDestY);

                                newDestinationPoint3 = new Point(fuelDispenser.Left - 20,
                                    destPoint.Y - 20);

                                activeCarView.DeleteDestinationSpot(this);
                                //activeCarView.AddDestinationPoint(newDestinationPoint3);
                                activeCarView.AddDestinationPoint(newDestinationPoint2);
                                activeCarView.AddDestinationPoint(newDestinationPoint1);
                            }

                            break;
                        }
                    }
                }
            }

            return activeCarView.GetDestinationPoint();
        }

        #endregion /CarLogic

        #region TopologyMappingLogic

        private void MapTopology( /*int[][] topology*/)
        {
            //CreateCashCounter();
            //CreateEnter();
            //CreateExit();
            //CreateFuelDispenser();
            //CreateFuelTank();

            #region CashCounter

            //_cashCounter = pictureBoxCashCounter;

            var cashCounterView = CreateCashCounterView("Cash Counter", 100000);
            var creationPoint = new Point(100, 200);
            _cashCounter = CreateCashCounterPictureBox(cashCounterView, creationPoint);

            #endregion /CashCounter

            #region Enter/Exit

            //_enter = pictureBoxEnter;
            //_exit = pictureBoxExit;
            creationPoint = new Point(300, 300);
            _enter = CreateEnterPictureBox(creationPoint);
            creationPoint = new Point(50, 300);
            _exit = CreateExitPictureBox(creationPoint);

            #endregion /Enter/Exit

            #region FuelDispensers

            //_fuelDispensersList.Add(pictureBoxFuelDispenser1);
            //_fuelDispensersList.Add(pictureBoxFuelDispenser2);
            var fuelView1 = CreateFuelDispenserView("fuelDispenser1", 10);
            var fuelView2 = CreateFuelDispenserView("fuelDispenser2", 15);

            creationPoint = new Point(300, 50);
            CreateFuelDispenserPictureBox(fuelView1, creationPoint);
            creationPoint = new Point(300, 150);
            CreateFuelDispenserPictureBox(fuelView2, creationPoint);

            #endregion /FuelDispensers

            #region FuelTanks

            //_fuelTanksList.Add(pictureBoxFuelTank1);
            //_fuelTanksList.Add(pictureBoxFuelTank2);

            var fuelTank = CreateFuelTankView("Fuel Tank", 10000);
            creationPoint = new Point(540, 50);
            CreateFuelTankPictureBox(fuelTank, creationPoint);

            #endregion /FuelTanks

            #region DestinationPoints

            var carHeight = 35;

            _noFillingHorizontalLine = this.Height - 2 * carHeight - 20;
            _filledHorizontalLine = this.Height - 3 * carHeight - 40;

            _rightPlaygroundBorder = this.Width;
            _leftPlaygroundBorder = 0;
            _leftCarDestroyingEdge = _leftPlaygroundBorder - 40;

            // Spawning/destroying car destination points
            _spawnPoint = new Point(_rightPlaygroundBorder + 50, _noFillingHorizontalLine);
            _leavePointNoFilling = new Point(_leftCarDestroyingEdge, _noFillingHorizontalLine);
            _leavePointFilled = new Point(_leftCarDestroyingEdge, _filledHorizontalLine);

            // Destination points to enter/leave gas station
            _enterCenter = new Point(_enter.Left + _enter.Width / 2,
                _enter.Top + _enter.Height / 2);
            _exitCenter = new Point(_exit.Left + _exit.Width / 2,
                _exit.Top + _exit.Height / 2);

            _enterPoint1 = new Point(_spawnPoint.X - 200, _filledHorizontalLine);
            _enterPoint2 = new Point(_enterCenter.X, _enterCenter.Y + _enter.Height);
            _enterPoint3 = new Point(_enterCenter.X, _enterCenter.Y - _enter.Height);

            _exitPoint1 = new Point(_exitCenter.X, _exitCenter.Y - _exit.Height);
            _exitPoint2 = new Point(_exitCenter.X, _exitCenter.Y + _exit.Height);

            // Save all predetermined points 
            _predeterminedPoints.AddRange(_fuelDispensersDestPoints.Values);

            _predeterminedPoints.Add(_enterPoint3);
            _predeterminedPoints.Add(_enterPoint2);
            _predeterminedPoints.Add(_enterPoint1);

            _predeterminedPoints.Add(_leavePointFilled);
            _predeterminedPoints.Add(_exitPoint2);
            _predeterminedPoints.Add(_exitPoint1);

            #endregion /DestinationPoints
        }

        #endregion /TopologyMappingLogic

        #region ElementsProducers

        #region CashCounter

        private CashCounterView CreateCashCounterView(string name, int maxCashVolume)
        {
            return new CashCounterView(name, maxCashVolume);
        }

        private PictureBox CreateCashCounterPictureBox(CashCounterView cashCounterView,
            Point locationPoint)
        {
            var size = 50;
            PictureBox cashCounter = new PictureBox();
            cashCounter.Tag = cashCounterView;
            cashCounter.Image = Properties.Resources.cashbox;
            cashCounter.Size = new Size(size, size);
            cashCounter.Location = locationPoint;
            cashCounter.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Controls.Add(cashCounter);
            cashCounter.BringToFront();

            _cashCounter = cashCounter;

            return cashCounter;
        }

        #endregion /CashCounter

        #region Enter/Exit

        private PictureBox CreateEnterPictureBox(Point locationPoint)
        {
            var sizeX = 80;
            var sizeY = 20;
            PictureBox enter = new PictureBox();
            enter.Tag = "Enter";
            enter.BackColor = Color.Chartreuse;
            //enter.Image = Properties.Resources.Enter;
            enter.Size = new Size(sizeX, sizeY);
            enter.Location = locationPoint;
            enter.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Controls.Add(enter);
            enter.BringToFront();

            _enter = enter;

            return enter;
        }

        private PictureBox CreateExitPictureBox(Point locationPoint)
        {
            var sizeX = 80;
            var sizeY = 20;
            PictureBox exit = new PictureBox();
            exit.Tag = "Exit";
            exit.BackColor = Color.Coral;
            //enter.Image = Properties.Resources.Exit;
            exit.Size = new Size(sizeX, sizeY);
            exit.Location = locationPoint;
            exit.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Controls.Add(exit);
            exit.BringToFront();

            _exit = exit;

            return exit;
        }

        #endregion /Enter/Exit

        #region Cars

        private CarView CreateCarView( /*CarModel*/)
        {
            var id = _timerTicksCount;
            var name = "mycar";
            var tankVolume = 80;
            var fuelRemained = 20;
            FuelModel fuel = new FuelModel(1, "АИ-92", 42.9);
            var isTruck = false;
            var isGoesFilling = false;

            return new CarView(id, name, tankVolume, fuelRemained,
                fuel, isTruck, isGoesFilling);
        }

        private PictureBox CreateCarPictureBox(CarView carView)
        {
            PictureBox car = new PictureBox();
            car.Tag = carView;
            car.Image = Properties.Resources.car_32x17__left;
            car.Location = _spawnPoint;
            car.SizeMode = PictureBoxSizeMode.AutoSize;

            carView.CreateSafeArea(car, _carSpeedFilling);
            this.Controls.Add(car);
            car.BringToFront();

            return car;
        }

        #endregion /Cars

        #region FuelDispensers

        private FuelDispenserView CreateFuelDispenserView(string name, int speedOfFilling)
        {
            return new FuelDispenserView(name, speedOfFilling);
        }

        private PictureBox CreateFuelDispenserPictureBox(FuelDispenserView fuelDispenserView,
            Point locationPoint)
        {
            var size = 50;
            PictureBox fuelDispenser = new PictureBox();
            fuelDispenser.Tag = fuelDispenserView;
            fuelDispenser.Image = Properties.Resources.dispenser70;
            fuelDispenser.Size = new Size(size, size);
            fuelDispenser.Location = locationPoint;
            fuelDispenser.SizeMode = PictureBoxSizeMode.StretchImage;

            //// Filling area
            //PictureBox fillingArea = new PictureBox();
            //fillingArea.Tag = fuelDispenserView;
            //fillingArea.BackColor = Color.LightSlateGray;
            //fillingArea.Size = new Size(size, size / 2);
            //fillingArea.Left = fuelDispenser.Left;
            //fillingArea.Top = fuelDispenser.Bottom;
            //fillingArea.SizeMode = PictureBoxSizeMode.AutoSize;
            //this.Controls.Add(fillingArea);
            //fillingArea.BringToFront();

            this.Controls.Add(fuelDispenser);
            fuelDispenser.BringToFront();

            _fuelDispensersList.Add(fuelDispenser);

            var pointOfFueling = new Point(fuelDispenser.Left + _fuelingPointDeltaX,
                fuelDispenser.Bottom + _fuelingPointDeltaY);
            _fuelDispensersDestPoints.Add(fuelDispenser, pointOfFueling);

            return fuelDispenser;
        }

        #endregion /FuelDispensers

        #region FuelTanks

        private FuelTankView CreateFuelTankView(string name, int volume)
        {
            return new FuelTankView(name, volume);
        }

        private PictureBox CreateFuelTankPictureBox(FuelTankView fuelTankView,
            Point locationPoint)
        {
            var size = 50;
            PictureBox fuelTank = new PictureBox();
            fuelTank.Tag = fuelTankView;
            fuelTank.Image = Properties.Resources.FuelTank;
            fuelTank.Size = new Size(size, size);
            fuelTank.Location = locationPoint;
            fuelTank.SizeMode = PictureBoxSizeMode.StretchImage;
            fuelTank.BackColor = Color.Wheat; //For testing

            this.Controls.Add(fuelTank);
            fuelTank.BringToFront();

            _fuelTanksList.Add(fuelTank);

            return fuelTank;
        }

        #endregion /FuelTanks

        #endregion /ElementsProducers
    }
}

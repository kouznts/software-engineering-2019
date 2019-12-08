using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GasStationMs.Bll.Services.Interfaces;
using GasStationMs.Dal;

namespace GasStationMs.App
{
    public partial class TopologyConstructor : Form
    {
        private readonly GasStationContext _db;
        private readonly IFuelService _fuelService;
        private readonly ICarService _carService;

        public TopologyConstructor(GasStationContext gasStationContext, IFuelService fuelService,
            ICarService carService)
        {
            _db = gasStationContext;
            _fuelService = fuelService;
            _carService = carService;

            InitializeComponent();

            SetSettings();
        }

        #region события
        private void cellsHorizontally_ValueChanged(object sender, EventArgs e)
        {
            fillingStationField.ColumnCount = (int)cellsHorizontally.Value;
        }

        private void cellsVertically_ValueChanged(object sender, EventArgs e)
        {
            fillingStationField.RowCount = (int)cellsVertically.Value;
        }
        #endregion
    }
}

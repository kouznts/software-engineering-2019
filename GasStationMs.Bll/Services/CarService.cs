using System;
using System.Collections.Generic;
using System.Text;
using GasStationMs.Bll.Services.Interfaces;
using GasStationMs.CommonLayer.Dto;
using GasStationMs.Dal;

namespace GasStationMs.Bll.Services
{
    public class CarService : ICarService
    {
        private readonly GasStationContext _db;

        public CarService(GasStationContext gasStationContext)
        {
            _db = gasStationContext;
        }
        public IEnumerable<CarDto> GetAllCars()
        {
            throw new NotImplementedException();
        }
    }
}

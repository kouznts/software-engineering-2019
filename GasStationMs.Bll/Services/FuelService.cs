using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GasStationMs.Bll.Services.Interfaces;
using GasStationMs.CommonLayer.Dto;

namespace GasStationMs.Bll.Services
{
    public class FuelService : IFuelService
    {
        public IEnumerable<FuelDto> GetAllFuels()
        {
            throw new NotImplementedException();
        }

        public Task<FuelDto> GetFuelByIdAsync(int fuelId)
        {
            throw new NotImplementedException();
        }

        public Task<FuelDto> AddFuelAsync(FuelDto fuelDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFuelAsync(int fuelId)
        {
            throw new NotImplementedException();
        }
    }
}

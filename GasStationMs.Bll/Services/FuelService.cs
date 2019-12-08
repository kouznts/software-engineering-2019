using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GasStationMs.Bll.Services.Interfaces;
using GasStationMs.CommonLayer.Dto;
using GasStationMs.Dal;
using Microsoft.EntityFrameworkCore;

namespace GasStationMs.Bll.Services
{
    public class FuelService : IFuelService
    {
        private readonly GasStationContext _db;

        public FuelService(GasStationContext gasStationContext)
        {
            _db = gasStationContext;
        }

        public IEnumerable<FuelDto> GetAllFuels()
        {
            throw new NotImplementedException();
        }

        public FuelDto GetFuelByIdAsync(int fuelId)
        {
            var retrievedFuel =  _db.Fuels.FirstOrDefault(f => f.Id == fuelId);
            return new FuelDto()
            {
                Id = retrievedFuel.Id,
                Name = retrievedFuel.Name,
                Price = retrievedFuel.Price
            };
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

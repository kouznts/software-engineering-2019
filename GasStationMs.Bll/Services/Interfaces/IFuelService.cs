using System.Collections.Generic;
using System.Threading.Tasks;
using  GasStationMs.CommonLayer.Dto;

namespace GasStationMs.Bll.Services.Interfaces
{
    public interface IFuelService
    {
        IEnumerable<FuelDto> GetAllFuels();
        Task<FuelDto> GetFuelByIdAsync(int fuelId);
        Task<FuelDto> AddFuelAsync(FuelDto fuelDto);
        
        //The logic of the program does not require fuel update
        //Task<FuelDto> UpdateFuelAsync(int fuelId, FuelDto newFuelInfo);
        Task<bool> DeleteFuelAsync(int fuelId);
    } 
}

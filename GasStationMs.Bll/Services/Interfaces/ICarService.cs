using System.Collections.Generic;
using GasStationMs.CommonLayer.Dto;

namespace GasStationMs.Bll.Services.Interfaces
{
    public interface ICarService
    {
        //The external contract consists only of GetAllCars method
        //the other methods are not required for the program logic
        IEnumerable<CarDto> GetAllCars();
        
    }
}

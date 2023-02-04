using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalConsole
{
    internal class CarsInventory : ICarsInventory
    {
        readonly Dictionary<int, Car> _carsDict = new();

        public event EventHandler<Car>? OnCarUpdated = delegate { };
        public event EventHandler<List<Car>> OnInventoryUpdated = delegate { };

        public void AddCar(Car car)
        {
            lock (_carsDict)
            {
                _carsDict[car.LicenseNumber] = car;
            }
            OnInventoryUpdated?.Invoke(this, GetCars());
        }
        public Car GetCar(int carNumber)
        {
            return _carsDict[carNumber];
        }
        public List<Car> GetCars()
        {
            return _carsDict.Values.ToList();
        }

        public void RemoveCar(Car car)
        {
            _carsDict.Remove(car.LicenseNumber);
            OnInventoryUpdated?.Invoke(this, GetCars());
        }

        public void UpdateCar(Car car)
        {
            _carsDict[car.LicenseNumber] = car;
            OnCarUpdated?.Invoke(this, car);
        }

    }
}

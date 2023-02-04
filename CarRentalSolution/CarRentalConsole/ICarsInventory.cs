using System;
using System.Linq;

namespace CarRentalConsole
{
    internal interface ICarsInventory
    {
        void AddCar(Car car);
        Car GetCar(int carNumber);
        List<Car> GetCars();
        void RemoveCar(Car car);
        void UpdateCar(Car car);

        event EventHandler<Car>? OnCarUpdated;
        public event EventHandler<List<Car>> OnInventoryUpdated;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRentalConsole.Car;

namespace CarRentalConsole
{
    internal class CarRentalManager
    {
        public CarRentalManager(CarsInventory inventory)
        {
            Inventory = inventory;
        }

        public void AddNewCar(int licenseNumber, CarTypeEnum carType, string description)
        {
            var car = new Car(licenseNumber, carType, description);
            Inventory.AddCar(car);
        }

        public CarsInventory Inventory { get; set; }
    }
}

using CarRentalConsole;
using System.Globalization;
using static CarRentalConsole.Car;



Random rnd = new();
CarsInventory inventory = new();
CarRentalManager manager = new(inventory);
manager.Inventory.OnInventoryUpdated += Inventory_OnInventoryUpdated;

void Inventory_OnInventoryUpdated(object? sender, List<Car> e)
{
    Console.WriteLine($"Inventory changed {manager.Inventory.GetCars().ToStringListLines()}");
}

manager.AddNewCar(rnd.Next(1111111, 9999999), CarTypeEnum.LARGE_SEDAN, "blue BMW 530i");
manager.AddNewCar(rnd.Next(1111111, 9999999), CarTypeEnum.LARGE_SEDAN, "white Camry 2.4l");
manager.AddNewCar(rnd.Next(1111111, 9999999), CarTypeEnum.SMALL_SEDAN, "white Subaru impreza 1.6l");
manager.AddNewCar(rnd.Next(1111111, 9999999), CarTypeEnum.MINI, "yellow seat ibiza 1l");




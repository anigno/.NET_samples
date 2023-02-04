using System;
using System.Linq;

namespace CarRentalConsole
{
    public class Car
    {
        public Car(int licenseNumber, CarTypeEnum carType, string description)
        {
            LicenseNumber = licenseNumber;
            CarType = carType;
            Description = description;
            IsRented = false;
            RentTime = DateTime.MaxValue;
            ReturnTime = DateTime.MaxValue;
        }

        public override string ToString()
        {
            return $"[Car {LicenseNumber} {IsRented} ({Description}) ({RentTime}) ({ReturnTime})]";
        }

        public CarTypeEnum CarType { get; set; }
        public string Description { get; set; }
        public bool IsRented { get; set; }
        public int LicenseNumber { get; set; }
        public DateTime? RentTime { get; set; }
        public DateTime? ReturnTime { get; set; }

        public enum CarTypeEnum
        {
            SMALL_SEDAN,
            LARGE_SEDAN,
            MINI,
            SPORT
        }
    }
}

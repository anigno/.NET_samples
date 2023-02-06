using System;
using System.Linq;

namespace CodeSamples.oop
{
    public class Car : RoadLegalVehicle
    {
        public Car(int maxSpeed, string licenseNumber, string brand, CarTypeEnum carType) : base(maxSpeed, licenseNumber)
        {
            Brand = brand;
            CarType = carType;
        }

        public override string ToString()
        {
            return $"[Car, {Brand} {CarType} {base.ToString()}]";
        }

        public string Brand { get; private set; }
        public CarTypeEnum CarType { get; set; }
    }
}


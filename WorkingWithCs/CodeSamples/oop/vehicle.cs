using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples.oop
{
    internal interface IVehicle
    {
        int MaxSpeed { get; }
        void Move(int speed);
    }

    public abstract class Vehicle : IVehicle
    {
        private int speed = 0;
        public int MaxSpeed { get; protected set; }
        public virtual void Move(int newSpeed)
        {
            Console.WriteLine($"speed will be changed from: {this.speed} to: {newSpeed}");
            this.speed = newSpeed;
        }
        public override string ToString()
        {
            return $"[Vehicle, {speed}<{MaxSpeed}]";
        }
        public int Speed
        {
            get
            {
                if (speed > 100) Console.WriteLine("too fast!");
                return speed;
            }
            set { speed = value; }
        }
    }

    public abstract class RoadLegalVehicle : Vehicle
    {
        public string LicenseNumber { get; private set; }

        protected RoadLegalVehicle(int maxSpeed, string licenseNumber)
        {
            MaxSpeed = maxSpeed;
            LicenseNumber = licenseNumber;
        }
        public override void Move(int newSpeed)
        {
            base.Move(newSpeed);
            Speed = newSpeed <= MaxSpeed ? Speed : MaxSpeed;
        }
        public override string ToString()
        {
            return $"[RoadLegalVehicle, {LicenseNumber} {base.ToString()}]";
        }
    }

    public enum CarTypeEnum
    {
        Sedan = 1,
        Convertable = 2,
        Sport = 3
    }

    public class Car : RoadLegalVehicle
    {
        public CarTypeEnum CarType { get; set; }
        public string Brand { get; private set; }
        public Car(int maxSpeed, string licenseNumber, string brand,CarTypeEnum carType) : base(maxSpeed, licenseNumber)
        {
            this.Brand = brand;
            CarType = carType;
        }
        public override string ToString()
        {
            return $"[Car, {Brand} {CarType} {base.ToString()}]";
        }
    }
}


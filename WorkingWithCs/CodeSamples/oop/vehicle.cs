using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSamples.oop
{
    internal interface IVehicle
    {
        void Move(int speed);

        int MaxSpeed { get; }
    }

    public abstract class Vehicle : IVehicle
    {
        private int speed = 0;

        public Vehicle(int maxSpeed)
        {
            MaxSpeed = maxSpeed;
        }

        public virtual void Move(int newSpeed)
        {
            Console.WriteLine($"speed will be changed from: {this.speed} to: {newSpeed}");
            this.speed = newSpeed;
        }

        public override string ToString()
        {
            return $"[Vehicle, {speed}<{MaxSpeed}]";
        }

        public int MaxSpeed { get; protected set; }
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

    public enum CarTypeEnum
    {
        Sedan = 1,
        Convertable = 2,
        Sport = 3
    }
}


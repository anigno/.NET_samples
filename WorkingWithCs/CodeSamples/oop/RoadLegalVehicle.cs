using System;
using System.Linq;

namespace CodeSamples.oop
{
    public abstract class RoadLegalVehicle : Vehicle
    {

        protected RoadLegalVehicle(int maxSpeed, string licenseNumber) : base(maxSpeed)
        {
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

        public string LicenseNumber { get; private set; }
    }
}


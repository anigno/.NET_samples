#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BlCommon;

#endregion

namespace SerializationTesting
{
    internal class Program
    {
        #region Constructors

        public Program()
        {
            VehicleOwnerA vehicleOwnerA = new VehicleOwnerA();
            vehicleOwnerA.Vehicles.Add(new Vehicle() {LicensePlateNumber = DateTime.Now.Ticks, Type = "Some vehicle type"});
            vehicleOwnerA.Vehicles.Add(new Vehicle() {LicensePlateNumber = DateTime.Now.Ticks, Type = "Some vehicle type"});
            vehicleOwnerA.Vehicles.Add(new Vehicle() {LicensePlateNumber = DateTime.Now.Ticks, Type = "Some vehicle type"});
            VehicleOwnerB vehicleOwnerB = new VehicleOwnerB();
            vehicleOwnerB.Vehicles.Add(new Vehicle() {LicensePlateNumber = DateTime.Now.Ticks, Type = "Some vehicle type"});
            vehicleOwnerB.Vehicles.Add(new Vehicle() {LicensePlateNumber = DateTime.Now.Ticks, Type = "Some vehicle type"});
            vehicleOwnerB.Vehicles.Add(new Vehicle() {LicensePlateNumber = DateTime.Now.Ticks, Type = "Some vehicle type"});

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStreamA = new MemoryStream();
            MemoryStream memoryStreamB = new MemoryStream();
            formatter.Serialize(memoryStreamA, vehicleOwnerA);
            formatter.Serialize(memoryStreamB, vehicleOwnerB);
        }

        #endregion

        #region Private Methods

        private static void Main(string[] args)
        {
            new Program();
            TestUtils.WaitForEnter();
        }

        #endregion
    }

    [Serializable]
    public class Person
    {
        #region Public Properties

        public string Name { get; set; }
        public int Id { get; set; }

        #endregion
    }

    [Serializable]
    public class Vehicle
    {
        #region Public Properties

        public string Type { get; set; }
        public long LicensePlateNumber { get; set; }

        #endregion
    }

    [Serializable]
    public class VehicleOwnerA : Person
    {
        #region Constructors

        public VehicleOwnerA()
        {
            Vehicles = new List<Vehicle>();
        }

        #endregion

        #region Public Properties

        public List<Vehicle> Vehicles { get; set; }

        #endregion
    }

    [Serializable]
    public class VehicleOwnerB
    {
        #region Constructors

        public VehicleOwnerB()
        {
            Vehicles = new List<Vehicle>();
        }

        #endregion

        #region Public Properties

        public Person PersonItem { get; set; }
        public List<Vehicle> Vehicles { get; set; }

        #endregion
    }

    [Serializable]
    public class VehicleOwners
    {
        #region Constructors

        public VehicleOwners()
        {
            VehiclesOwners = new Dictionary<Vehicle, List<int>>();
        }

        #endregion

        #region Public Properties

        public Dictionary<Vehicle, List<int>> VehiclesOwners { get; set; }

        #endregion
    }
}
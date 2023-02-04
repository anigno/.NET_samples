using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WfpCarRental.BL
{
    class CarRentalManager
    {
        private Random rnd = new();

        public CarRentalManager()
        {

        }

        public event EventHandler OnCarReturned;

        /// <summary>
        /// start a new timer for auto car return (as customer does in airports)
        /// </summary>
        public void StartNewCarRentalTimer(int licenseNumber, int durationInSec)
        {
            int rentTime = rnd.Next((int)(durationInSec * 0.8), (int)(durationInSec * 1.2));

            Task.Factory.StartNew(async () =>
            {
                Console.WriteLine($"{licenseNumber} {rentTime}");
                await Task.Delay(rentTime * 1000);
                Console.WriteLine($"{licenseNumber} {rentTime}");
            });
        }
    }

}

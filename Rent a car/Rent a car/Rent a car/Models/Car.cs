using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.Models
{
    public class Car
    {
        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public int passagerSeats { get; set; }
        public string information { get; set; }
        public decimal price { get; set; }

        public Car()
        {

        }
    }
}

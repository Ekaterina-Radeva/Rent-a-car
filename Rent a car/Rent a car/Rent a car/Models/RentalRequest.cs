using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_a_car.Models
{
    public class RentalRequest
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int CarId { get; set; }
        public Car Car { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsApproved { get; set; } = false;
    }
}

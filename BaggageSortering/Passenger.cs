using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Passenger
    {
        public Passenger(string firstName, string lastName = "dummy", string email = "dummy@gmail.com", string phoneNumber = "+4512345678", string address = "street 2")
        {
            FirstName =  firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber =  phoneNumber;
            Address =  address;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Address { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Passenger
    {
        public Passenger(string firstName = "Dummy", string lastName = "Dummy", string email = "dummy@gmail.com", string phoneNumber = "+4512345678", string address = "street 2")
        {
            Id = _id++;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        private static int _id = 0; 

        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return $"My names is {FirstName} {LastName}, I live in {Address}. \nMy email is {Email} and my phone number is {PhoneNumber}";
        }
    }
}

namespace BaggageSorteringLib
{
    public class Passenger
    {
        public Passenger(string firstName, string lastName, string email, string phoneNumber, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }

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

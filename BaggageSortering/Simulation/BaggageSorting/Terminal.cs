using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class Terminal : IOpenClose
    {
        public Terminal(int id)
        {
            Id = id;
            IsOpen = false;
            Luggages = new Queue<Luggage>();
        }

        public int Id { get; private set; }
        public bool IsOpen { get; private set; }
        public Queue<Luggage> Luggages { get; private set; }
        public Flight Flight { get; set; }

        public void Close()
        {
            IsOpen = false;
        }
        public void Open()
        {
            IsOpen = true;
        }
        
        public void AddLuggage(Luggage luggage)
        {
            Luggages.Enqueue(luggage);
        }

        public override string ToString()
        {
            if (IsOpen)
            {
                return $"{Id}";
            }
            else
            {
                return $"{Id}";
            }
        }

        public void LoadFlightLuggages()
        {
            if (Flight != null)
            {
                Flight.LoadWithLuggages(Luggages);
                Luggages = new Queue<Luggage>();
            }
        }
    }
}

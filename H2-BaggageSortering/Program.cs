using BaggageSorteringLib;
using Engine;
using System;

class Program
{
    enum FormState
    {
        AirportOverview,
        PassengerCreation,
        ReservationCreation,
    };

    static Simulator simulator;
    static AutoGenerator autoGenerator;
    static ConsoleBuffer infoBuffer = new ConsoleBuffer(12);
    static FormState formState = FormState.AirportOverview;

    static Passenger passenger = new Passenger();
    //static Reservation reservation = new Reservation();

    static void Main(string[] args)
    {
        // Starts simulator
        simulator = new Simulator();
        simulator.IsAutoGenerationEnabled = true;
        simulator.SortingMachine.ProcessInfo += OnSortingMachineProcessInfo;
        simulator.SortingMachine.ProcessException += OnSortingMachineProcessException;
        simulator.Start();

        autoGenerator = new AutoGenerator(simulator);

        // Creates advanced console
        ConsoleEx.Create(80, 48);
        ConsoleEx.SetFont("Terminal", 12, 16);

        while (true)
        {
            GlobalUserInput();

            // Shows current form state
            switch (formState)
            {
                case FormState.AirportOverview      : AirportOverview();    break;
                case FormState.PassengerCreation    : PassengerCreation();  break;
                case FormState.ReservationCreation  : ReservationCreation();  break;
            }
            simulator.Update();

            ConsoleEx.Update();
            ConsoleEx.Clear();  
        }
    }

    // Events ***************************************
    private static void OnSortingMachineProcessInfo(string msg)
    {
        infoBuffer.WriteLine(msg);
    }
    private static void OnSortingMachineProcessException(string msg)
    {
        infoBuffer.WriteLine("\fc" + msg);
    }
  
    // Form methods *******************************
    static void AirportOverview()
    {
        // Testing
        if (Input.KeyPressed((Key)('A'))) simulator.CheckLuggageIn(1, new Luggage(1, autoGenerator.CreateRandomReservation()));
        if (Input.KeyPressed((Key)('S'))) simulator.CheckLuggageIn(2, new Luggage(2, autoGenerator.CreateRandomReservation()));
        if (Input.KeyPressed((Key)('D'))) simulator.CheckLuggageIn(3, new Luggage(3, autoGenerator.CreateRandomReservation()));
        if (Input.KeyPressed((Key)('F'))) simulator.CheckLuggageIn(4, new Luggage(4, autoGenerator.CreateRandomReservation()));
        if (Input.KeyPressed((Key)('G'))) simulator.CheckLuggageIn(5, new Luggage(5, autoGenerator.CreateRandomReservation()));

        ConsoleEx.WriteLine($"- \faAIRPOT OVERVIEW \f7| \feSPEED {simulator.Time.Speed}x \f7| \ff{simulator.Time.DateTime.ToShortTimeString()}");
        AirportDraw.Counters(simulator.Counters, 1);
        AirportDraw.ConveyorBelt(simulator.SortingMachine.ConveyorBelt, " - Sorting system's conveyor belt", 8);
        AirportDraw.Terminals(simulator.Terminals, 9);
        AirportDraw.FlightSchedule(19, simulator.FlightSchedule);
        AirportDraw.InfoBuffer(33, "SORTING MACHINE INFO", infoBuffer);
    }
    static void PassengerCreation()
    {
        ConsoleEx.WriteLine("- \f9PASSENGER CREATION");
        passenger.FirstName = UserTextInput(passenger.FirstName, 'F', "FirstName");
        passenger.LastName = UserTextInput(passenger.LastName, 'L', "LastName");
        passenger.Address = UserTextInput(passenger.Address, 'A', "Address");
        passenger.Email = UserTextInput(passenger.Email, 'E', "Email");
        passenger.PhoneNumber = UserTextInput(passenger.PhoneNumber, 'P', "PhoneNumber");
        ConsoleEx.WriteLine($"\nPassenger: {passenger}");
    }
    static void ReservationCreation()
    {
        ConsoleEx.WriteLine("- \fbRESERVATION CREATION");
    }

    // Input methods *******************************
    static void GlobalUserInput()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.KeyPressed((Key)(i + '1')))
            {
               simulator.Time.Speed = (int)Math.Pow(2, i);
            }
        }

        // Changes form state by pressing space
        if (Input.KeyPressed(Key.SPACE))
        {
            formState++;
            if ((int)formState >= Enum.GetNames(typeof(FormState)).Length)
            {
                formState = 0;
            }
        }
    }

    // Misc *********************************
    static string UserTextInput(string previewsData, char key, string fieldName)
    {
        ConsoleEx.WriteLine($"{key}: Sets {fieldName}");

        if (Input.KeyPressed((Key)key))
        { 
            string output = previewsData;
            Input.Flush();

            while (!Input.KeyPressed(Key.RETURN))
            {
                ConsoleEx.SetPosition(0, 0);
                ConsoleEx.WriteLine(fieldName + " : " + output);
                output = Input.Read(output);
                ConsoleEx.Update();
                ConsoleEx.Clear();
            }
            if (!string.IsNullOrEmpty(output))
            {
                return output;
            }
        }
        return previewsData;
    }
}
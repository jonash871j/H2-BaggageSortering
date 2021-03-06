namespace BaggageSorteringLib
{
    interface IOpenClose
    {
        bool IsOpen { get; }

        void Open();
        void Close();
    }
}

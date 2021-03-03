namespace BaggageSorteringLib
{
    public class BufferTray<T>
    {
        public BufferTray(int length)
        {
            Length = length;
            Buffer = new T[length];
        }

        public T this[int i]
        {
            get => Buffer[i];
            set => Buffer[i] = value;
        }

        public int Length { get; private set; }
        public T[] Buffer { get; set; }
        public int Position { get; set; } = 0;

        public void Push(T type)
        {
            Buffer[Position] = type;
            Position++;
        }
        public void PushToFirst(T type)
        {
            for (int i = Position; i > 0; i--)
            {
                Buffer[i] = Buffer[i - 1];
            }

            Buffer[0] = type;
            Position++;
        }
        public T Pull()
        {
            Position--;
            return Buffer[Position];
        }
        public void Clear()
        {
            Position = 0;
        }
        public T[] ToArray()
        {
            T[] items = new T[Position];

            for (int i = 0; i < Position; i++)
            {
                items[i] = Buffer[i];
            }

            return items;
        }
        public bool IsSpace()
        {
            if (Position < Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsEmpty()
        {
            if (Position == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

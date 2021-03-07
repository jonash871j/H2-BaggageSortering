namespace BaggageSorteringLib
{
    public class ConveyorBelt<T>
    {
        public ConveyorBelt(int length)
        {
            Length = length;
            Buffer = new T[length];
            Clear();
        }

        public T this[int i]
        {
            get => Buffer[i];
            internal set => Buffer[i] = value;
        }

        public int Length { get; private set; }
        public T[] Buffer { get; internal set; }

        /// <summary>
        /// Used to set type to first index in buffer
        /// </summary>
        internal void Push(T type)
        {
            Buffer[0] = type;
        }

        /// <summary>
        /// Used to get last type in buffer
        /// </summary>
        /// <returns>type</returns>
        internal T Pull()
        {
            T type = Buffer[Length - 1];
            if (type == null)
            {
                return default;
            }
            else
            {
                Buffer[Length - 1] = default; 
                MoveForward();
                return type;
            }
        }
        /// <summary>
        /// Checks if last pull is empty
        /// </summary>
        /// <returns>true when empty</returns>
        public bool IsPullEmpty()
        {
            T type = Buffer[Length - 1];
            if (type == null)
            {
                return true;
            }   
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Used to check is there is space on the first index
        /// </summary>
        /// <returns>true there is space</returns>
        public bool IsSpace()
        {
            if (Buffer[0] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Used to move conveyor belt forward if last index is empty
        /// </summary>
        internal void MoveForward()
        {
            if (Buffer[Length - 1] == null)
            {
                for (int i = Length - 1; i > 0; i--)
                {
                    Buffer[i] = Buffer[i - 1];
                }
                Buffer[0] = default;
            }
        }

        /// <summary>
        /// Used to clear the buffer with nulls
        /// </summary>
        internal void Clear()
        {
            for (int i = 0; i < Length; i++)
            {
                Buffer[i] = default;
            }
        }
    }
}

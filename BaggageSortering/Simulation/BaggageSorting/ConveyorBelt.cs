using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSorteringLib
{
    public class ConveyorBelt<T>
    {
        public ConveyorBelt(int length)
        {
            Length = length;
            Buffer = new T[length];
            
            for (int i = 0; i < Length; i++)
            {
                Buffer[i] = default;
            }
        }

        public T this[int i]
        {
            get => Buffer[i];
            set => Buffer[i] = value;
        }

        public int Length { get; private set; }
        public T[] Buffer { get; set; }

        public void Push(T type)
        {
            Buffer[0] = type;
        }
        public T Pull()
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
        public void MoveForward()
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
    }
}

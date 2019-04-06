using System;
namespace Rokkit
{
    public class Property<T>
    {
        public Type Type = typeof(T);
        public T Value;

        public Property(T value)
        {
            Value = value;
        }
    }
}

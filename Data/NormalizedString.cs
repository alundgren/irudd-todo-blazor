using System;
using System.Diagnostics.CodeAnalysis;

namespace Irudd.Todo.Data
{
    public class NormalizedString : IEquatable<NormalizedString>
    {
        public NormalizedString(string s)
        {
            NormalizedValue = Normalize(s);
        }

        public NormalizedString()
        {

        }

        public string NormalizedValue { get; set; }

        public static string Normalize(string s)
        {
            return s?.Trim()?.Replace("\r", "").Replace("\n", "").Replace("[", "(").Replace("]", ")");
        }

        public static bool operator ==(NormalizedString obj1, NormalizedString obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;

            if (ReferenceEquals(obj1, null))
                return false;

            if (ReferenceEquals(obj2, null))
                return false;

            return obj1.Equals(obj2);
        }

        public static bool operator !=(NormalizedString obj1, NormalizedString obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals([AllowNull] NormalizedString other)
        {
            if (other == null)
                return false;
            return other.NormalizedValue.Equals(NormalizedValue, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.NormalizedValue.GetHashCode();
        }

        public override string ToString()
        {
            return this.NormalizedValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((NormalizedString)obj);
        }

        public static implicit operator NormalizedString(string s) => new NormalizedString(s);
    }
}

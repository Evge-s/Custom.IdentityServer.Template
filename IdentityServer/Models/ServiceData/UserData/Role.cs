using System.Reflection;

namespace IdentityServer.Models.Data.UserData
{
    public class Role : Enumeration
    {
        public static readonly Role GeneralAdmin = new Role(0, "GeneralAdmin");
        public static readonly Role AppAdmin = new Role(1, "AppAdmin");
        public static readonly Role Customer = new Role(2, "Customer");

        public Account Account { get; set; }
        public Guid AccountId { get; set; }

        private Role(int id, string name) : base(id, name) { }

        public static Role From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);
            if (state is null)
            {
                throw new InvalidOperationException($"Possible values for CommunicationType: {string.Join(",", List().Select(s => s.Name))}");
            }
            return state;
        }

        public static Role FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (state is null)
            {
                throw new InvalidOperationException($"Possible values for CommunicationType: {string.Join(",", List().Select(s => s.Name))}");
            }
            return state;
        }
        public static IEnumerable<Role> List() =>
        new[] { GeneralAdmin, AppAdmin, Customer };
    }

    public abstract class Enumeration : IComparable
    {
        protected Enumeration(int id, string name) => (Id, Name) = (id, name);
        public int Id { get; private set; }
        public string Name { get; private set; }

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }
        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }
        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }
        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                        .Select(f => f.GetValue(null))
                        .Cast<T>();
        public static bool Contains<T>(T enumeration)
            where T : Enumeration
            => GetAll<T>().Any(e => e.Equals(enumeration));
        public static bool Contains<T>(int id)
            where T : Enumeration
            => GetAll<T>().Any(e => e.Id == id);
        public int CompareTo(object? obj)
            => Id.CompareTo(((Enumeration?)obj)?.Id);
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj is not Enumeration otherValue)
            {
                return false;
            }
            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }
        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString() => Name;
        public static implicit operator int(Enumeration e)
            => e.Id;
        public static bool operator ==(Enumeration left, Enumeration right)
            => left is null
                ? right is null
            : left.Equals(right);
        public static bool operator !=(Enumeration left, Enumeration right)
            => !(left == right);
        public static bool operator <(Enumeration left, Enumeration right)
            => left is null
                ? right is not null
            : left.CompareTo(right) < 0;
        public static bool operator <=(Enumeration left, Enumeration right)
            => left is null || left.CompareTo(right) <= 0;
        public static bool operator >(Enumeration left, Enumeration right)
            => left is not null && left.CompareTo(right) > 0;
        public static bool operator >=(Enumeration left, Enumeration right)
            => left is null
                ? right is null
                : left.CompareTo(right) >= 0;
        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);
            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
            return matchingItem;
        }
    }
}
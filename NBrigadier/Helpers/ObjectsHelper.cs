namespace NBrigadier.Helpers
{
    public class ObjectsHelper
    {
        public new static bool Equals(object thizz, object other)
        {
            if (thizz == null && other == null) return true;
            return thizz?.Equals(other) == true;
        }
        public static int Hash(params object[] objects)
        {
            return objects?.GetHashCode() ?? 0;
        }
    }
}
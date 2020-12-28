namespace NBrigadier.Helpers
{
    public class ObjectsHelper
    {
        public new static bool Equals(object thizz, object other)
        {
            return thizz.Equals(other);
        }
        public new static int Hash(params object[] objects)
        {
            return objects.GetHashCode();
        }
    }
}
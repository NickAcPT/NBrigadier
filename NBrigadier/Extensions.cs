using System;

namespace NBrigadier
{
    public static class Extensions
    {
        public static bool IsInstanceOfGenericType(this Type genericType, object instance)
        {
            Type type = instance.GetType();
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

    }
}
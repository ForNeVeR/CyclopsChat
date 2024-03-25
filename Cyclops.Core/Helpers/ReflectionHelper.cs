using System;
using System.Reflection;

namespace Cyclops.Core.Helpers
{
    /// <summary>
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Creates a shallow copy of an obect 
        /// </summary>
        public static TNew ShallowCopyTo<TOld, TNew>(TOld old) where TNew : new()
        {
            var result = new TNew();
            Type oldType = typeof (TOld);
            Type newType = typeof (TNew);

            foreach (PropertyInfo property in old.GetType().GetProperties())
            {
                object value = oldType.GetProperty(property.Name).GetValue(old, null);

                PropertyInfo propertyOfNew = newType.GetProperty(property.Name);
                if (propertyOfNew != null)
                    propertyOfNew.SetValue(result, value, null);
            }
            return result;
        }
    }
}
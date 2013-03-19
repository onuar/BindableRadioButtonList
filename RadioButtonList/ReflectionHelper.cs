﻿using System;

namespace NiceControls
{
    internal class ReflectionHelper
    {
        internal static void SetPropertyValue(object @object, string propertyName, object value)
        {
            var propertyInfo = @object.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return;
            }

            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;
            value = Convert.ChangeType(value, targetType);
            propertyInfo.SetValue(@object, value, null);
        }

        internal static object GetPropertyValue(object @object, string propertyName)
        {
            var propertyInfo = @object.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return null;
            }
            var value = propertyInfo.GetValue(@object);
            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;
            value = Convert.ChangeType(value, targetType);

            return value;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
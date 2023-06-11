namespace Converter.Base
{
    using System;
    using UnityFoundation.Scripts.Converter.BaseConvert;

    public abstract class BaseGenericConverter : BaseConverter
    {
        public override bool CanConvert(Type type)
        {
            return type.IsGenericType && this.ConvertibleType.IsAssignableFrom(type.GetGenericTypeDefinition());
        }
    }
}
namespace Converter.Implement
{
    using System;
    using Converter.Base;
    using Newtonsoft.Json;
    using UnityFoundation.Scripts.Converter.BaseConvert;

    public class DefaultConverter : BaseConverter
    {
        protected override Type ConvertibleType => typeof(object);

        protected override object ConvertFromString_Internal(string str, Type type)
        {
            return JsonConvert.DeserializeObject(str, type);
        }

        protected override string ConvertToString_Internal(object obj, Type type)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
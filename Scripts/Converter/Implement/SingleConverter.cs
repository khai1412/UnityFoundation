namespace Converter.Implement
{
    using System;
    using Converter.Base;
    using TheOneStudio.HyperCasual.Extensions;

    public class SingleConverter : BaseConverter
    {
        protected override Type ConvertibleType => typeof(float);

        protected override object ConvertFromString_Internal(string str, Type type)
        {
            return float.Parse(str.IsNullOrWhitespace() ? "0" : str);
        }

        protected override string ConvertToString_Internal(object obj, Type type)
        {
            return obj?.ToString() ?? "0";
        }
    }
}
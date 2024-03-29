namespace Converter.Implement
{
    using System;
    using System.Linq;
    using Converter.Base;
    using TheOneStudio.HyperCasual.Extensions;
    using UnityEngine;
    using UnityFoundation.Scripts.Converter.BaseConvert;

    public class UnityVector2Converter : BaseConverter
    {
        private readonly string separator;

        public UnityVector2Converter(string separator = "|") : base()
        {
            this.separator = separator;
        }

        protected override Type ConvertibleType => typeof(Vector2);

        protected override object ConvertFromString_Internal(string str, Type type)
        {
            var values = str.Split(this.separator).Select(value => float.Parse(value.IsNullOrWhitespace() ? "0" : value)).ToArray();
            return new Vector2(values[0], values[1]);
        }

        protected override string ConvertToString_Internal(object obj, Type type)
        {
            var vector2 = (Vector2)obj;
            return $"{vector2.x}{this.separator}{vector2.y}";
        }
    }
}
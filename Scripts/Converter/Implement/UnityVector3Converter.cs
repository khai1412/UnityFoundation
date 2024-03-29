namespace Converter.Implement
{
    using System;
    using System.Linq;
    using Converter.Base;
    using TheOneStudio.HyperCasual.Extensions;
    using UnityEngine;
    using UnityFoundation.Scripts.Converter.BaseConvert;

    public class UnityVector3Converter : BaseConverter
    {
        private readonly string separator;

        public UnityVector3Converter(string separator = "|") : base()
        {
            this.separator = separator;
        }

        protected override Type ConvertibleType => typeof(Vector3);

        protected override object ConvertFromString_Internal(string str, Type type)
        {
            var values = str.Split(this.separator).Select(value => float.Parse(value.IsNullOrWhitespace() ? "0" : value)).ToArray();
            return new Vector3(values[0], values[1], values[2]);
        }

        protected override string ConvertToString_Internal(object obj, Type type)
        {
            var vector3 = (Vector3)obj;
            return $"{vector3.x}{this.separator}{vector3.y}{this.separator}{vector3.z}";
        }
    }
}
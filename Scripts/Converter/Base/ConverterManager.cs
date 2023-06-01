namespace Converter.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Converter.Base;
    using Converter.Implement;

    public class ConverterManager
    {
        public static readonly ConverterManager Instance = new();

        private readonly List<IConverter> converters = new();

        private ConverterManager()
        {
            this.converters.Add(new DefaultConverter());
            this.converters.Add(new Int32Converter());
            this.converters.Add(new SingleConverter());
            this.converters.Add(new DoubleConverter());
            this.converters.Add(new BooleanConverter());
            this.converters.Add(new StringConverter());
            this.converters.Add(new UnityVector2Converter());
            this.converters.Add(new UnityVector3Converter());
            this.converters.Add(new ListGenericConverter());
            this.converters.Add(new ReadonlyCollectionGenericConverter());
            this.converters.Add(new DictionaryGenericConverter());
        }

        public IConverter GetConverter(Type type)
        {
            return this.converters.LastOrDefault(converter => converter.CanConvert(type))
                   ?? throw new($"No converter found for type {type}");
        }

        public void AddConverter(IConverter converter)
        {
            this.converters.Add(converter);
        }
    }
}
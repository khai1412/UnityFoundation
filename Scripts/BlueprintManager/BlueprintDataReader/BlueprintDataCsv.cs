namespace UnityFoundation.Scripts.BlueprintManager
{
    using System;
    using System.Collections.Generic;
    using UnityFoundation.Scripts.BlueprintManager.BlueprintBase;
    using UnityFoundation.Scripts.BlueprintManager.BlueprintDataReader;
    using UnityFoundation.Scripts.BlueprintManager.HandleBlueprintData;
    using UnityFoundation.Scripts.Extensions;
    using Zenject;

    public abstract class BlueprintDataCsv<TKey, TData> : IBlueprintData where TData : IDataRecord
    {
        public Dictionary<TKey, TData> Data;
        public void ConvertData(string rawData)
        {
            this.Data ??= new Dictionary<TKey, TData>();
            var dataRecord                   = CsvDataConverter.Deserialize<TData>(rawData);
            foreach (var record in dataRecord)
            {
                var key = record.GetFieldInfoWithAttribute<KeyOfRecord>().GetValue(record);
                if (key == null) throw new Exception("this data must have key attribute");
                this.Data.Add((TKey)key, record);
            }
        }

        public void SetupHandleData(DiContainer container)
        {
            var handleDataType                                                                                   = this.GetCustomAttribute<DataInfoAttribute>().HandleLocalDataType;
            if (container.Resolve(handleDataType) is IHandleBlueprintData handleData) handleData.BlueprintData = this;
        }
        
    }
}
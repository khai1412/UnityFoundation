namespace UnityFoundation.Scripts.BlueprintManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityFoundation.Scripts.BlueprintManager.BlueprintBase;
    using UnityFoundation.Scripts.BlueprintManager.BlueprintDataReader;
    using UnityFoundation.Scripts.BlueprintManager.HandleBlueprintData;
    using UnityFoundation.Scripts.Extensions;
    using Zenject;

    public abstract class BlueprintDataCsv<TKey, TData> : IBlueprintData
    {
        public Dictionary<TKey, TData> Data;
        public void ConvertData(string rawData)
        {
            this.Data ??= new Dictionary<TKey, TData>();
            var dataRecord                   = CsvDataConverter.Deserialize<TData>(rawData);
            foreach (var record in dataRecord)
            {
                var    listRecord = record.GetFieldInfoWithAttribute<KeyOfRecord>();
                switch (listRecord.Count)
                {
                    case 0: throw new Exception("");
                    case 1: this.Data.Add((TKey) listRecord.First().GetValue(record),record);
                        break;
                    case >1:
                        var multiKey = "";
                        listRecord.ForEach(field=>multiKey+=field.GetValue(record));
                        object key = multiKey;
                        this.Data.Add((TKey)key,record);
                        break;
                }
            }
        }

        public void SetupHandleData(DiContainer container)
        {
            if(this.GetCustomAttribute<DataInfoAttribute>().HandleLocalDataType==null) return;
            var handleDataType                                                                                   = this.GetCustomAttribute<DataInfoAttribute>().HandleLocalDataType;
            if (container.Resolve(handleDataType) is IHandleBlueprintData handleData) handleData.BlueprintData = this;
        }
        
    }
}
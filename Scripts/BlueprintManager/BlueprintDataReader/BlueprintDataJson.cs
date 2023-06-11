namespace UnityFoundation.Scripts.BlueprintManager.BlueprintDataReader
{
    using System.Collections.Generic;
    using UnityFoundation.Scripts.BlueprintManager.BlueprintBase;
    using UnityFoundation.Scripts.BlueprintManager.HandleBlueprintData;
    using Zenject;

    public abstract class BlueprintDataJson<TKey, TData> : IBlueprintData where TData : class
    {
        public Dictionary<TKey, TData> Data;
        public IHandleBlueprintData    HandleBlueprintData                    { get; set; }
        public void                    ConvertData(string rawData)            { }
        public void                    SetupHandleData(DiContainer container) { }
    }
}
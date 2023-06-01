namespace Data.BlueprintData
{
    using System;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class LocalResourceBlueprintJsonDataHandler : BlueprintJsonDataHandler
    {
        protected override async UniTask<string> GetJson(Type type)
        {
            return ((TextAsset)await Resources.LoadAsync<TextAsset>($"Blueprints/{type.Name}.json")).text;
        }
    }
}
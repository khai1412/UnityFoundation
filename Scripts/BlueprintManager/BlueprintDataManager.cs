namespace UnityFoundation.Scripts.BlueprintManager
{
    using System.Linq;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityFoundation.Scripts.BlueprintManager.BlueprintBase;
    using UnityFoundation.Scripts.Extensions;
    using Zenject;


    public class BlueprintDataManager
    {
        [Inject] private SignalBus signalBus; 
        public           float     CurrentLoadedProgress;
        
        public async void LoadAllData()
        {
            var listAllBlueprint = typeof(IBlueprintData).GetDerivedTypes();
            foreach (var blueprintData in listAllBlueprint.Where(data=>!data.IsAbstract).Select(dataType => (IBlueprintData)this.GetCurrentContainer().Resolve(dataType)))
            {
                var rawData = await this.GetRawData(blueprintData);
                blueprintData.ConvertData(rawData);
                blueprintData.SetupHandleData(this.GetCurrentContainer());
            }
            this.signalBus.Fire<LoadedAllBlueprintDataSignal>();
        }
        
        private async Task<string> GetRawData(IBlueprintData blueprint)
        {
            var dataPath = blueprint.GetCustomAttribute<DataInfoAttribute>().DataPath;
            var rawData  = (await Addressables.LoadAssetAsync<TextAsset>(dataPath).ToUniTask(Progress.CreateOnlyValueChanged<float>(progress =>
            {
                this.CurrentLoadedProgress += progress;
            }))).text;
            return rawData;
        }
    }
}
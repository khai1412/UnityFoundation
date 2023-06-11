namespace UnityFoundation.Scripts.BlueprintManager.BlueprintBase
{
    using Zenject;

    public interface IBlueprintData
    {
        void ConvertData(string rawData);
        void SetupHandleData(DiContainer container);
    }
}
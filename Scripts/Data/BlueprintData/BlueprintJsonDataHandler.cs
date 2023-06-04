namespace Data.BlueprintData
{
    using System;
    using Cysharp.Threading.Tasks;
    using Data.Base;

    public abstract class BlueprintJsonDataHandler : BaseJsonDataHandler
    {
        public override UniTask Flush()
        {
            return UniTask.CompletedTask;
        }

        public override bool CanHandle(Type type)
        {
            return typeof(IData).IsAssignableFrom(type);
        }

        protected override UniTask SaveJson(string json, Type type)
        {
            return UniTask.CompletedTask;
        }
    }
}
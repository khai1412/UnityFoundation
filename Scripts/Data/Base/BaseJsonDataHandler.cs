namespace Data.Base
{
    using System;
    using Cysharp.Threading.Tasks;
    using Newtonsoft.Json;

    public abstract class BaseJsonDataHandler : IDataHandler
    {
        public async UniTask Populate(IData data)
        {
            JsonConvert.PopulateObject(await this.GetJson(data.GetType()), data);
        }

        public async UniTask Save(IData data)
        {
            await this.SaveJson(JsonConvert.SerializeObject(data), data.GetType());
        }

        public abstract UniTask Flush();
        public abstract bool    CanHandle(Type type);

        protected abstract UniTask<string> GetJson(Type type);
        protected abstract UniTask         SaveJson(string json, Type type);
    }
}
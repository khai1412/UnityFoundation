namespace Data.PlayerData
{
    using System;
    using Data.Base;

    public abstract class PlayerJsonDataHandler : BaseJsonDataHandler
    {
        public override bool CanHandle(Type type)
        {
            return typeof(PlayerData).IsAssignableFrom(type);
        }
    }
}
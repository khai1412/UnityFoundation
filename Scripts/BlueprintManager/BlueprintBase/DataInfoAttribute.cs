namespace UnityFoundation.Scripts.BlueprintManager
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class DataInfoAttribute : Attribute
    {
        public readonly string DataPath;
        public readonly Type   HandleLocalDataType;
        public DataInfoAttribute(string dataPath,Type handleLocalDataType = null)
        {
            this.DataPath            = dataPath;
            this.HandleLocalDataType = handleLocalDataType;
        }
    }
}
namespace UnityFoundation.Scripts.BlueprintManager
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class DataInfoAttribute : Attribute
    {
        public string DataPath;
        public Type   HandleLocalDataType;
        public DataInfoAttribute(string dataPath,Type handleLocalDataType = null)
        {
            this.DataPath            = dataPath;
            this.HandleLocalDataType = handleLocalDataType;
        }
    }
}
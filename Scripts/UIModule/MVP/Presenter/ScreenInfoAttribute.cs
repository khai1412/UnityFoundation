namespace GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter
{
    using System;

    /// <summary> attributes to store basic information of a screen </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScreenInfoAttribute : Attribute
    {
        public string AddressableScreenPath { get; }
        public bool   IsOverlay             { get; }
      
        public ScreenInfoAttribute(string addressableScreenPath) { this.AddressableScreenPath = addressableScreenPath; }
    }
    
}
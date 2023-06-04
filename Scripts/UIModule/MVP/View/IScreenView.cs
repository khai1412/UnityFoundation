namespace GameFoundation.Scripts.UIModule.MVP.View
{
    using System;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public interface IScreenView
    {
        public RootUICanvas  ViewRoot       { get; set; }
        public GameObject    ViewMonoObject { get; }
        public RectTransform RectTransform  { get; }
        public bool          IsReadyToUse   { get; }
        public void          Open();
        public void          Close();
        public void          Hide();
        public void          Show();
        
        public void DestroySelf();

        public event Action OnViewClose;
        public event Action OnViewOpen;
        public event Action OnViewDestroy;
    }
}
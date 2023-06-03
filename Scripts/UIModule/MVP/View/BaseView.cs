namespace UnityFoundation.Scripts.UIModule.MVP.View
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule;
    using GameFoundation.Scripts.UIModule.MVP.View;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class BaseView : MonoBehaviour,IScreenView
    {
        private CanvasGroup   viewCanvas;
        public  RootUICanvas  ViewRoot       { get; set; }
        public  GameObject    ViewMonoObject { get => this.gameObject;}
        public  RectTransform RectTransform  { get; private set; }
        public  bool          IsReadyToUse   { get; private set; }
        
        private void Awake()
        {
            this.viewCanvas      = this.GetComponent<CanvasGroup>();
            this.RectTransform = this.GetComponent<RectTransform>();
            this.UpdateAlpha(false);
            this.IsReadyToUse = true;
        }
        public void Open()
        {
            this.UpdateAlpha(true);
            this.OnViewOpen?.Invoke();
        }
        public void Close()
        {
            this.UpdateAlpha(false);
            this.OnViewClose?.Invoke();
        }
        public void Hide()
        {
            this.UpdateAlpha(true);
        }
        public void Show()
        {
            this.UpdateAlpha(false);
        }
        
        public void DestroySelf()
        {
            Destroy(this.gameObject);
        }
        public event Action OnViewClose;
        public event Action OnViewOpen;
        public event Action OnViewDestroy;

        private void UpdateAlpha(bool isOpen)
        {
            this.viewCanvas.alpha = isOpen ? 1 : 0;
        }

        private void OnDestroy()
        {
            this.OnViewDestroy?.Invoke();
        }
    }
}
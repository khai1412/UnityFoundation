namespace UnityFoundation.Scripts.UIModule.MVP.Presenter
{
    using System;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.MVP.View;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityFoundation.Scripts.Extensions;
    using UnityFoundation.Scripts.UIModule.MVP.Model;
    using UnityFoundation.Scripts.UIModule.MVP.Signals;
    using UnityFoundation.Scripts.UIModule.MVP.View;
    using Zenject;

    public abstract class BaseScreenPresenter : IScreenPresenter
    {
        private readonly SignalBus         signalBus;
        private readonly ObjectPoolManager objectPoolManager;
        protected BaseScreenPresenter(SignalBus signalBus,ObjectPoolManager objectPoolManager)
        {
            this.signalBus         = signalBus;
            this.objectPoolManager = objectPoolManager;
        }

        private IScreenView  View          { get; set; }
        public  ScreenStatus CurrentStatus { get; set; }
        public async Task<TView> GetView<TView>() where TView : class, IScreenView
        {
            if (this.View == null)
            {
                this.SetView(await this.SetupView<TView>());
                return (TView)this.View;
            }
            if (!typeof(TView).IsSubclassOf(this.View.GetType()))
            {
                throw new Exception("can't get view!");
            }

            return this.View as TView;
        }

        public TView GetViewMono<TView>() where TView : class, IScreenView
        {
            return this.View as TView;
        }
        public void SetView(IScreenView view)
        {
            this.View = view;
            this.OnVewReady();
        }

        public async void OpenView()
        {
            await UniTask.WaitUntil(() => this.View != null && this.View.IsReadyToUse);
            this.View.Open();
            this.BindData();
            this.CurrentStatus = ScreenStatus.Opened;
            this.signalBus.Fire(new ShowScreenSignal() { Presenter = this,View = this.View});
        }
        public virtual void CloseView()
        {
            if (this.CurrentStatus == ScreenStatus.Closed) return;
            this.View.Close();
            this.CurrentStatus = ScreenStatus.Closed;
            this.signalBus.Fire(new CloseScreenSignal() { ScreenPresenter = this,View = this.View});
        }
        private async Task<TView> SetupView<TView>() where TView : IScreenView
        {
            var viewAssetPath = this.GetCustomAttribute<ScreenInfoAttribute>().AddressableScreenPath;
            var viewObject    = await this.objectPoolManager.Spawn(viewAssetPath);
            await UniTask.WaitUntil(() => viewObject.GetComponent<TView>().IsReadyToUse);
            return viewObject.GetComponent<TView>();
        }

        public virtual void HideView()
        {
            this.CurrentStatus = ScreenStatus.Hide;
            this.View.Hide();
        }
        public virtual void BindData()             { }

        public virtual void OnVewReady() { }


        public void OverrideView<TView>(TView view) where TView : BaseView
        {
            if (!view.GetType().IsSubclassOf(View.GetType())) throw new Exception("cannot over current view");
            this.View = view;
        }
        
    }

    public class BaseScreenPresenterWithModel : BaseScreenPresenter
    {
        private IModel    model;
        private SignalBus signalBus;
        public BaseScreenPresenterWithModel(SignalBus signalBus,ObjectPoolManager objectPoolManager) : base(signalBus, objectPoolManager)
        {
            this.signalBus         = signalBus;
        }
        public                 void BinData(IModel model) { this.model = model; }
        public sealed override void BindData()            { base.BindData(); }
    }
}
namespace UnityFoundation.Scripts.UIModule.MVP.Presenter
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule;
    using GameFoundation.Scripts.UIModule.MVP.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.HyperCasual.Extensions;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityFoundation.Scripts.UIModule.MVP.Model;
    using UnityFoundation.Scripts.UIModule.MVP.Signals;
    using UnityFoundation.Scripts.UIModule.MVP.View;
    using Zenject;

    public abstract class BaseScreenPresenter : IScreenPresenter
    {
        private readonly ObjectPoolManager objectPoolManager;
        private readonly SignalBus         signalBus;
        public BaseScreenPresenter(ObjectPoolManager objectPoolManager, SignalBus signalBus)
        {
            this.objectPoolManager = objectPoolManager;
            this.signalBus         = signalBus;
        }

        public IScreenView  View          { get; set; }
        public ScreenStatus CurrentStatus { get; set; }

        public void OpenView()
        {
            if (this.View == null || !this.View.IsReadyToUse) return;
            this.View.Open();
            this.BindData();
            this.CurrentStatus = ScreenStatus.Opened;
            this.signalBus.Fire(new ShowScreenSignal() { Presenter = this });
        }
        public virtual void CloseView()
        {
            if (this.CurrentStatus == ScreenStatus.Closed) return;
            this.View.Close();
            this.CurrentStatus = ScreenStatus.Closed;
            this.signalBus.Fire(new CloseScreenSignal() { ScreenPresenter = this });
        }

        public virtual void HideView()
        {
            this.CurrentStatus = ScreenStatus.Hide;
            this.View.Hide();
        }
        public virtual void BindData()             { }
        public virtual void BindData(IModel model) { }

        public virtual void OnVewReady() { }


        public void OverrideView<TView>(TView view) where TView : BaseView
        {
            if (!view.GetType().IsSubclassOf(View.GetType())) throw new Exception("cannot over current view");
            this.View = view;
        }

        public async void SetupView()
        {
            if (this.View != null) return;
            var viewAssetPath = this.GetCustomAttribute<ScreenInfoAttribute>().AddressableScreenPath;
            var viewObject    = await this.objectPoolManager.Spawn(viewAssetPath);
            this.View = viewObject.GetComponent<BaseView>();
            this.OnVewReady();
        }
    }

    public class BaseScreenPresenterWithModel : BaseScreenPresenter
    {
        public  IModel            Model;
        private ObjectPoolManager objectPoolManager;
        private SignalBus         signalBus;
        public BaseScreenPresenterWithModel(ObjectPoolManager objectPoolManager, SignalBus signalBus) : base(objectPoolManager, signalBus)
        {
            this.objectPoolManager = objectPoolManager;
            this.signalBus         = signalBus;
        }

        public void BinData(IModel model)
        {
            this.Model = model;
        }
        public sealed override void BindData() { base.BindData(); }
    }
}
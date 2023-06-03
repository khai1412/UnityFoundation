namespace UnityFoundation.Scripts.UIModule.MVP.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.Utilities.Extension;
    using TheOneStudio.HyperCasual.Extensions;
    using UnityEngine;
    using UnityFoundation.Scripts.UIModule.MVP.Presenter;
    using UnityFoundation.Scripts.UIModule.MVP.Signals;
    using Zenject;

    public class ScreenManager : IInitializable,IDisposable
    {
        public  Transform                          CurrentRootScreen  { get; set; }
        public  Transform                          CurrentHiddenRoot  { get; set; }
        public  Transform                          CurrentOverlayRoot { get; set; }
        public  RootUICanvas                       RootUICanvas       { get; set; }
        
        private Dictionary<Type, IScreenPresenter> loadedPresenters;
        private List<IScreenPresenter>             activeScreens;
        private SignalBus                          signalBus;

        public ScreenManager(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }
        
        public void Initialize()
        {
            this.loadedPresenters   = new();
            this.activeScreens = new();
            this.signalBus.Subscribe<ShowScreenSignal>(this.OnShowScreen);
            this.signalBus.Subscribe<CloseScreenSignal>(this.OnCloseScreen);
        }
        public async void OpenScreen<T>() where T : IScreenPresenter
        {
            if (!this.loadedPresenters.TryGetValue(typeof(T), out var screenPresenter))
            {
                var presenter = this.GetCurrentContainer().Instantiate<T>();
                this.loadedPresenters.Add(typeof(T), presenter);
                presenter.SetupView();
                await UniTask.WaitUntil(() => presenter.View.IsReadyToUse);
                presenter.OpenView();
                return;
            }
            screenPresenter.OpenView();
        }

        private void OnShowScreen(ShowScreenSignal signal)
        {
            var presenter = signal.Presenter;
            presenter.View.RectTransform.SetParent(this.CheckIsOverlay(presenter)?this.CurrentOverlayRoot:this.CurrentRootScreen);
            if (!this.activeScreens.Contains(presenter))
            {
                this.activeScreens.Add(presenter);
            }
        }
        
        public void CloseScreen<T>() where T : IScreenPresenter
        {
            if(!this.loadedPresenters.TryGetValue(typeof(T),out var presenter)) return;
            presenter.CloseView();
        }

        private void OnCloseScreen(CloseScreenSignal signal)
        {
            var presenter = signal.ScreenPresenter;
            presenter.View.RectTransform.parent = this.CurrentHiddenRoot;
            this.activeScreens.Remove(presenter);
        }

        public void HideScreen<T>() where T : IScreenPresenter
        {
            if(!this.loadedPresenters.TryGetValue(typeof(T),out var presenter)) return;
            presenter.HideView();
        }

        public void CloseAllScreen()
        {
            foreach (var screen in this.loadedPresenters.Where(screen => screen.Value.CurrentStatus != ScreenStatus.Closed))
            {
                this.activeScreens.Clear();
                screen.Value.CloseView();
            }
        }

        public void CloseCurrentScreen()
        {
            if(this.activeScreens.Count<=0) return;
            var currentPresenter = this.activeScreens.Last();
            currentPresenter.CloseView();
        }
        
      
        private bool CheckIsOverlay(IScreenPresenter screenPresenter) { return screenPresenter.GetType().IsSubclassOf(typeof(BaseScreenPresenter)) && screenPresenter.GetCustomAttribute<PopupInfoAttribute>().IsOverlay; }
        
        public void Dispose()
        {
            this.signalBus.Unsubscribe<ShowScreenSignal>(this.OnShowScreen);
            this.signalBus.Unsubscribe<CloseScreenSignal>(this.OnCloseScreen);
        }
    }
}
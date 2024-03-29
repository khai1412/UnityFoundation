﻿using System.Net.NetworkInformation;

namespace UnityFoundation.Scripts.UIModule.MVP.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.UIModule;
    using GameFoundation.Scripts.UIModule.MVP.View;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;
    using UnityFoundation.Scripts.Extensions;
    using UnityFoundation.Scripts.UIModule.MVP.Presenter;
    using UnityFoundation.Scripts.UIModule.MVP.Signals;
    using Zenject;

    public class ScreenManager : MonoBehaviour, IInitializable, IDisposable
    {
        public Transform    CurrentRootScreen  { get; set; }
        public Transform    CurrentHiddenRoot  { get; set; }
        public Transform    CurrentOverlayRoot { get; set; }
        public RootUICanvas RootUICanvas       { get; set; }

        private Dictionary<Type, IScreenPresenter> loadedPresenters = new();
        private IScreenPresenter                   activeScreen;
        private List<IScreenPresenter>             activePopup = new();
        private SignalBus                          signalBus;

        public ScreenManager(SignalBus signalBus) { this.signalBus = signalBus; }

        public void Initialize()
        {
            this.signalBus        = this.GetCurrentContainer().Resolve<SignalBus>();
            this.signalBus.Subscribe<ShowScreenSignal>(this.OnShowScreen);
            this.signalBus.Subscribe<CloseScreenSignal>(this.OnCloseScreen);
        }

        public async void OpenScreen<TView, TPresenter>() where TView : class, IScreenView
            where TPresenter : BaseScreenPresenter
        {
            if (this.loadedPresenters == null) this.loadedPresenters = new();
            if (!this.loadedPresenters.TryGetValue(typeof(TPresenter), out var screenPresenter))
            {
                var presenter = this.GetCurrentContainer().Instantiate<TPresenter>();
                await presenter.GetView<TView>();
                this.loadedPresenters.Add(typeof(TPresenter), presenter);
                presenter.OpenView();
                return;
            }

            screenPresenter.OpenView();
        }

        private void OnShowScreen(ShowScreenSignal signal)
        {
            var presenter = signal.Presenter;
            var view      = signal.View;
            view.RectTransform.SetParent(this.CheckIsOverlay(presenter)
                ? this.CurrentOverlayRoot
                : this.CurrentRootScreen);
            view.ViewMonoObject.transform.localPosition = Vector3.zero;
            view.ViewMonoObject.transform.localScale    = new Vector3(1, 1, 1);
            if (!this.CheckIsOverlay(presenter) && this.activeScreen != null)
            {
                this.activeScreen.CloseView();
                this.activeScreen = presenter;
            }
            if (!this.CheckIsOverlay(presenter) && this.activeScreen == null) {this.activeScreen = presenter;}
            if (this.CheckIsOverlay(presenter)) this.activePopup.Add(presenter);
        }

        public void CloseScreen<T>() where T : IScreenPresenter
        {
            if (!this.loadedPresenters.TryGetValue(typeof(T), out var presenter)) return;
            presenter.CloseView();
        }

        private void OnCloseScreen(CloseScreenSignal signal)
        {
            var presenter = signal.ScreenPresenter;
            signal.View.RectTransform.parent = this.CurrentHiddenRoot;
            if (this.CheckIsOverlay(presenter))
            {
                this.activePopup.Remove(presenter);
                return;
            }

            if (activeScreen == presenter) activeScreen = null;
        }

        public void HideScreen<T>() where T : IScreenPresenter
        {
            if (!this.loadedPresenters.TryGetValue(typeof(T), out var presenter)) return;
            presenter.HideView();
        }

        public void CloseAllScreen()
        {
            foreach (var screen in this.loadedPresenters.Where(screen =>
                         screen.Value.CurrentStatus != ScreenStatus.Closed))
            {
                this.activeScreen = null;
                screen.Value.CloseView();
            }
        }

        public void CloseCurrentScreen(bool isPopup = false)
        {
            if (!isPopup)
            {
                this.activeScreen?.CloseView();
                return;
            }

            this.activePopup.First().CloseView();
        }

        private bool CheckIsOverlay(IScreenPresenter screenPresenter)
        {
            return screenPresenter.GetType().IsSubclassOf(typeof(BaseScreenPresenter)) &&
                   screenPresenter.GetCustomAttribute<ScreenInfoAttribute>().IsOverlay;
        }

        public void Dispose()
        {
            this.signalBus.Unsubscribe<ShowScreenSignal>(this.OnShowScreen);
            this.signalBus.Unsubscribe<CloseScreenSignal>(this.OnCloseScreen);
        }
    }
}
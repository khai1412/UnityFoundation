namespace UnityFoundation.Scripts.UIModule.MVP.Presenter
{
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule;
    using GameFoundation.Scripts.UIModule.MVP.View;
    using UnityEngine;

    public interface IScreenPresenter
    {
        ScreenStatus       CurrentStatus { get; set; }
        void               OpenView();
        void               CloseView();
        void               HideView();
        void               BindData();
        void               OnVewReady();
        public Task<TView> GetView<TView>() where TView: class, IScreenView;
    }

    public enum ScreenStatus
    {
        Opened,
        Closed,
        Hide,
        Destroyed,
    }
}
namespace UnityFoundation.Scripts.UIModule.MVP.Presenter
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule;
    using GameFoundation.Scripts.UIModule.MVP.View;
    using UnityEngine;

    public interface IScreenPresenter
    {
        IScreenView View { get; set; }

        ScreenStatus CurrentStatus { get; set; }
        void         OpenView();
        void         CloseView();
        void         HideView();
        void         BindData();
        void         OnVewReady();
        void         SetupView();
    }

    public enum ScreenStatus
    {
        Opened,
        Closed,
        Hide,
        Destroyed,
    }
}
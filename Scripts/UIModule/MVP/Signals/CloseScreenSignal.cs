namespace UnityFoundation.Scripts.UIModule.MVP.Signals
{
    using GameFoundation.Scripts.UIModule.MVP.View;
    using UnityFoundation.Scripts.UIModule.MVP.Presenter;
    using UnityFoundation.Scripts.UIModule.MVP.View;

    public class CloseScreenSignal
    {
        public IScreenPresenter ScreenPresenter;
        public IScreenView      View;
    }
}
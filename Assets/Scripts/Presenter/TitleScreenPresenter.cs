using System;
using Model;
using Zenject;
using UniRx;
using View;

namespace Presenter
{
    public sealed class TitleScreenPresenter : IInitializable, IDisposable
    {
        readonly TitleScreenModel _titleScreenModel;
        readonly TitleScreenView _titleScreenView;
        
        readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        [Inject]
        TitleScreenPresenter(TitleScreenModel titleScreenModel, TitleScreenView titleScreenView)
        {
            _titleScreenModel = titleScreenModel;
            _titleScreenView = titleScreenView;
        }
        
        public void Initialize()
        {
            _titleScreenView.OnSinglePlayButtonClicked
                .Subscribe(_ => _titleScreenModel.EnterSinglePlayScreen());
            
            _titleScreenView.OnMultiPlayButtonClicked
                .Subscribe(_ => _titleScreenModel.EnterMultiPlayScreen());
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
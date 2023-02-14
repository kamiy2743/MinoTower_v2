```mermaid
classDiagram
    class TitleScreenModel {
        +EnterSinglePlayScreen()
        +EnterMultiPlayScreen()
    }

    class TitleScreenPresenter {

    }

    class TitleScreenView {
        +OnSinglePlayButtonClicked IObservable~Uint~
        +OnMultiPlayButtonClicked IObservable~Uint~
    }

    TitleScreenPresenter o--> TitleScreenModel
    TitleScreenPresenter o--> TitleScreenView
```
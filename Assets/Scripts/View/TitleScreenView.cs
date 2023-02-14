using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public sealed class TitleScreenView : MonoBehaviour
    {
        [SerializeField] Button singlePlayButton;
        [SerializeField] Button multiPlayButton;

        public IObservable<Unit> OnSinglePlayButtonClicked => singlePlayButton.OnClickAsObservable();
        public IObservable<Unit> OnMultiPlayButtonClicked => multiPlayButton.OnClickAsObservable();
    }
}
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    sealed class ResultScreenView : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI resultHeightText;
        [SerializeField] Button retryButton;
        [SerializeField] Button titleButton;

        IObservable<Unit> OnRetryButtonClicked => retryButton.OnClickAsObservable();
        IObservable<Unit> OnTitleButtonClicked => titleButton.OnClickAsObservable();

        internal void SetResultHeight(float height)
        {
            resultHeightText.text = $"{height:F1} m";
        }
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [SerializeField] CanvasGroup canvasGroup;

        internal async UniTask ShowAsync(CancellationToken ct)
        {
            canvasGroup.DOKill();
            canvasGroup.interactable = false;
            canvasGroup.gameObject.SetActive(true);
            
            canvasGroup.alpha = 1;
            await UniTask.DelayFrame(1, cancellationToken: ct);
            
            canvasGroup.interactable = true;
        }

        internal async UniTask HideAsync(CancellationToken ct)
        {
            canvasGroup.DOKill();
            
            canvasGroup.alpha = 0;
            await UniTask.DelayFrame(1, cancellationToken: ct);
            
            canvasGroup.interactable = false;
            canvasGroup.gameObject.SetActive(false);
        }

        internal void SetResultHeight(float height)
        {
            resultHeightText.text = $"{height:F1} m";
        }

        internal async UniTask<bool> WaitRetryOrBackToTitleAsync(CancellationToken ct)
        {
            var retryObservable = retryButton.OnClickAsObservable().First();
            var backToTitleObservable = titleButton.OnClickAsObservable().First();

            var result = await UniTask.WhenAny(
                retryObservable.ToUniTask(cancellationToken: ct),
                backToTitleObservable.ToUniTask(cancellationToken: ct)
            );

            return result.winArgumentIndex == 0;
        }
    }
}

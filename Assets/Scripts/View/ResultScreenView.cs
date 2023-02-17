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
            gameObject.SetActive(true);
            canvasGroup.DOKill();
            canvasGroup.gameObject.SetActive(true);
            
            await canvasGroup.DOFade(1, 0.5f).WithCancellation(ct);
        }

        internal async UniTask HideAsync(CancellationToken ct, bool immediate = false)
        {
            gameObject.SetActive(true);
            canvasGroup.DOKill();

            if (!immediate)
            {
                await canvasGroup.DOFade(0, 0.5f).WithCancellation(ct);
            }
            else
            {
                canvasGroup.alpha = 0;
            }
            
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

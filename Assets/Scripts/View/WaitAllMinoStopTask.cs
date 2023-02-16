using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace View
{
    static class WaitAllMinoStopTask
    {
        internal static UniTask Start(IEnumerable<MinoView> minoViews, CancellationToken ct)
        {
            return UniTask.Create(async () =>
            {
                while (true)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: ct);

                    var completed = true;
                    foreach (var minoView in minoViews)
                    {
                        if (!minoView.IsSleeping())
                        {
                            completed = false;
                            break;
                        }
                    }

                    if (completed)
                    {
                        return;
                    }
                }
            });
        }
    }
}
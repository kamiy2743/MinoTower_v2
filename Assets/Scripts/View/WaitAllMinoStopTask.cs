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

                    var sleeping = true;
                    foreach (var minoView in minoViews)
                    {
                        if (minoView.GetVelocity() > 0.2f)
                        {
                            sleeping = false;
                            break;
                        }
                    }

                    if (sleeping)
                    {
                        foreach (var minoView in minoViews)
                        {
                            minoView.Sleep();
                        }
                        break;
                    }
                }
            });
        }
    }
}
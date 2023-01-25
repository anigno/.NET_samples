#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

#endregion

namespace BlCommon
{
    public class RxExamples
    {
        #region Public Methods

        [Test]
        public void TestObservableTimerAndSample()
        {
            IObservable<long> observable = Observable.Timer(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(100));
            observable.Subscribe(p_long => TestUtils.DebugLog("Normal subscription,  EventIndex={0}", p_long));
            observable.Sample(TimeSpan.FromMilliseconds(200)).Subscribe(p_long => TestUtils.DebugLog("* every 200ms EventIndex={0}", p_long));
            observable.Sample(TimeSpan.FromMilliseconds(600)).Subscribe(p_long => TestUtils.DebugLog("- every 600ms EventIndex={0}", p_long));
            Task.Delay(1800).Wait();
        }

        [Test]
        public void TestObservableDelaySkipTake()
        {
            IObservable<long> observable = Observable.Timer(TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(100));
            observable.Subscribe(p_long => TestUtils.DebugLog("Normal subscription,  EventIndex={0}", p_long));
            observable.Delay(TimeSpan.FromMilliseconds(500)).Subscribe(p_long => TestUtils.DebugLog("* observable.Delay EventIndex={0}", p_long));
            observable.DelaySubscription(TimeSpan.FromMilliseconds(500)).Subscribe(p_long => TestUtils.DebugLog("- observable.DelaySubscription EventIndex={0}", p_long));
            observable.Skip(7).Subscribe(p_long => TestUtils.DebugLog("+ observable.Skip EventIndex={0}", p_long));
            observable.Take(TimeSpan.FromMilliseconds(300)).Subscribe(p_long => TestUtils.DebugLog("+ observable.Take EventIndex={0}", p_long));
            Task.Delay(1000).Wait();
        }

        #endregion
    }
}
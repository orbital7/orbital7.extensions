// Source: https://github.com/brminnick/AsyncAwaitBestPractices

using System;
using System.Reflection;

namespace Orbital7.Extensions.Events
{
    internal struct Subscription
    {
        public Subscription(WeakReference subscriber, MethodInfo handler)
        {
            Subscriber = subscriber;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public WeakReference Subscriber { get; }
        public MethodInfo Handler { get; }
    }
}

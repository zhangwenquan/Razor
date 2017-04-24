// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Microsoft.CodeAnalysis.Razor
{
    internal class SingleThreadedSynchronizationContext : SynchronizationContext
    {
        private readonly BlockingCollection<(SendOrPostCallback, object)> _queue = new BlockingCollection<(SendOrPostCallback, object)>();

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotSupportedException();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            _queue.Add((d, state));
        }

        public void Run()
        {
            while (_queue.TryTake(out var item, Timeout.Infinite))
            {
                item.Item1(item.Item2);
            }
        }

        public void Complete()
        {
            _queue.CompleteAdding();
        }
    }
}

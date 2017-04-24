// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor
{
    internal class DefaultThreadDispatcher : ThreadDispatcher
    {
        private readonly Thread _thread;

        public DefaultThreadDispatcher()
        {
            _thread = Thread.CurrentThread;
            
            ForegroundScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            BackgroundScheduler = TaskScheduler.Default;
        }

        public override TaskScheduler ForegroundScheduler { get; }

        public override TaskScheduler BackgroundScheduler { get; }

        protected override bool IsForegroundThread()
        {
            return Thread.CurrentThread == _thread;
        }
    }
}

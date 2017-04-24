// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor
{
    internal abstract class ThreadDispatcher
    {
        public abstract TaskScheduler ForegroundScheduler { get; }

        public abstract TaskScheduler BackgroundScheduler { get; }

        public void AssertForegroundThread([CallerMemberName] string caller = null)
        {
            if (!IsForegroundThread())
            {
throw new InvalidOperationException(Workspaces.Resources.FormatInvalidThreading_ForegroundThreadRequired(caller));
            }
        }

        public void AssertBackgroundThread([CallerMemberName] string caller = null)
        {
            if (IsForegroundThread())
            {
                throw new InvalidOperationException(Workspaces.Resources.FormatInvalidThreading_BackgroundThreadRequired(caller));
            }
        }

        public void AssertAnyThread([CallerMemberName] string caller = null)
        {
            // Do nothing
        }

        protected abstract bool IsForegroundThread();
    }
}

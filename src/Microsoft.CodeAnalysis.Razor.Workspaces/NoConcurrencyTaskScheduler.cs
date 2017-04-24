// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor
{
    // Executes items in order on the thread pool. Doesn't require a dedicated thread, but still
    // enforces single-threaded semantics.
    internal class NoConcurrencyTaskScheduler : TaskScheduler
    {
        // Per thread state - needed to inline tasks.
        [ThreadStatic]
        private static bool _threadExectutingTasks;

        private readonly object _lock;
        private readonly Queue<Task> _tasks;

        private bool _taskRunning;

        public NoConcurrencyTaskScheduler()
        {
            _lock = new object();
            _tasks = new Queue<Task>();
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_lock)
            {
                return _tasks.ToArray();
            }
        }

        protected override void QueueTask(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            lock (_lock)
            {
                _tasks.Enqueue(task);

                if (!_taskRunning)
                {
                    QueueWorker();
                }
            }
        }

        protected override bool TryDequeue(Task task)
        {
            // We only support dequeueing from the front of the queue.
            lock (_lock)
            {
                if (_tasks.Count > 0 && _tasks.First() == task)
                {
                    _tasks.Dequeue();
                    return true;
                }

                return false;
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // We can only execute a Task inline if this thread is currently processing the queue.
            if (!_threadExectutingTasks)
            {
                return false;
            }

            // This might be a continuation, so see if it's 'next'.
            if (taskWasPreviouslyQueued)
            {
                if (!TryDequeue(task))
                {
                    return false;
                }
            }

            // Ok we can actually run the task.
            return TryExecuteTask(task);
        }

        private void QueueWorker()
        {
            _taskRunning = true;

            ThreadPool.QueueUserWorkItem((state) =>
            {
                _threadExectutingTasks = true;

                try
                {
                    while (true)
                    {
                        Task task;
                        lock (_lock)
                        {
                            if (_tasks.Count == 0)
                            {
                                _taskRunning = false; // Allows another worker to be queued.
                                break;
                            }

                            task = _tasks.Dequeue();
                        }

                        TryExecuteTask(task);
                    }
                }
                finally
                {
                    _threadExectutingTasks = false;
                }
            });
        }
    }
}

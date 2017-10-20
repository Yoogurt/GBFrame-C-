using System;
using System.Collections.Generic;
using System.Threading;

namespace GBFrame.ThreadSupport
{
    class ContinutyThread
    {
        public readonly Thread RunThread;

        private readonly object mLock;

        private readonly Queue<ThreadStart> mQueue;

        public event ThreadStart TerminalHook;

        private bool _Finish;
        public bool Finish
        {
            get
            {
                return _Finish;
            }
            set
            {
                lock (mLock)
                {
                    _Finish = value;
                    Monitor.Pulse(mLock);
                }
            }
        }

        public ContinutyThread()
        {
            RunThread = new Thread(Run);
            mLock = new SemaphoreSlim(0, 1);
            mQueue = new Queue<ThreadStart>();
        }

        public void Start()
        {
            RunThread.Start();
        }

        private void Run()
        {
            ThreadStart entry;

            while (!Finish)
            {
                entry = null;

                lock (mLock)
                {
                    if (mQueue.Count <= 0 && !Finish)
                    {
                        Monitor.Wait(mLock);
                    }

                    if (!Finish)
                        entry = mQueue.Dequeue();
                }

                if (entry != null)
                    entry.Invoke();
                else break;
            }

            TerminalHook?.Invoke();
        }

        public void Post(ThreadStart task)
        {
            lock (mLock)
            {
                if (!Finish)
                {
                    mQueue.Enqueue(task);
                    Monitor.Pulse(mLock);
                }
                else
                    throw new Exception("ContinutyThread has Stopped");
            }
        }

        public void Post(IEnumerable<ThreadStart> tasks)
        {
            lock (mLock)
            {
                if (!Finish)
                {
                    foreach (var task in tasks)
                        mQueue.Enqueue(task);
                    Monitor.Pulse(mLock);
                }
                else
                    throw new Exception("ContinutyThread has Stopped");
            }
        }

        ~ContinutyThread()
        {
            if (!Finish)
                Finish = true;
        }
    }
}

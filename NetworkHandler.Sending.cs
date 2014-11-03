using System;
using System.Threading;
using MineLib.Network.Data;

namespace MineLib.Network
{
    public class CustomModuleAsyncResult<T> : IAsyncResult
    {
        private volatile Boolean _isCompleted;
        private ManualResetEvent _evt;
        private readonly AsyncCallback _cbMethod;
        private readonly Object _state;

        private readonly Object _locker = new Object();

        public CustomModuleAsyncResult(Func<T> workToBeDone, AsyncCallback cbMethod, Object state)
        {
            _cbMethod = cbMethod;
            _state = state;

            QueueWorkOnThreadPool(workToBeDone);
        }

        private void QueueWorkOnThreadPool(Func<T> workToBeDone)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    workToBeDone();
                }
                catch
                {
                    throw new NetworkHandlerException("Custom Module error: Method call caused an exception.");
                }
                finally
                {
                    UpdateStatusToComplete(); // -- 1 and 2
                    NotifyCallbackWhenAvailable(); // -- 3. Callback invocation
                }
            });
        }

        private void NotifyCallbackWhenAvailable()
        {
            if (_cbMethod != null)
                _cbMethod(this);
        }

        public object AsyncState
        {
            get { return _state; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return GetEvtHandle(); }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        private ManualResetEvent GetEvtHandle()
        {
            lock (_locker)
            {
                if (_evt == null)
                    _evt = new ManualResetEvent(false);

                if (_isCompleted)
                    _evt.Set();
            }

            return _evt;
        }

        private void UpdateStatusToComplete()
        {
            _isCompleted = true; // -- 1. Set _iscompleted to true

            lock (_locker)
            {
                if (_evt != null)
                    _evt.Set(); // -- 2. Set the event, when it exists             
            }
        }
    }

    public sealed partial class NetworkHandler
    {
        public IAsyncResult BeginConnectToServer(AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginConnectToServer(asyncCallback, state);
        }

        public IAsyncResult BeginKeepAlive(Int32 value, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginKeepAlive(value, asyncCallback, state);
        }

        public IAsyncResult BeginSendClientInfo(AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginSendClientInfo(asyncCallback, state);
        }

        public IAsyncResult BeginRespawn(AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginRespawn(asyncCallback, state);
        }

        public IAsyncResult BeginPlayerMoved(PlaverMovedData data, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginPlayerMoved(data, asyncCallback, state);
        }

        public IAsyncResult BeginPlayerSetRemoveBlock(PlayerSetRemoveBlockData data, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginPlayerSetRemoveBlock(data, asyncCallback, state);
        }

        public IAsyncResult BeginSendMessage(String message, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginSendMessage(message, asyncCallback, state);
        }

        public IAsyncResult BeginPlayerHeldItem(Int16 slot, AsyncCallback asyncCallback, Object state)
        {
            return _protocol.BeginPlayerHeldItem(slot, asyncCallback, state);
        }


        public void EndSend(IAsyncResult asyncResult)
        {
            _protocol.EndSendPacket(asyncResult);
        }
    }
}

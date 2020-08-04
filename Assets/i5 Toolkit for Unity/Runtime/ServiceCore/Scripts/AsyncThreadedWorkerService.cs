using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    public class AsyncThreadedWorkerService<OperationType>
        : IUpdateableService where OperationType : IOperation
    {
        private ConcurrentQueue<OperationType> queuedOperations = new ConcurrentQueue<OperationType>();
        private ConcurrentQueue<OperationType> finishedOperations = new ConcurrentQueue<OperationType>();

        public bool Enabled { get; set; } = true;

        public void Cleanup()
        {
        }

        public void Initialize(IServiceManager owner)
        {
        }

        public void Update()
        {
            OperationType operation;
            if (queuedOperations.TryDequeue(out operation))
            {
                StartOperation(operation);
            }

            OperationType finishedOperation;
            if (finishedOperations.TryDequeue(out finishedOperation))
            {
                finishedOperation.ReturnCallback();
            }
        }

        public void AddOperation(OperationType operation)
        {
            queuedOperations.Enqueue(operation);
        }

        private void StartOperation(OperationType operation)
        {
            // start thread here which computes AsyncOperation
            ThreadPool.QueueUserWorkItem(ExecuteOperation, operation);
        }

        private void ExecuteOperation(object callback)
        {
            OperationType operation = (OperationType)callback;
            AsyncOperation(operation);
            finishedOperations.Enqueue(operation);
        }

        protected virtual void AsyncOperation(OperationType operation)
        {
        }
    }

    public class Operation<ResultType> : IOperation
    {
        public OperationStatus status;
        public ResultType result;
        public Action<Operation<ResultType>> callback;

        public Operation(Action<Operation<ResultType>> callback) : base()
        {
            this.status = OperationStatus.WORKING;
            this.callback = callback;
        }

        public void ReturnCallback()
        {
            callback(this);
        }
    }

    public interface IOperation
    {
        void ReturnCallback();
    }

    public enum OperationStatus
    {
        WORKING, SUCCESS, ERROR
    }
}
using i5.Toolkit.Core.ServiceCore;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace i5.Toolkit.Core.ServiceCore
{
    /// <summary>
    /// Threaded worker service which can perform operations asynchronously on other threads
    /// </summary>
    /// <typeparam name="OperationType">The type of operation that should be performed</typeparam>
    public class AsyncThreadedWorkerService<OperationType>
        : IUpdateableService where OperationType : IOperation
    {
        private ConcurrentQueue<OperationType> queuedOperations = new ConcurrentQueue<OperationType>();
        private ConcurrentQueue<OperationType> finishedOperations = new ConcurrentQueue<OperationType>();

        /// <summary>
        /// If set to true, the update routine will run every frame
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Called by the ServiceManager to clean the service up when it is removed
        /// </summary>
        public void Cleanup()
        {
        }

        /// <summary>
        /// Called by the ServiceManager to initialize the service
        /// </summary>
        /// <param name="owner"></param>
        public void Initialize(IServiceManager owner)
        {
        }

        /// <summary>
        /// Called by the ServiceManager every frame
        /// If there is an operation waiting to be executed, it will be started
        /// If there are finished operations available, their callback method is called
        /// </summary>
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

        /// <summary>
        /// Adds an operation to the queue of operations to execute on a separate thread
        /// </summary>
        /// <param name="operation">The operation which should be executed</param>
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

    /// <summary>
    /// The operation to execute
    /// </summary>
    /// <typeparam name="ResultType">The type of the operation's return value</typeparam>
    public class Operation<ResultType> : IOperation
    {
        /// <summary>
        /// The status of the operation
        /// </summary>
        public OperationStatus status;
        /// <summary>
        /// The result of the operation once it is finished
        /// </summary>
        public ResultType result;
        /// <summary>
        /// The callback method which is invoked once the operation has finished
        /// The call will provide the result
        /// </summary>
        public Action<Operation<ResultType>> callback;

        /// <summary>
        /// Creates a new operation instance with the given callback method
        /// </summary>
        /// <param name="callback">The callback method which is invoked once the operation has finished</param>
        public Operation(Action<Operation<ResultType>> callback) : base()
        {
            this.status = OperationStatus.WORKING;
            this.callback = callback;
        }

        /// <summary>
        /// Invokes the callback method
        /// </summary>
        public void ReturnCallback()
        {
            callback(this);
        }
    }

    /// <summary>
    /// Contract which defines the interface of an operation
    /// </summary>
    public interface IOperation
    {
        void ReturnCallback();
    }

    /// <summary>
    /// The possible states of an operation
    /// </summary>
    public enum OperationStatus
    {
        WORKING, SUCCESS, ERROR
    }
}
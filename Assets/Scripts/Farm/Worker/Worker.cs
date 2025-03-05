using System;
using Data;
using Manager;
using UnityEngine;

namespace Farm.Worker
{
    public class Worker : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string plantAnimName;
        [Header("Moving")]
        [SerializeField] private Transform workerTransform;
        [SerializeField] private float moveSpeed;

        private bool isFree = true;
        private IState currentState = null;

        private Vector3 destinationPosition;

        private ProductResource workerResource;
        private Action<Worker> OnComplete { get; set; }
        private Action OnCompletePlant { get; set; }
        private int id = 0;
        private int cellAccess;

        #region Get - Set

        public int TimeExecute()
        {
            if(workerResource != null) return workerResource.TimeProcess;
            return 1;
        }
        public bool IsFree => isFree;
        public bool SetFree(bool value) => isFree = value;
        
        #endregion

        public void Initialized(int id, Action<Worker> onCompleteAction, ProductResource workerResource)
        {
            this.id = id;
            OnComplete = onCompleteAction;
            this.workerResource = workerResource;
            // Debug.Log(this.workerResource.TimeProcess);
        }

        public void Complete()
        {
            ChangeState(new WaitingState());
            // RaiseActionCompleted();
        }
        private void Update()
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
            Debug.Log(OnCompletePlant != null);
        }

        public void StartAccessCell()
        {
            GameController.Instance.UpdateWorker(id, cellAccess, DateTime.UtcNow);
        }
        public void RaiseActionCompleted()
        {
            Debug.Log("Raise Action Completed " );
            OnCompletePlant?.Invoke();
            OnComplete?.Invoke(this);
            
            // if(OnCompletePlant != null)
            //     OnCompletePlant = null;
        }

        public void MoveTo(Vector3 destinationPosition, Action onCompletePlant, int cellId)
        {
            this.destinationPosition = destinationPosition;
            OnCompletePlant = onCompletePlant;
            ChangeState(new MoveState());
            this.cellAccess = cellId;
            Debug.Log(OnCompletePlant != null);
        }
        public void Move()
        {
            workerTransform.position = Vector3.MoveTowards(workerTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
        }

        public bool ReachDestination()
        {
            return Vector3.Distance(workerTransform.position, destinationPosition) < 0.1f;
        }

        public void Waiting()
        {
            ChangeState(new WaitingState());
        }
        public void ChangeState(IState newState)
        {
            currentState?.OnExit(this);
            currentState = newState;
            currentState.OnEnter(this);
        }
    }
}
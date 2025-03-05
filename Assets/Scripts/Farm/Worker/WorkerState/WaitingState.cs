using Farm.Worker;
using UnityEngine;

public class WaitingState : IState
{
    public void OnEnter(Worker worker)
    {
        worker.SetFree(true);
    }

    public void OnExecute(Worker worker)
    {
        
    }

    public void OnExit(Worker worker)
    {
        
    }
}

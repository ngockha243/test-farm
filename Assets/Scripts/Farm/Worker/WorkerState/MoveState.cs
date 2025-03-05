using Farm.Worker;
using UnityEngine;

public class MoveState : IState
{
    public void OnEnter(Worker worker)
    {
        
    }

    public void OnExecute(Worker worker)
    {
        worker.Move();
        if (worker.ReachDestination())
        {
            worker.ChangeState(new PlantState());
        }
    }

    public void OnExit(Worker worker)
    {
        
    }
}

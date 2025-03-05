using Farm.Worker;
using UnityEngine;

public class PlantState : IState
{
    private float timer = 0;
    public void OnEnter(Worker worker)
    {
        timer = 0.0f;
        worker.SetFree(false);
        worker.StartAccessCell();
    }

    public void OnExecute(Worker worker)
    {
        timer += Time.deltaTime;
        if (timer >= worker.TimeExecute())
        {
            worker.ChangeState(new WaitingState());
            worker.RaiseActionCompleted();
        }
    }

    public void OnExit(Worker worker)
    {
    }
}

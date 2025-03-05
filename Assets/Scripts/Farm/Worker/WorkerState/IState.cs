using Farm.Worker;

public interface IState
{
    void OnEnter(Worker worker);
    void OnExecute(Worker worker);
    void OnExit(Worker worker);
}

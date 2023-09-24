public enum STATE
{
    INTRO,
    CONDUCCION,
    DESCARGA
};
public class GameState
{
    //observer.
    System.Action<STATE> OnChangeState;
    //state.
    private STATE _state;

    public STATE State { get => _state; }

    public void ChangeState(STATE state)
    {
        _state = state;
        OnChangeState(_state);
    }
}
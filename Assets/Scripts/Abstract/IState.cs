using System.Collections.Generic;

public interface IState
{
    void Enter();

    void Exit();

    void Tick();
}



public class StateMachine
{
    IState _currentState;

    readonly List<StateTransition> _stateTransitions;
    readonly List<StateTransition> _anyTransitions;

    public StateMachine()
    {
        _stateTransitions = new List<StateTransition>();
        _anyTransitions = new List<StateTransition>();
    }

    public void SetState(IState state)
    {
        if (state.Equals(_currentState)) return;

        _currentState?.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    public void Tick()
    {
        IState state = CheckState();

        if (state != null)
        {
            SetState(state);

        }


        _currentState.Tick();
    }

    private IState CheckState()
    {
        foreach (var stateTransition in _anyTransitions)
        {
            if (stateTransition.Condition.Invoke())
            {
                return stateTransition.To;
            }
        }

        foreach (var stateTransition in _stateTransitions)
        {
            if (stateTransition.Condition.Invoke() && stateTransition.From.Equals(_currentState))
            {
                return stateTransition.To;
            }

        }
        return null;
    }

    public void SetNormalState(IState from, IState to, System.Func<bool> condition)
    {
        _stateTransitions.Add(new StateTransition(from, to, condition));
    }

    public void SetAnyState(IState to, System.Func<bool> condition)
    {
        _anyTransitions.Add(new StateTransition(null, to, condition));


    }
}

public class StateTransition
{
    public IState From { get; }
    public IState To { get; }
    public System.Func<bool> Condition { get; }

    public StateTransition(IState from, IState to, System.Func<bool> condition)
    {
        From = from;
        To = to;
        Condition = condition;
    }
}




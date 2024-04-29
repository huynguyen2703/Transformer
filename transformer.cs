namespace Transformer;

public class Transformer
{
    // data section
    private readonly int _targetValue;
    private readonly int _threshold;
    protected uint NumQueries;
    protected uint HighData;
    protected uint LowData;
    protected bool InitialState;
    protected bool TargetKnown;
    protected State CurrentState;
    protected FirstTierOperation FirstTierType;

    public enum State
    {
        // 0 : Inactive, 1 : Active
        Inactive,
        Active,
        ShutDown
    }

    public enum FirstTierOperation
    {
        // 0 : Unknown, 1 : Sum, 2 : Difference
        Unknown,
        Sum,
        Difference
    }

    // properties section
    public uint GetNumQueries => NumQueries;
    public uint GetHighData => HighData;
    public uint GetLowData => LowData;
    public bool GetInitialState => InitialState;
    public bool IsTargetKnown => TargetKnown;
    public State GetState => CurrentState;
    public FirstTierOperation GetFirstTierOperation => FirstTierType;


    // methods section 
    public Transformer(int targetValue, int threshold = 0)
    {
        // other data initialized to correct default values
        _targetValue = targetValue;
        _threshold = threshold;
        InitialState = true;
        TargetKnown = false;
        FirstTierType = FirstTierOperation.Unknown;
    }


    public virtual double Transform(int guessValue)
    {
        if (TaskHelper(guessValue))
        {   // only if a guess hits correctly
            return 0;
        }

        if (guessValue > _threshold)
        {
            FirstTierType = FirstTierOperation.Difference;
            return Math.Abs(_targetValue - guessValue);
        }

        if (guessValue < _threshold)
        {
            FirstTierType = FirstTierOperation.Sum;
            return _targetValue + guessValue;
        }

        return -1; // guess unsuccessful
    }

    public virtual bool IsActive()
    {
        return CurrentState == State.Active;
    }

    public virtual bool IsInactive()
    {
        return CurrentState == State.Inactive;
    }

    public virtual bool IsShutDown()
    {
        return CurrentState == State.ShutDown;
    }

    public virtual void Reset()
    {
        NumQueries = 0;
        HighData = 0;
        LowData = 0;
        CurrentState = State.Inactive;
        InitialState = true;
        TargetKnown = false;
        FirstTierType = FirstTierOperation.Unknown;
    }

    public virtual void Activate()
    {
        CheckDead();
        if (IsInactive())
        {
            InitialState = false;
            CurrentState = State.Active;
        }
    }

    public virtual void Deactivate()
    {
        CheckDead();
        if (IsActive())
        {
            CurrentState = State.Inactive;
        }
    }

    protected virtual void PreCheck()
    {
        if (IsInactive() || IsShutDown())
        {
            throw new Exception("Invalid Request");
        }
    }

    protected virtual void CheckDead()
    {
        if (IsShutDown())
        {
            throw new Exception("Transformer is currently shut down");
        }
    }

    protected virtual bool TaskHelper(int guessValue)
    {
        PreCheck();
        NumQueries += 1;
        InitialState = false;
        if (guessValue < _targetValue)
        {
            LowData += 1;
        }
        else if (guessValue > _targetValue)
        {
            HighData += 1;
        }

        if (guessValue == _targetValue)
        {
            TargetKnown = true;
            CurrentState = State.ShutDown;
            return true; // guess successful
        }

        return false;
    }

    public bool IsSum()
    {
        return FirstTierType == FirstTierOperation.Sum;
    }

    public bool IsDifference()
    {
        return FirstTierType == FirstTierOperation.Difference;
    }

    public virtual bool IsUnknown()
    {
        return FirstTierType == FirstTierOperation.Unknown;
    }
}
namespace Transformer;

public class AccelerateTransformer : Transformer
{
    // data section
    private readonly int _targetValue;
    private double _accelerateFactor;
    private double _accelerateValue;


    // methods section
    public AccelerateTransformer(int targetValue, double accelerateFactor) : base(targetValue)
    {
        CheckAccelerateFactor(accelerateFactor);
        _targetValue = targetValue;
        _accelerateValue = targetValue;
        _accelerateFactor = accelerateFactor;
    }

    public override double Transform(int guessValue)
    {
        if (base.TaskHelper(guessValue))
        {
            // only if a guess hits correctly
            return 0;
        }

        return _accelerateValue + _accelerateFactor;
    }

    public override void Reset()
    {
        NumQueries = 0;
        HighData = 0;
        LowData = 0;
        CurrentState = State.Inactive;
        InitialState = true;
        TargetKnown = false;
        FirstTierType = FirstTierOperation.Unknown;
        _accelerateValue = _targetValue;
        _accelerateFactor = 0;
    }

    private void CheckAccelerateFactor(double accelerateFactor)
    {
        if (accelerateFactor <= 0)
        {
            throw new Exception("accelerator factor must be positive");
        }
    }
}
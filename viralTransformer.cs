
namespace Transformer;

public class ViralTransformer : Transformer
{
    // data section
    private readonly int _targetValue;
    private readonly double _moduloFactor;
    private SecondTierOperation _secondTierType;

    public enum SecondTierOperation
    {
        Unknown,
        Multiple,
        Modulo
    }

    // properties section
    public SecondTierOperation GetSecondTierOperation => _secondTierType;

    // methods section
    public ViralTransformer(int targetValue, double moduloFactor) : base(targetValue)
    {
        CheckModuloFactor(moduloFactor);
        _targetValue = targetValue;
        _moduloFactor = moduloFactor;
    }

    public override double Transform(int guessValue)
    {
        if (TaskHelper(guessValue))
        {
            // only if a guess hits correctly
            return 0;
        }

        if (_targetValue < _moduloFactor)
        {
            _secondTierType = SecondTierOperation.Multiple;
            return _targetValue * guessValue;
        }

        if (_targetValue > _moduloFactor)
        {
            _secondTierType = SecondTierOperation.Modulo;
            return (_targetValue * guessValue) % _moduloFactor;
        }

        return -1; // guess unsuccessful
    }

    private void CheckModuloFactor(double moduloFactor)
    {
        if (moduloFactor == 0)
        {
            throw new Exception("modulo factor cannot be 0");
        }
    }

    public override void Reset()
    {
        NumQueries = 0;
        HighData = 0;
        LowData = 0;
        CurrentState = State.Inactive;
        InitialState = true;
        TargetKnown = false;
        _secondTierType = SecondTierOperation.Unknown;

    }
    
    public bool IsProduct()
    {
        return _secondTierType == SecondTierOperation.Multiple;
    }

    public bool IsModulo()
    {
        return _secondTierType == SecondTierOperation.Modulo;
    }

    public override bool IsUnknown()
    {
        return _secondTierType == SecondTierOperation.Unknown;
    }
}
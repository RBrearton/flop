namespace Flop.Core;

/// <summary>
/// Health factors are, as the name suggests, factors that can affect the health of an Actor.
/// Examples include things like poison, hit points, blood loss, etc.
/// </summary>
public class HealthFactor(float currentValue, float maxValue, HealthFactorType type) : IDescribable
{
    /// <summary>
    /// The current value of the health factor.
    /// </summary>
    public float CurrentValue { get; private set; } = currentValue;

    /// <summary>
    /// The maximum value of the health factor.
    /// </summary>
    public float MaxValue { get; } = maxValue;

    /// <summary>
    /// The ratio of the health factor's current value to its maximum value.
    /// </summary>
    public float FractionValue => CurrentValue / MaxValue;

    /// <summary>
    /// The type of the health factor.
    /// </summary>
    public HealthFactorType Type { get; } = type;

    /// <summary>
    /// Whether higher is better for this health factor.
    /// </summary>
    public bool IsHigherBetter { get; } =
        type switch
        {
            HealthFactorType.HitPoints => true,
            HealthFactorType.BloodLoss => false,
            HealthFactorType.Poison => false,
            _ => throw new ArgumentException($"Invalid health factor type: {type}"),
        };

    /// <summary>
    /// The description of the health factor.
    /// </summary>
    public string Description { get; } =
        type switch
        {
            HealthFactorType.HitPoints => "Hit points",
            HealthFactorType.BloodLoss => "Blood loss",
            HealthFactorType.Poison => "Poison",
            _ => throw new ArgumentException($"Invalid health factor type: {type}"),
        };

    /// <summary>
    /// Increase the current value of the health factor by the given amount.
    /// </summary>
    /// <param name="amount">The amount to increase the current value by.</param>
    public void Increase(float amount)
    {
        CurrentValue = Math.Min(CurrentValue + amount, MaxValue);
    }

    /// <summary>
    /// Decrease the current value of the health factor by the given amount.
    /// </summary>
    /// <param name="amount">The amount to decrease the current value by.</param>
    public void Decrease(float amount)
    {
        CurrentValue = Math.Max(CurrentValue - amount, 0);
    }
}

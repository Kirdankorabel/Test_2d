public static class AbilityUser
{
    public static event System.Action<int> OnMoneyAdded;
    public static void Treat(int value)
    {
        Player.Instance.HealthController.TookDamage(-value);
    }
    public static void AddMoney(int value)
    {
        OnMoneyAdded?.Invoke(value);
    }
}

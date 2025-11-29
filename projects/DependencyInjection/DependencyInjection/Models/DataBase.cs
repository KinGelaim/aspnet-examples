namespace DependencyInjection.Models;

public abstract class DataBase
{
    public int Count { get; } = Random.Shared.Next(1, 1_000_000);
}

public sealed class TransientData : DataBase;

public sealed class ScopedData : DataBase;

public sealed class SingletonData : DataBase;
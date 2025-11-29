namespace DependencyInjection.Models;

public abstract class WrapperBase
{
    private readonly DataBase _data;

    public int Count => _data.Count;

    public WrapperBase(DataBase data)
    {
        _data = data;
    }
}

public sealed class TransientWrapper(TransientData data) : WrapperBase(data);

public sealed class ScopedWrapper(ScopedData data) : WrapperBase(data);

public sealed class SingletonWrapper(SingletonData data) : WrapperBase(data);

public sealed class SingletonWrapperWithScopedData(ScopedData data) : WrapperBase(data);
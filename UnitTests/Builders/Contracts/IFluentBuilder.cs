namespace UnitTests.Builders.Contracts
{
    internal interface IFluentBuilder<out T>
    where T : notnull
    {
        T Build();
    }

    internal interface IFluentBuilder<TKey, T> : IFluentBuilder<T>
    where TKey : struct
    where T : notnull
    {
        IDictionary<TKey, T> InstanceCache { get; set; }
    }
}

namespace BlApi
{
    public static class Factory
    {
        private static readonly Lazy<IBl> lazyBl = new Lazy<IBl>(() => new BlImplementation.Bl(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static IBl Get() => lazyBl.Value;
    }
}

namespace ConsumerOne.Mobile.Services
{
    public interface IApiService
    {
        IConsumerOneService Speculative { get; }
        IConsumerOneService UserInitiated { get; }
        IConsumerOneService Background { get; }
    }
}

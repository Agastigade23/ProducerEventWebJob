using ProducerEventWebJob;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IProducer, Producer>();
    })
    .Build();

await host.StartAsync();

await host.WaitForShutdownAsync();
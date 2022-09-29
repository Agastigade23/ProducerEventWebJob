namespace ProducerEventWebJob
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IProducer _producer;
        private Timer? _timer = null;
        public Worker(ILogger<Worker> logger,IProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _timer = new Timer(ProduceCharge, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(1000));

            return Task.CompletedTask;
        }

        public async void ProduceCharge(object? state)
        {
           string d=_producer.ProduceChargeEvent();

            _logger.LogInformation(
                "sessionID: {Count}", d);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
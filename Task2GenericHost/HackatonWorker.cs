namespace Task2GenericHost
{
    public class HackatonWorker : BackgroundService
    {
        private readonly ILogger<HackatonWorker> _logger;
        private readonly Hackaton _hackaton;

        private double _sumHarmonies;
        private int _hackatonCounter;

        public HackatonWorker(ILogger<HackatonWorker> logger, Hackaton hackaton)
        {
            _logger = logger;
            _hackaton = hackaton;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                double harmony = _hackaton.Hold();
                lock (this)
                {
                    _hackatonCounter++;
                    _sumHarmonies += harmony;
                    _logger.LogInformation("Hackaton #{i}, harmony level: {harmony}", _hackatonCounter, harmony);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            lock (this)
            {
                _logger.LogInformation("{total} hackatons passed, average harmony: {average}",
                    _hackatonCounter, _sumHarmonies / _hackatonCounter);
            }
            await base.StopAsync(cancellationToken);
        }
    }
}

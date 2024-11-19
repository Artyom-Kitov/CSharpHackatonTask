namespace Task5Http.Commons
{
    public class TimeoutPostHttpClient(HttpClient client, ILogger logger, string url, int timeoutMillis)
    {
        private readonly HttpClient _client = client;
        private readonly string _url = url;
        private readonly ILogger _logger = logger;
        private readonly int _timeoutMillis = timeoutMillis;

        public async Task Send<T>(T requestBody)
        {
            _logger.LogInformation("POST to {Host}", _url);
            bool isReachable = await SendRequest(requestBody);
            while (!isReachable)
            {
                _logger.LogWarning("POST to {Url} failed. Trying again in {Timeout} milliseconds...",
                    _url, _timeoutMillis);
                Thread.Sleep(_timeoutMillis);
                isReachable = await SendRequest(requestBody);
            }
            _logger.LogInformation("POST to {Host} successful!", _url);
        }

        private async Task<bool> SendRequest<T>(T body)
        {
            try
            {
                return (await _client.PostAsJsonAsync(_url, body)).IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

    }
}

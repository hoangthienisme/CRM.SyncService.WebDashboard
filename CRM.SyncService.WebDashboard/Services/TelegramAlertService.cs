namespace CRM.SyncService.WebDashboard.Services
{
    public class TelegramAlertService
    {
        private readonly string _botToken = "7743481184:AAG7mt4MYz4XBGb1-SeHd0nLMy2TM6OVxys";
        private readonly string _chatId = "-1003065878488";

        public async Task SendAlertAsync(string message)
        {
            using var client = new HttpClient();
            var url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("chat_id", _chatId),
                new KeyValuePair<string, string>("text", message)
            });
            await client.PostAsync(url, data);
        }
    }
}

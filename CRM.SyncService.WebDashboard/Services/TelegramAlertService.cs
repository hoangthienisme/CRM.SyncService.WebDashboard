namespace CRM.SyncService.WebDashboard.Services
{
    public class TelegramAlertService
    {
        private readonly string _botToken = "8458318076:AAHVLfO0LEoQjK8rfFbTCPDwWEk6MKIVjKk";
        private readonly string _chatId = "-5060501651";

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

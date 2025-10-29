//using System.Net.Http;
//using System.Text.Json;

//namespace CRM.SyncService.WebDashboard.Services
//{
//    public class TelegramService
//    {
//        private readonly string _botToken;
//        private readonly string _chatId;
//        private readonly HttpClient _http;

//        public TelegramService(IConfiguration config)
//        {
//            _botToken = config["Telegram:BotToken"]
//                ?? throw new ArgumentNullException("Missing Telegram BotToken");
//            _chatId = config["Telegram:ChatId"]
//                ?? throw new ArgumentNullException("Missing Telegram ChatId");

//            _http = new HttpClient
//            {
//                BaseAddress = new Uri($"https://api.telegram.org/bot{_botToken}/")
//            };
//        }

//        public async Task SendLogAsync(string message)
//        {
//            if (string.IsNullOrWhiteSpace(message)) return;

//            var url = $"sendMessage?chat_id={_chatId}&text={Uri.EscapeDataString(message)}";

//            var res = await _http.GetAsync(url);

//            if (!res.IsSuccessStatusCode)
//            {
//                var content = await res.Content.ReadAsStringAsync();
//                throw new Exception($"Telegram API error: {content}");
//            }
//        }
//    }
//}

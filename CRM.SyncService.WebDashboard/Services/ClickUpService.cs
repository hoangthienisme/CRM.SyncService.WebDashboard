using CRM.SyncService.WebDashboard.Models;
using Newtonsoft.Json;
using Npgsql.Internal;
using RestSharp;

namespace CRM.SyncService.WebDashboard.Services
{
    public class ClickUpService
    {
        private readonly IConfiguration _config;
        private readonly string _apiToken;
        private readonly string _listId;
        private readonly RestClient _client;

        public ClickUpService(IConfiguration config)
        {
            _config = config;
            _apiToken = _config["ClickUp:ApiToken"] ?? throw new ArgumentNullException("ApiToken missing");
            _listId = _config["ClickUp:ListId"] ?? throw new ArgumentNullException("ListId missing");

            _client = new RestClient("https://api.clickup.com/api/v2");
        }

        private static string FormatPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return phone ?? "";
            phone = phone.Trim();
            if (phone.StartsWith("0")) return "+84" + phone.Substring(1);
            if (phone.StartsWith("+")) return phone;
            return "+" + phone;
        }

        public async Task PushContactAsync(Contact contact)
        {
            var phoneFormatted = FormatPhone(contact.Phone);

            var body = new
            {
                name = contact.ContactName,
                custom_fields = new[]
                {
                    new { id = _config["ClickUp:CustomPhoneId"], value = phoneFormatted },
                    new { id = _config["ClickUp:CustomEmailId"], value = contact.Email }
                }
            };

            var request = new RestRequest($"/list/{_listId}/task", Method.Post)
                .AddHeader("Authorization", _apiToken)
                .AddHeader("Content-Type", "application/json")
                .AddStringBody(JsonConvert.SerializeObject(body), RestSharp.DataFormat.Json);

            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                var content = response.Content;
                throw new Exception($"ClickUp Sync Failed (StatusCode: {response.StatusCode}) — {content}");
            }
        }
    }
}

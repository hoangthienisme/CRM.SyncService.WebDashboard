//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Services;
//using Google.Apis.Sheets.v4;
//using Google.Apis.Sheets.v4.Data;
//using CRM.SyncService.WebDashboard.Models;

//namespace CRM.SyncService.WebDashboard.Services
//{
//    public class GoogleSheetService
//    {
//        private readonly IConfiguration _config;
//        private readonly string _spreadsheetId;
//        private readonly SheetsService _service;
//        private readonly string _sheetName;

//        public GoogleSheetService(IConfiguration config)
//        {
//            _config = config;

//            _spreadsheetId = _config["Google:SpreadsheetId"]
//                ?? throw new ArgumentNullException("Google SpreadsheetId missing");

//            _sheetName = _config["Google:SheetName"] ?? "Sheet1";

//            var credFile = _config["Google:CredentialsPath"] ?? "credentials.json";

//            if (!File.Exists(credFile))
//                throw new FileNotFoundException($"Google API credentials not found: {credFile}");

//            var credential = GoogleCredential.FromFile(credFile)
//                .CreateScoped(SheetsService.Scope.Spreadsheets);

//            _service = new SheetsService(new BaseClientService.Initializer
//            {
//                HttpClientInitializer = credential,
//                ApplicationName = "CRM Sync Dashboard"
//            });
//        }

//        public async Task UpdateContactAsync(Contact contact)
//        {
//            var range = $"{_sheetName}!A:F";

//            var values = new List<IList<object>>
//            {
//                new List<object>
//                {
//                    contact.ContactName ?? "",
//                    contact.Phone ?? "",
//                    contact.Email ?? "",
//                    contact.Status ?? "",
//                    contact.Source ?? "",
//                    contact.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
//                }
//            };

//            var valueRange = new ValueRange { Values = values };

//            var request = _service.Spreadsheets.Values.Append(valueRange, _spreadsheetId, range);
//            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest
//                                        .ValueInputOptionEnum.RAW;

//            await request.ExecuteAsync();
//        }
//    }
//}

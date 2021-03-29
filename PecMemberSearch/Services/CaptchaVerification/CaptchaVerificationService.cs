using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PecMemberSearch.Services.CaptchaVerification
{
    public class CaptchaVerificationService : ICaptchaVerificationService
    {
        private CaptchaSettings captchaSettings;
        private ILogger<CaptchaVerificationService> logger;

        public string ClientKey => captchaSettings.SiteKey;

        public CaptchaVerificationService(IOptions<CaptchaSettings> captchaSettings, ILogger<CaptchaVerificationService> logger)
        {
            this.captchaSettings = captchaSettings.Value;
            this.logger = logger;
        }

        public async Task<bool> IsCaptchaValid(string token)
        {
            var result = false;

            var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";

            try
            {
                using var client = new HttpClient();

                var response = await client.PostAsync($"{googleVerificationUrl}?secret={captchaSettings.SecretKey}&response={token}", null);
                var jsonString = await response.Content.ReadAsStringAsync();
                var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

                result = captchaVerfication.Success;
            }
            catch (Exception e)
            {
                // fail gracefully, but log
                logger.LogError("Failed to process captcha validation", e);
            }

            return result;
        }

        public class CaptchaSettings
        {
            public string SiteKey { get; set; }
            public string SecretKey { get; set; }
        }

        public class CaptchaVerificationResponse
        {
            public bool Success { get; set; }

            [JsonProperty("challenge_ts")]
            public DateTime ChallengeTimestamp { get; set; }
            public string Hostname { get; set; }

            [JsonProperty("error-codes")]
            public string[] Errorcodes { get; set; }


        }
    }
}

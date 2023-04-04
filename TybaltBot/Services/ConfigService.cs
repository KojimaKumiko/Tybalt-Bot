using Newtonsoft.Json;

namespace TybaltBot.Services
{
    public class ConfigService
    {
        public const string configFileName = "config.json";

        [JsonProperty]
        public string Token { get; set; } = string.Empty;

        [JsonProperty]
        public ulong GuildId { get; set; } = 0;

        [JsonProperty]
        public Dictionary<string, ulong> Channels { get; set; } = new Dictionary<string, ulong>();

        [JsonProperty]
        public Dictionary<string, ulong> Roles { get; set; } = new Dictionary<string, ulong>();

        private ConfigService? Config;

        public async Task<ConfigService?> LoadJsonAsync(string fileName, bool loadFresh = false)
        {
            if (Config != null && !loadFresh)
            {
                return Config;
            }

            if (File.Exists(fileName))
            {
                using var sr = new StreamReader(fileName);
                Config = JsonConvert.DeserializeObject<ConfigService>(await sr.ReadToEndAsync());
                return Config;
            }

            return null;
        }

        public ConfigService? LoadJson(string fileName, bool loadFresh = false)
        {
            return LoadJsonAsync(fileName, loadFresh).GetAwaiter().GetResult();
        }

        public void SaveJson(string fileName)
        {
            using var sw = new StreamWriter(fileName);
            sw.Write(JsonConvert.SerializeObject(this));
        }
    }
}

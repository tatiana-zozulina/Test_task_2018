using System.IO;
using Newtonsoft.Json;

namespace TwitterAnalytics
{
    public class Config
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public int TweetCount { get; set; }

        public static Config Load(string filename)
        {
            var data = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<Config>(data);
        }
    }
}

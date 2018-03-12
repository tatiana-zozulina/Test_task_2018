using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterAnalytics
{
    static class StatUtil
    {
        public static Dictionary<string, int> GetTextStat(string input, int tweetCount)
        {
            var userIdentifier = new UserIdentifier(input);
            var tweets = Timeline.GetUserTimeline(userIdentifier, tweetCount);
            if (tweets == null)
            {
                return null;
            }

            var charStat = new Dictionary<string, int>();
            foreach (var tweet in tweets)
            {
                MakeStat(tweet.Text, charStat);
            }
            return charStat;
        }

        public static string FormatStat(string input, int tweetCount, Dictionary<string, int> charStat)
        {
            var user = User.GetUserFromScreenName(input);
            var serialized = JsonConvert.SerializeObject(charStat, Formatting.None);
            return ($"@{user.ScreenName}, статистика для последних {tweetCount} твитов: {serialized}");
        }

        private static void MakeStat(string tweet, Dictionary<string, int> result)
        {
            tweet = tweet.ToLowerInvariant();
            var graphems = SplitByGraphemes(tweet);
            foreach (var letter in graphems)
            {
                if (result.ContainsKey(letter))
                    result[letter]++;
                else
                    result.Add(letter, 1);
            }
            return;
        }

        private static List<string> SplitByGraphemes(string value)
        {
            var result = new List<string>();
            var elementEnumerator = StringInfo.GetTextElementEnumerator(value);
            while (elementEnumerator.MoveNext())
            {
                var grapheme = elementEnumerator.GetTextElement();
                result.Add(grapheme);
            }
            return result;
        }
    }
}

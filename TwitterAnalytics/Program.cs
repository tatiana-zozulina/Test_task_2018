using System;
using Tweetinvi;
using System.Linq;

namespace TwitterAnalytics
{
    public class Program
    {
        private const string MESSAGE = "Программа выводит статистику последних 5 твитов запрашиваемого аккаунта на консоль и публикует её от имени бота.";
        private const string PROMPT = "Введите имя user(@user):";
        private const string GET_FAIL = "Не удалось получить твиты. Возможно, неверное имя пользователя либо не удалось установить соединение.";
        private const string DEFAULT_CONFIG = "./config.json";

        static void Main(string[] args)
        {
            var configFile = args.FirstOrDefault() ?? DEFAULT_CONFIG;
            var config = Config.Load(configFile);
            Auth.SetUserCredentials(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessTokenSecret);
            Console.WriteLine(MESSAGE);
            Console.WriteLine(PROMPT);
            var input = ReadName();
            while (input != "")
            {
                var stat = StatUtil.GetTextStat(input, config.TweetCount);
                if (stat != null)
                {
                    var result = StatUtil.FormatStat(input, config.TweetCount, stat);
                    Console.WriteLine(result);
                    TryPublishTweet(result);
                }
                else
                {
                    Console.WriteLine(GET_FAIL);
                }
                Console.WriteLine(PROMPT);
                input = ReadName();
            }
        }

        private static string ReadName()
        {
            return Console.ReadLine().Replace("@", "").Trim();
        }

        private static void TryPublishTweet(string tweet)
        {
            try
            {
                var tweetCut = tweet.Length <= 275 
                    ? tweet 
                    : tweet.Substring(0, 275);
                Tweet.PublishTweet(tweetCut + "...}");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"Не удалось опубликовать статистику. {e}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MetroLog;
using MetroLog.Targets;

namespace TealiumUWP
{
    public class Tealium
    {

        private static string visitorId;

        private static string sessionId;

        private static string account;

        private static string profile;

        private static string environment;

        private static ILogger logger;

        private static MetroLog.LogLevel logLevel;

        private Tealium()
        { }

        public static void Initialize(string account, string profile, string environment, LogLevel logLevel)
        {
            if (account != null && account != "" && account.Trim(' ')!= "")
                Tealium.account = account;
            if (environment != null && environment != "" && environment.Trim(' ') != "")
                Tealium.environment = environment;
            if (profile != null && profile != "" && profile.Trim(' ') != "")
                Tealium.profile = profile;

            Tealium.logLevel = (MetroLog.LogLevel)logLevel;

            try
            {
                LogManagerFactory.DefaultConfiguration.AddTarget(MetroLog.LogLevel.Trace, MetroLog.LogLevel.Fatal, new StreamingFileTarget());
                logger = LogManagerFactory.DefaultLogManager.GetLogger<Tealium>();

                ResetSessionId();
                visitorId = Util.GetVisitorId();
            }
            catch (Exception e)
            {
                logger.Log((MetroLog.LogLevel)logLevel, e.Message, e);
            }
        }

        public static void Track(string name)
        {
            Track(name, null, null);
        }
        public static void Track(string name, Dictionary<string, string> data)
        {
            Track(name, data, null);
        }
        public static void Track(string name,  Action<bool> callBackFunction = null)
        {
            Track(name, null, callBackFunction);
        }
        public static void Track(string name, Dictionary<string, string> data , Action<bool> callBackFunction )
        {
            string finalURL;
            try
            {
                finalURL = MakeUrlString(name, data);
                var response = Task.Run(() => SendGetRequest(finalURL));
                response.Wait();
                callBackFunction?.Invoke(response.Result);
            }
            catch (Exception e)
            {
                logger.Log(logLevel, e.Message, e);
            }
        }
        
        public static void ResetSessionId()
        {
            sessionId = Util.GetSessionId();   
        }

        public static void SetSessionId(string value)
        {
            if(value != null && value.Trim(' ') != "")
                sessionId = value;
        }

        public static void SetVisitorId(string value)
        {
            if (value != null && value.Trim(' ') != "")
                visitorId = value;
        }

        static bool SendGetRequest(string url)
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var response = hc.GetAsync(url);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    logger.Log(logLevel, e.Message, e);
                }
            }
            return false;
        }

        private static string MakeUrlString(string name, Dictionary<string, string> data = null)
        {
            String finalURL;
            finalURL = "tealium_account=" + Uri.EscapeDataString(account) + "&tealium_profile="
                + Uri.EscapeDataString(profile) + "&tealium_vid=" + Uri.EscapeDataString(visitorId) + "&tealium_event=" + Uri.EscapeDataString(name) +
                "&tealium_session_id=" + Uri.EscapeDataString(Util.GetSessionId()) + "&tealium_environment=" + Uri.EscapeDataString(environment) + "&tealium_random=" + Uri.EscapeDataString(Util.GenerateRandomNumber()) +
                "&tealium_library_name=" + Uri.EscapeDataString("windows") + "&tealium_library_version=" + Uri.EscapeDataString("1.0.0") + " & tealium_timestamp_epoch=" +
                Uri.EscapeDataString(Util.GenerateTimeStampEpoch()) + "&tealium_timestamp_utc=" +
                Uri.EscapeDataString(DateTime.UtcNow.ToString()) + "&tealium_timestamp_local=" + Uri.EscapeDataString(DateTime.Now.ToString()) +
                "&tealium_visitor_id=" + Uri.EscapeDataString(visitorId) + "&tealium_datasource=" + Uri.EscapeDataString("");
            if (data != null)
            {
                foreach (var pair in data)
                {
                    finalURL += "&" + pair.Key + "=" + Uri.EscapeDataString(pair.Value);
                }
            }
            finalURL = Constants.T_BASE_URL + finalURL;
            return finalURL;
        }

        public enum LogLevel
        {
            Trace = 0,
            DEBUG = 1,
            INFO = 2,
            WARN = 3,
            ERROR = 4,
            Fatal = 5
        }
    }
}

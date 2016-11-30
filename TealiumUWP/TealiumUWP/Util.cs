using System;
using System.Collections.Generic;

/*
 Utility Functions
 */

namespace TealiumUWP
{
    class Util
    {

        static public string GenerateRandomNumber()
        {
            Random r = new Random();
            int rInt = r.Next(100000000, 999999999);
            return rInt.ToString();
        }

        static public string GenerateTimeStampEpoch()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            return secondsSinceEpoch.ToString();
        }

        public static string GetSessionId()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long secondsSinceEpoch = (long)t.TotalMilliseconds;
            return secondsSinceEpoch.ToString();
        }

        public static string GetVisitorId()
        {
            return Windows.System.UserProfile.AdvertisingManager.AdvertisingId;
        }

    }
}

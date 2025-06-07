using System;

namespace DFun.Bookmarks
{
    public static class TimeUtils
    {
        private static readonly DateTime UnixEpoch = new DateTime(
            1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// Seconds since Jan 1, 1970
        public static long TimestampUtc => (long)(NowUtc - UnixEpoch).TotalSeconds;

        public static DateTime NowUtc => DateTime.UtcNow;

        public static long SecondsSinceTimeUtc(DateTime time)
        {
            return (long)(NowUtc - time).TotalSeconds;
        }

        public static long MillisecondsSinceTimeUtc(DateTime time)
        {
            return (long)(NowUtc - time).TotalMilliseconds;
        }
    }
}
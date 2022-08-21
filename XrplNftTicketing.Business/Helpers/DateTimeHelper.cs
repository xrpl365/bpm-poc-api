using System;

namespace XrplNftTicketing.Business.Helpers
{
    public static class DateTImeHelper
    { 
        public static string EventDisplayDateTime(this DateTime value) => value.ToString("d MMM yyyy 'at' h:mm") + value.ToString("tt").ToLower();
        
    }
}
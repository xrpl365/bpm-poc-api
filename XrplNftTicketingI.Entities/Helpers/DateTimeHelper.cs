using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace XrplNftTicketing.Entities.Helpers
{
    public static class DateTImeHelper
    { 
        public static string EventDisplayTime(this DateTime value) => value.ToString("d MMM yyyy 'at' h:mm") + value.ToString("tt").ToLower();
        
    }
}
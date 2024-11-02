using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Helpers
{
    public static class SiteListingParser
    {
        public static List<CamplyNotification> ParseNotifications(string notifications)
        {
            var notificationsList = new List<CamplyNotification>();
            var regex = new Regex(@"(?<campsiteName>.*?) \(#(?<campsiteId>\d+)\)");

            foreach (Match match in regex.Matches(notifications))
            {
                var campsiteName = match.Groups["campsiteName"].Value.Trim();
                var campsiteId = match.Groups["campsiteId"].Value;
                var notification = new CamplyNotification
                {
                    Site = campsiteName,
                    CampsiteId = campsiteId
                };
                notificationsList.Add(notification);
            }

            return notificationsList;
        }
    }

    public class CamplyNotification
    {
        public string Site { get; set; }
        public string CampsiteId { get; set; }
    }
}
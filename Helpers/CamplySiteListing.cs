using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Helpers
{
    public static class SiteListingParser
    {
        public static List<CamplyNotification> ParseNotifications(string notifications)
        {
            var notificationsList = new List<CamplyNotification>();
            var regex = new Regex(@"Campsite non-electric #(?<campsiteNumber>\d+) - \(#(?<campsiteId>\d+)\)");

            foreach (Match match in regex.Matches(notifications))
            {
                var campsiteNumber = match.Groups["campsiteNumber"].Value;
                var campsiteId = match.Groups["campsiteId"].Value;
                var notification = new CamplyNotification
                {
                    Site = $"Campsite non-electric #{campsiteNumber}",
                    CampsiteNumber = campsiteNumber,
                    CampsiteId = campsiteId
                };
                notificationsList.Add(notification);
            }

            return notificationsList;
        }
    }
}
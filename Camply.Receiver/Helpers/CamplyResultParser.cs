using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Models;

namespace Helpers
{
    public static class CamplyResultParser
    {
        public static List<CampsiteSearchResult> ExtractAndParseResults(string stdout)
        {
            var results = new List<CampsiteSearchResult>();
            Console.WriteLine(stdout);
            // Define regular expressions to capture different parts of the output
            var datePattern = new Regex(@"ranging from (?<DateRange>\d{4}-\d{2}-\d{2} to \d{4}-\d{2}-\d{2})");
            var campgroundPattern = new Regex(@"\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\] INFO\s+\p{So}\s+(?<Campground>[^\(]+)");
            var urlPattern = new Regex(@"https?://[^\s]+");
            var noReservablePattern = new Regex(@"0 Reservable Campsites Matching Search Preferences");

            // Temporary storage for capturing information for each site entry
            string campgroundName = string.Empty;
            string url = string.Empty;

            // Extract campground name
            var campgroundMatch = campgroundPattern.Match(stdout);
            if (campgroundMatch.Success)
            {
                campgroundName = campgroundMatch.Groups["Campground"].Value.Trim();
            }

            // Extract URL if available
            var urlMatch = urlPattern.Match(stdout);
            if (urlMatch.Success)
            {
                url = urlMatch.Value.Trim();
            }

            // Check if there are no reservable campsites
            if (noReservablePattern.IsMatch(stdout))
            {
                // If no reservable campsites, create a result with default values
                results.Add(new CampsiteSearchResult
                {
                    SiteId = null,
                    CampgroundName = campgroundName,
                    AvailableDate = null,
                    SiteUrl = null,
                    IsReservable = false,
                    AvailableSites = 0
                });
            }
            else
            {
                // Loop over each date match in the stdout log
                foreach (Match match in datePattern.Matches(stdout))
                {
                    var availableDate = match.Groups["DateRange"].Value;

                    // Assume if we have dates, there may be available sites
                    var result = new CampsiteSearchResult
                    {
                        SiteId = Guid.NewGuid().ToString(),
                        CampgroundName = campgroundName,
                        AvailableDate = availableDate,
                        SiteUrl = url,
                        IsReservable = true, // Setting true since we're in the available case
                        AvailableSites = 1 // Placeholder, update if more logic for site count is added
                    };

                    results.Add(result);
                }
            }

            return results;
        }
    }
}
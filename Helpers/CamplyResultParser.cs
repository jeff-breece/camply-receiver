using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Helpers
{
    public static class CamplyResultParser
    {
        public static CampsiteSearchResult ParseResult(string stringA, string stringB, string stringC)
        {
            var result = new CampsiteSearchResult();

            // Parse NumSites from String A
            var numSitesMatch = Regex.Match(stringA, @"(\d+) Reservable Campsites");
            if (numSitesMatch.Success && int.TryParse(numSitesMatch.Groups[1].Value, out int numSites))
            {
                result.NumSites = numSites;
            }

            // Parse DateAvailable from String B
            var dateAvailableMatch = Regex.Match(stringB, @"\[(.*?)\]");
            if (dateAvailableMatch.Success && DateTime.TryParse(dateAvailableMatch.Groups[1].Value, out DateTime dateAvailable))
            {
                result.DateAvailable = dateAvailable;
            }

            // Parse Campground from String C
            var campgroundMatch = Regex.Match(stringC, @"Searching (.+) for availability");
            if (campgroundMatch.Success)
            {
                result.Campground = campgroundMatch.Groups[1].Value;
            }

            return result;
        }

        public static CampsiteSearchResult ExtractAndParseResult(string pythonOutput)
        {
            // Extract the needed parts from the output
            string stringA = string.Empty;
            string stringB = string.Empty;
            string stringC = string.Empty;

            // Extract "NumSites" information from the output
            var numSitesMatch = Regex.Match(pythonOutput, @"(\d+) Reservable Campsites Matching Search Preferences");
            if (numSitesMatch.Success)
            {
                stringA = numSitesMatch.Value;
            }

            // Extract "DateAvailable" information from the output
            var dateAvailableMatch = Regex.Match(pythonOutput, @"\[(.*?)\]");
            if (dateAvailableMatch.Success)
            {
                stringB = dateAvailableMatch.Value;
            }

            // Extract "Campground" information from the output
            var campgroundMatch = Regex.Match(pythonOutput, @"Searching .+ for availability");
            if (campgroundMatch.Success)
            {
                stringC = campgroundMatch.Value;
            }

            // Parse the extracted strings and return the result
            return ParseResult(stringA, stringB, stringC);
        }
    }
}
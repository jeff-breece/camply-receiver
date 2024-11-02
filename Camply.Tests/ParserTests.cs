using NUnit.Framework;
using System.Text.RegularExpressions;
using Helpers;

[TestFixture]
public class CamplyResultParserTests
{
    private static readonly string sampleOutput = @"
        [2024-11-01 13:32:06] INFO ⛰ Scioto Trail State Park
        [2024-11-01 13:32:06] INFO 1 booking nights selected for search, ranging from 2024-11-01 to 2024-11-02
        [2024-11-01 13:32:06] INFO 0 Reservable Campsites Matching Search Preferences
        For more information, visit https://example.com/campsite";

    [Test]
    public void ExtractAndParseResults_CampgroundPattern()
    {
        var campgroundPattern = new Regex(@"INFO\s+⛰\s+(?<CampgroundName>.+?)\s*$", RegexOptions.Multiline);
        var match = campgroundPattern.Match(sampleOutput);

        Assert.IsTrue(match.Success, "Campground pattern did not match.");
        Assert.AreEqual("Scioto Trail State Park", match.Groups["CampgroundName"].Value.Trim(), "Extracted campground name did not match expected value.");
    }

    [Test]
    public void ExtractAndParseResults_UrlPattern()
    {
        var urlPattern = new Regex(@"https?://[^\s]+", RegexOptions.Multiline);
        var match = urlPattern.Match(sampleOutput);

        Assert.IsTrue(match.Success, "URL pattern did not match.");
        Assert.AreEqual("https://example.com/campsite", match.Value);
    }

    [Test]
    public void ExtractAndParseResults_DatePattern()
    {
        var datePattern = new Regex(@"ranging from (?<StartDate>\d{4}-\d{2}-\d{2}) to (?<EndDate>\d{4}-\d{2}-\d{2})");
        var match = datePattern.Match(sampleOutput);

        Assert.IsTrue(match.Success, "Date pattern did not match.");
        Assert.AreEqual("2024-11-01", match.Groups["StartDate"].Value);
        Assert.AreEqual("2024-11-02", match.Groups["EndDate"].Value);
    }

    [Test]
    public void ExtractAndParseResults_NoReservablePattern()
    {
        var noReservablePattern = new Regex(@"INFO 0 Reservable Campsites Matching Search Preferences");
        var match = noReservablePattern.Match(sampleOutput);

        Assert.IsTrue(match.Success, "No reservable pattern did not match.");
        Assert.AreEqual("INFO 0 Reservable Campsites Matching Search Preferences", match.Value);
    }
}

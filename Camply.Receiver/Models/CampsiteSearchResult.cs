public class CampsiteSearchResult
{
    public string SiteId { get; set; }
    public string CampgroundName { get; set; }
    public string AvailableDate { get; set; }
    public string SiteUrl { get; set; }
    public bool IsReservable { get; set; }
    public int AvailableSites { get; internal set; }
}

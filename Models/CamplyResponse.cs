using System.Collections.Generic;

namespace Models
{
    public class CamplyResponse
    {
        public string Command { get; set; } = string.Empty;
        public List<CampsiteSearchResult> Results { get; set; } = new List<CampsiteSearchResult>();
        public string StdErr { get; set; } = string.Empty;

        public string StdOut { get; set; }
    }
}

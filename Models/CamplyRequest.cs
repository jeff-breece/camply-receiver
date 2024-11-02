public record CamplyRequest
{
    public string CommandType { get; set; }
    public Dictionary<string, string> Options { get; set; }
}

public record CamplyResponseData
{
    public string Stdout { get; set; }
    public string Stderr { get; set; }
    public int Returncode { get; set; }
}

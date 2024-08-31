public class CamplyResponse
{
    public string command { get; set; }
    public string stdout { get; set; }
    public string stderr { get; set; }
    public int returncode { get; set; }
}
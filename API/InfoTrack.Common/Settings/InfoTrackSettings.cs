namespace InfoTrack.Common.Settings;

public class InfoTrackSettings
{
    public bool UseDummyResponse { get; set; }
    public SearchEngineSettings Google { get; set; }
    public SearchEngineSettings Yahoo { get; set; }
}
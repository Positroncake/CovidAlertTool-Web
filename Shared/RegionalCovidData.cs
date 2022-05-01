namespace CovidAlertTool.Shared;

public class RegionalCovidData
{
    public String Updated { get; set; } = String.Empty;
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public Int32 Confirmed { get; set; }
    public Int32 Deaths { get; set; }
    public Double Rate { get; set; }
}
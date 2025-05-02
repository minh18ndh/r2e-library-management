namespace LibraryManagement.Api.Models;

public class Error
{
    public Error(string title, string detail)
    {
        Title = title;
        Detail = detail;
    }

    public string Title { get; set; }
    public string Detail { get; set; }
}
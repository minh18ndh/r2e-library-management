namespace LibraryManagement.Api.Models;

public class Result
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<Error> Errors { get; set; } = new();
}
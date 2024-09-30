using System.ComponentModel.DataAnnotations;

public class SearchGradeRequest
{
    public double? minGrade { get; set; }
    public double? maxGrade { get; set; }
}
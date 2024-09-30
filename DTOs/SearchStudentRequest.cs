
using System.ComponentModel.DataAnnotations;
public class SearchStudentRequest
{
    public string? IdEstudiante { get; set; }
    [RegularExpression(@"^[A-Za-záéíóúüñÑÁÉÍÓÚÜ\s]+$", ErrorMessage = "El nombre del estudiante solo puede contener letras.")]
    public string? NombreEstudiante { get; set; }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Grade
{
    [BsonId]
    public Guid Id { get; set; } // UUID v4 como clave primaria
    public string Asignatura { get; set; } = string.Empty;
    public string NombreCalificacion { get; set; } = string.Empty;
    public string ComentarioCalificacion { get; set; } = string.Empty;
    public double Calificacion { get; set; }
    public Guid IdEstudiante { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public string ApellidoEstudiante { get; set; } = string.Empty;
    public string CorreoEstudiante { get; set; } = string.Empty;
}
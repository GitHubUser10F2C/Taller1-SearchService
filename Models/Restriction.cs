using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Restriction
{
    [BsonId]
    public Guid Id { get; set; } // UUID v4 como clave primaria
    public string Razon { get; set; } = string.Empty;
    public DateTime FechaRestriccion { get; set; }
    public Guid IdEstudiante { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public string ApellidoEstudiante { get; set; } = string.Empty;
    public string CorreoEstudiante { get; set; } = string.Empty;
}
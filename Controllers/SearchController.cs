using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly MongoDbContext _context;

    public SearchController(MongoDbContext context)
    {
        _context = context;
        InitializeDB(); //inicializa DB y agrega datos para test si las tablas están vacías
    }

    private void InitializeDB()
    {
        _context.Database.EnsureCreated();

        if (!_context.Grades.Any())
        {
            _context.Grades.AddRange(
                new Grade { Id = Guid.Parse("3e2d176b-9f0d-4c1c-b593-2b6a312e8e6f"), Asignatura = "inglés", NombreCalificacion = "cátedra 1", ComentarioCalificacion = "buen trabajo", Calificacion = 6.5, IdEstudiante = Guid.Parse("6b43a3c7-09f3-40f0-96d5-b0d1d36c1c48"), NombreEstudiante = "juan", ApellidoEstudiante = "soto", CorreoEstudiante = "juan.soto@alumnos.ucn.cl" },
                new Grade { Id = Guid.Parse("f5aef793-052c-4c6c-a29d-c4dbe1b0ae4e"), Asignatura = "matemáticas", NombreCalificacion = "cátedra 2", ComentarioCalificacion = "buen trabajo", Calificacion = 5.5, IdEstudiante = Guid.Parse("6b43a3c7-09f3-40f0-96d5-b0d1d36c1c48"), NombreEstudiante = "juan", ApellidoEstudiante = "soto", CorreoEstudiante = "juan.soto@alumnos.ucn.cl" },
                new Grade { Id = Guid.Parse("a4d56aaf-97dc-4c63-908f-38a6e7e177f1"), Asignatura = "inglés", NombreCalificacion = "cátedra 1", ComentarioCalificacion = "decente trabajo", Calificacion = 4.5, IdEstudiante = Guid.Parse("bb2c4f3d-7d34-4148-84b1-19d1f3374bba"), NombreEstudiante = "juana", ApellidoEstudiante = "sánchez", CorreoEstudiante = "juana.sanchez@alumnos.ucn.cl" },
                new Grade { Id = Guid.Parse("b6e0094d-c3af-4711-95d7-937fa1d7ff15"), Asignatura = "matemáticas", NombreCalificacion = "cátedra 2", ComentarioCalificacion = "mal trabajo", Calificacion = 3.5, IdEstudiante = Guid.Parse("bb2c4f3d-7d34-4148-84b1-19d1f3374bba"), NombreEstudiante = "juana", ApellidoEstudiante = "sánchez", CorreoEstudiante = "juana.sanchez@alumnos.ucn.cl" }
            );
            _context.SaveChanges();
        }

        if (!_context.Restrictions.Any())
        {
            _context.Restrictions.AddRange(
                new Restriction { Id = Guid.Parse("7d867534-4d7f-459b-b7a5-f7ef0d8a812c"), Razon = "fuera de proyección", FechaRestriccion = DateTime.Parse("Oct 25, 2023"), IdEstudiante = Guid.Parse("6b43a3c7-09f3-40f0-96d5-b0d1d36c1c48"), NombreEstudiante = "juan", ApellidoEstudiante = "soto", CorreoEstudiante = "juan.soto@alumnos.ucn.cl" },
                new Restriction { Id = Guid.Parse("2d8fcd84-0d8a-4130-bbf5-64a5e0ed8ed5"), Razon = "no está en proyección", FechaRestriccion = DateTime.Parse("Oct 25, 2022"), IdEstudiante = Guid.Parse("6b43a3c7-09f3-40f0-96d5-b0d1d36c1c48"), NombreEstudiante = "juan", ApellidoEstudiante = "soto", CorreoEstudiante = "juan.soto@alumnos.ucn.cl" },
                new Restriction { Id = Guid.Parse("0cde8e58-55a1-4815-9b8d-59a16d7a8966"), Razon = "semestre impago", FechaRestriccion = DateTime.Parse("Oct 25, 2021"), IdEstudiante = Guid.Parse("bb2c4f3d-7d34-4148-84b1-19d1f3374bba"), NombreEstudiante = "juana", ApellidoEstudiante = "sánchez", CorreoEstudiante = "juana.sanchez@alumnos.ucn.cl" },
                new Restriction { Id = Guid.Parse("4f5e158c-2c24-44f8-911f-f07785a8d7a4"), Razon = "estudiante anula semestre", FechaRestriccion = DateTime.Parse("Oct 25, 2020"), IdEstudiante = Guid.Parse("bb2c4f3d-7d34-4148-84b1-19d1f3374bba"), NombreEstudiante = "juana", ApellidoEstudiante = "sánchez", CorreoEstudiante = "juana.sanchez@alumnos.ucn.cl" }
            );
            _context.SaveChanges();
        }
    }

    [HttpGet("grades")]
    //endpoint para tests: revisar todas las calificaciones
    public IActionResult GetGrades()
    {
        var data = _context.Grades.ToList();
        return Ok(new { data });
    }

    [HttpGet("restrictions")]
    //endpoint para tests: revisar todas las restricciones
    public IActionResult GetRestrictions()
    {
        var data = _context.Restrictions.ToList();
        return Ok(new { data });
    }

    /// <summary>
    /// Busca estudiantes según el identificador o el nombre.
    /// </summary>
    /// <param name="request">Objeto que contiene los parámetros de búsqueda.</param>
    /// <returns>Un listado de estudiantes con sus calificaciones y restricciones asociadas.</returns>
    /// <response code="200">Retorna un listado de estudiantes encontrados.</response>
    /// <response code="400">Retorna errores de validación si el modelo es inválido.</response>
    /// <response code="404">No se encontraron estudiantes con los criterios especificados.</response>
    [HttpGet("search-students")]
    public IActionResult SearchStudent([FromQuery] SearchStudentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var grades = _context.Grades.AsQueryable();
        var restrictions = _context.Restrictions.AsQueryable();

        // Filtrar por ID del estudiante
        if (!string.IsNullOrEmpty(request.IdEstudiante))
        {
            grades = grades.Where(g => g.IdEstudiante.ToString().Contains(request.IdEstudiante));
            restrictions = restrictions.Where(r => r.IdEstudiante.ToString().Contains(request.IdEstudiante));
        }

        // Filtrar por nombre del estudiante
        if (!string.IsNullOrEmpty(request.NombreEstudiante))
        {
            grades = grades.Where(g => g.NombreEstudiante.Contains(request.NombreEstudiante));
            restrictions = restrictions.Where(r => r.NombreEstudiante.Contains(request.NombreEstudiante));
        }

        // Obtener los datos sin agrupación de MongoDB
        var gradesList = grades.ToList();
        var restrictionsList = restrictions.ToList();

        // Agrupar en memoria por IdEstudiante
        var groupedResult = gradesList.GroupBy(g => new
        {
            g.IdEstudiante,
            g.NombreEstudiante,
            g.ApellidoEstudiante,
            g.CorreoEstudiante
        })
        .Select(g => new
        {
            IdEstudiante = g.Key.IdEstudiante,
            NombreCompleto = $"{g.Key.NombreEstudiante} {g.Key.ApellidoEstudiante}",
            CorreoEstudiante = g.Key.CorreoEstudiante,

            // Listado de calificaciones por estudiante
            Calificaciones = g.Select(c => new
            {
                IdCalificacion = c.Id,
                Asignatura = c.Asignatura,
                NombreCalificacion = c.NombreCalificacion,
                Calificacion = c.Calificacion,
                Comentario = c.ComentarioCalificacion
            }).ToList(),

            // Listado de restricciones por estudiante (filtradas)
            Restricciones = restrictionsList
                .Where(r => r.IdEstudiante == g.Key.IdEstudiante)
                .Select(r => new
                {
                    IdRestriccion = r.Id,
                    Razon = r.Razon,
                    FechaRestriccion = r.FechaRestriccion.ToString("dd/MM/yyyy")
                }).ToList()
        }).ToList();

        if (!groupedResult.Any())
            return NotFound("No se encontraron estudiantes con los criterios especificados.");

        return Ok(groupedResult);

    }

    /// <summary>
    /// Busca restricciones según el identificador o la razón de la restricción.
    /// </summary>
    /// <param name="request">Objeto que contiene los parámetros de búsqueda.</param>
    /// <returns>Un listado de restricciones agrupadas por estudiante.</returns>
    /// <response code="200">Retorna un listado de restricciones encontradas.</response>
    /// <response code="400">Retorna errores de validación si el modelo es inválido.</response>
    /// <response code="404">No se encontraron restricciones con los criterios especificados.</response>
    [HttpGet("search-restrictions")]
    public IActionResult SearchRestrictions([FromQuery] SearchRestrictionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var restrictions = _context.Restrictions.AsQueryable();

        // Filtrar por ID de la restricción usando Contains
        if (!string.IsNullOrEmpty(request.IdRestriccion))
        {
            restrictions = restrictions.Where(r => r.Id.ToString().Contains(request.IdRestriccion));
        }

        // Filtrar por razón de la restricción usando Contains
        if (!string.IsNullOrEmpty(request.RazonRestriccion))
        {
            restrictions = restrictions.Where(r => r.Razon.Contains(request.RazonRestriccion));
        }

        // Obtener los datos sin agrupación de MongoDB
        var restrictionsList = restrictions.ToList();

        // Agrupar por estudiante
        var groupedResult = restrictionsList.GroupBy(r => new
        {
            r.IdEstudiante,
            r.NombreEstudiante,
            r.ApellidoEstudiante,
            r.CorreoEstudiante
        })
            .Select(g => new
            {
                IdEstudiante = g.Key.IdEstudiante,
                NombreEstudiante = g.Key.NombreEstudiante,
                ApellidoEstudiante = g.Key.ApellidoEstudiante,
                CorreoEstudiante = g.Key.CorreoEstudiante,
                Restricciones = g.Select(r => new
                {
                    IdRestriccion = r.Id,
                    RazonRestriccion = r.Razon,
                    FechaRestriccion = r.FechaRestriccion.ToString("dd/MM/yyyy")
                }).ToList()
            }).ToList();

        if (!groupedResult.Any())
            return NotFound("No se encontraron restricciones con los criterios especificados.");

        return Ok(groupedResult);
    }

    /// <summary>
    /// Busca estudiantes según un rango de calificaciones.
    /// </summary>
    /// <param name="request">Objeto que contiene los parámetros de búsqueda.</param>
    /// <returns>Un listado de estudiantes con sus calificaciones asociadas.</returns>
    /// <response code="200">Retorna un listado de estudiantes encontrados.</response>
    /// <response code="400">Retorna errores de validación si el modelo es inválido.</response>
    /// <response code="404">No se encontraron estudiantes con las calificaciones especificadas.</response>
    [HttpGet("search-grades")]
    public IActionResult SearchGrades([FromQuery] SearchGradeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Validar que minGrade no sea mayor que maxGrade
        if (request.minGrade.HasValue && request.maxGrade.HasValue && request.minGrade.Value > request.maxGrade.Value)
        {
            return BadRequest("La calificación mínima debe ser menor que la calificación máxima.");
        }

        var grades = _context.Grades.AsQueryable();

        // Filtrar por mínimo de calificación
        if (request.minGrade.HasValue)
        {
            grades = grades.Where(g => g.Calificacion >= request.minGrade.Value);
        }

        // Filtrar por máximo de calificación
        if (request.maxGrade.HasValue)
        {
            grades = grades.Where(g => g.Calificacion <= request.maxGrade.Value);
        }

        // Obtener los datos sin agrupación
        var gradesList = grades.ToList();

        // Agrupar por estudiante
        var groupedResult = gradesList.GroupBy(g => new
        {
            g.IdEstudiante,
            g.NombreEstudiante,
            g.ApellidoEstudiante,
            g.CorreoEstudiante
        })
        .Select(g => new
        {
            IdEstudiante = g.Key.IdEstudiante,
            NombreEstudiante = g.Key.NombreEstudiante,
            ApellidoEstudiante = g.Key.ApellidoEstudiante,
            CorreoEstudiante = g.Key.CorreoEstudiante,
            Calificaciones = g.Select(g => new
            {
                IdCalificacion = g.Id,
                NombreAsignatura = g.Asignatura,
                NombreCalificacion = g.NombreCalificacion,
                ValorCalificacion = g.Calificacion,
                Comentario = g.ComentarioCalificacion
            }).ToList()
        }).ToList();

        if (!groupedResult.Any())
            return NotFound("No se encontraron estudiantes con las calificaciones especificadas.");

        return Ok(groupedResult);
    }
}
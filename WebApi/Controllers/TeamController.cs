using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Datos;
using WebApi.Modelos;

namespace WebApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class TeamController : Controller
    {
        private readonly ILogger<TeamController> _logger;
        private readonly ApplicationDbContext _db;

        public TeamController(ILogger<TeamController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;

        }

        [HttpGet("Equipos", Name = "Equipos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<UsEq>>> GetPrueba()
        {
            var resultados = await _db.Tablaequipos.ToListAsync();

            foreach (var resultado in resultados)
            {
                if (resultado.Id == null)  // Verificar si el ID es null
                {
                    // Manejar el caso cuando no hay ID, podrías omitirlo o mostrar un error
                    return BadRequest("Uno de los registros no tiene ID.");
                }
            }

            return Ok(resultados);
        }

        [HttpGet("Id", Name = "Id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<UsEq> Getinformacion(int Id)
        {

            if (Id == 0)
            {
                _logger.LogError("Error con " + Id);
                return BadRequest();
            }

            //var prueba = PruebaStore.pruebalist.FirstOrDefault(v => v.Id == id);
            var prueba = _db.Tablaequipos.FirstOrDefault(v => v.Id == Id);

            if (prueba == null)

            {
                return NotFound();
            }
            return Ok(prueba);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UsEq> CrearPrueba([FromBody] UsEq useq)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_db.Tablaequipos.FirstOrDefault(v => v.Equipo.ToLower() == useq.Equipo.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La prueba con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (useq == null)
            {
                return BadRequest();
            }
            if (useq.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            UsEq modelo = new()
            {
                Equipo = useq.Equipo,
                Team = useq.Team,
                Fecha = useq.Fecha,
                semana = useq.semana
            };
            _db.Tablaequipos.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetPrueba", new { id = useq.Id }, useq);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> updateAsync(int id, [FromBody] UsEq useq)
        {
            if (useq == null || id != useq.Id)
            {
                return BadRequest();
            }

            _db.Tablaequipos.Entry(useq).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YourEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
   
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        /*[
        { "op": "replace", "path": "/Team", "value": "Eq1" }
        ]*/
        public async Task<IActionResult> updatePartial(int id, [FromBody] JsonPatchDocument<UsEq> useq)
        {

            if (useq == null)
            {
                return BadRequest();
            }

            var entity = await _db.Tablaequipos.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            // Aplica los cambios del PatchDocument a la entidad
            useq.ApplyTo(entity, ModelState);

            // Verifica si hay errores en el modelo después de aplicar el parche
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YourEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool YourEntityExists(int id)
        {
            return _db.Tablaequipos.Any(e => e.Id == id);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePrueba(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var prueba = _db.Tablaequipos.FirstOrDefault(v => v.Id == id);
            if (prueba == null)
            {
                return NotFound();
            }
            _db.Tablaequipos.Remove(prueba);
            _db.SaveChanges();

            return NoContent();
        }

    }
}

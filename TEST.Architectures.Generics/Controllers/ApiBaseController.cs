using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TEST.Architectures.Generics.Data;
using TEST.Architectures.Generics.Entities;

namespace TEST.Architectures.Generics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController<T> : ControllerBase where T : BaseEntity, new()
    {
        protected ApplicationContext _context;
        protected DbSet<T> DbSet;

        public ApiBaseController(ApplicationContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        //private async Task<IQueryable<T>> GetSelectAsync(long keyword)
        //{
        //    IQueryable<T> query = await Task.Run(() => IncluirAsync()).ConfigureAwait(true);
        //    if (keyword != 0)
        //    {
        //        query = query.Where(s => s.Id.Equals(keyword));
        //    }
        //    return query.OrderBy(u => u.Id);
        //}
        //private async Task<IQueryable<T>> IncluirAsync()
        //{
        //    IQueryable<T> query = DbSet;
        //    List<Expression<Func<T, object>>> foreignKeys = await Task.Run(() => ForeignAtributtesAsync<T>()).ConfigureAwait(true);
        //    foreach (var navProperty in foreignKeys)
        //    {
        //        query = query.Include(navProperty);
        //    }
        //    return query;
        //}

        //private async Task<List<Expression<Func<T, object>>>> ForeignAtributtesAsync<T>()
        //{
        //    var foreignKeys = new List<Expression<Func<T, object>>>();
        //    var props = typeof(T).GetProperties();
        //    var type = typeof(T);
        //    foreach (var prop in props)
        //    {
        //        var loadInclude = false;
        //        var attrs = prop.GetCustomAttributes(true);
        //        foreach (object attr in attrs)
        //        {
        //            var includeAttr = attr as Includes;
        //            if (includeAttr != null)
        //            {
        //                loadInclude = true;
        //                continue;
        //            }
        //            if (loadInclude)
        //            {
        //                var foreignAttr = attr as System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute;
        //                var jsonIgnoreAttr = attr as Newtonsoft.Json.JsonIgnoreAttribute;
        //                if (foreignAttr != null || jsonIgnoreAttr != null)
        //                {
        //                    var arg = Expression.Parameter(type, "x");
        //                    var propertyInfo = type.GetProperty(prop.Name);
        //                    Expression expr = Expression.Property(arg, propertyInfo);
        //                    if (propertyInfo.PropertyType.IsValueType)
        //                        expr = Expression.Convert(expr, typeof(object));
        //                    var expression = Expression.Lambda<Func<T, object>>(expr, arg);
        //                    foreignKeys.Add(expression);
        //                }
        //            }
        //        }
        //    }
        //    return foreignKeys;
        //}



        /// <summary>
        /// Gets the properties of the model.
        /// </summary>
        /// <returns></returns>
        [HttpGet("map")]
        public IActionResult GetProperties()
        {
            var MapLabels = new List<DTOMapping>();
            foreach (var item in MetadataProvider.GetMetadataForType(typeof(T)).Properties)
            {
                MapLabels.Add(new DTOMapping()
                {
                    ModelName = item.Name,
                    DisplayName = item.DisplayName,
                    Description = item.Description,
                    DataType = item.UnderlyingOrModelType.Name,
                    ElementType = item.ElementMetadata?.UnderlyingOrModelType.Name,
                    IsNullable = item.IsNullableValueType || Equals(item.ElementMetadata?.IsNullableValueType,true)
                });
            }
            return Ok(MapLabels);
        }

        /// <summary>
        /// Gets the count of the items stored.
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")]
        public IActionResult GetCount()
        {
            try
            {
                return Ok(DbSet.Count());
            }
            catch (Exception)
            {
                return Ok(0);
            }
        }

        /// <summary>
        /// Gets all the items stored.
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public ActionResult<IEnumerable<T>> GetAll()
        {
            return DbSet;
        }

        /// <summary>
        /// Gets a specific item by id.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id)
        {
            var result = await Task.Run(() => DbSet.FirstOrDefaultAsync(m => m.Id.Equals(id))).ConfigureAwait(true);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Modifies an item.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <param name="value">Data of the item.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(long id, [FromBody] T value)
        {
            if (id.Equals(value.Id) && value.Id.ToString() != "0")
            {
                ModelState.AddModelError("errors", "Se han encontrado errores en la petición. Refresque la página y vuelva a intentar.");
                return BadRequest();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(value).State = EntityState.Modified;
                    await Task.Run(() => _context.SaveChangesAsync()).ConfigureAwait(true);
                    return Ok("Se ha guardado el registro exitosamente.");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("errors", "No se pueden guardar los cambios.");
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Creates an item.
        /// </summary>
        /// <param name="registro">Data of the item.</param>
        /// <returns></returns>
        [HttpPost("create")]
        public virtual async Task<IActionResult> Post([FromBody] T registro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbSet.Add(registro);
                    await Task.Run(() => _context.SaveChangesAsync()).ConfigureAwait(true);
                    return Ok("Se han guardado los cambios.");
                }
            }
            catch (DbUpdateException exception)
            {
                var innerMessage = (exception.InnerException != null)
                    ? exception.InnerException.Message
                    : exception.Message;
                if (exception.InnerException is Microsoft.Data.SqlClient.SqlException sqlException)
                {
                    if (sqlException.Number == 2601 || sqlException.Number == 2627)
                    {
                        ModelState.AddModelError("errors", "El código ingresado ya se encuentra registrado");
                    }
                    else
                    {
                        ModelState.AddModelError("errors", $"Ocurrió un error de validación de base de datos. Detalles del error: {innerMessage}");
                    }
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("errors", $"No fue posible crear el registro. Por favor intente nuevamente. Detalles del error: {exception}");
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="id">Id of the item.</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public virtual async Task<IActionResult> Delete(long id)
        {
            var result = await Task.Run(() => DbSet.FindAsync(id)).ConfigureAwait(true);
            if (result.Result == null)
            {
                return NotFound("Not found");
            }
            DbSet.Remove(result.Result);
            await Task.Run(() => _context.SaveChangesAsync()).ConfigureAwait(true);
            return Ok("registro eliminado");
        }
    }
}

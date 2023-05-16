using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEST.Architectures.Repositories.Data;
using TEST.Architectures.Repositories.Entities;
using TEST.Architectures.Repositories.Interfaces;

namespace TEST.Architectures.Repositories.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DbModelsController : ControllerBase
    {
        private readonly IDbModelRepository _dbModelRepository;

        public DbModelsController(IDbModelRepository dbModelRepository)
        {
            _dbModelRepository = dbModelRepository;
        }

        /// <summary>
        /// Gets all elements.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDbModel()
        {
            try
            {
                return Ok(await _dbModelRepository.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Finds element by Id.
        /// </summary>
        /// <param name="id">Id of the element.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDbModel(int id)
        {
            try
            {
                var dbModel = await _dbModelRepository.Find(id);
                if (dbModel == null)
                {
                    return NotFound();
                }
                return Ok(dbModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Updates an element.
        /// </summary>
        /// <param name="id">Id of the element.</param>
        /// <param name="dbModel">Data of the element.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDbModel(long id, DbModel dbModel)
        {
            try
            {
                if (id != dbModel.Id)
                    return BadRequest();
                await _dbModelRepository.Update(id, dbModel);
                await _dbModelRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Creates an element.
        /// </summary>
        /// <param name="dbModel">Data of the element.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<DbModel>> PostDbModel(DbModel dbModel)
        {
            try
            {
                await _dbModelRepository.Add(dbModel);
                await _dbModelRepository.SaveChangesAsync();
                return Ok(dbModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Delets an element.
        /// </summary>
        /// <param name="id">Id of the element.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDbModel(int id)
        {
            try
            {
                await _dbModelRepository.Delete(id);
                await _dbModelRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}

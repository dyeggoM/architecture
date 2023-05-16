using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TEST.Architectures.UnitOfWork.Entities;
using TEST.Architectures.UnitOfWork.Interfaces;

namespace TEST.Architectures.UnitOfWork.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DbModelsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DbModelsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                var result = await _unitOfWork.dbModelRepository.GetAll();
                return Ok(result);
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
                var dbModel = await _unitOfWork.dbModelRepository.Find(id);
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
                    return BadRequest("");
                await _unitOfWork.dbModelRepository.Update(id, dbModel);
                await _unitOfWork.SaveChangesAsync();
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
        public async Task<IActionResult> PostDbModel(DbModel dbModel)
        {
            try
            {
                await _unitOfWork.dbModelRepository.Add(dbModel);
                await _unitOfWork.SaveChangesAsync();
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
                await _unitOfWork.dbModelRepository.Delete(id);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}

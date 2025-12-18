using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Контроллер описывает конечные точки для работы с ресурсом
/// "Renter" (арендатор)
/// </summary>
[ApiController]
[Route("renters")]
public sealed class RentersController(IRenterService renterService) : ControllerBase
{
    /// <summary>
    /// Получить всех арендаторов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RenterDto>>> GetAll()
    {
        var renters = await renterService.GetAll();
        return Ok(renters);
    }

    /// <summary>
    /// Получить арендатора по идентификатору
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RenterDto>> GetById(int id)
    {
        var renter = await renterService.GetById(id);
        if (renter is null)
        {
            return NotFound();
        }
        return Ok(renter);
    }

    /// <summary>
    /// Создать нового арендатора
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
    public async Task<ActionResult<RenterDto>> Create([FromBody] RenterCreateUpdateDto dto)
    {
        var created = await renterService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить существующего арендатора
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RenterDto>> Update(int id, [FromBody] RenterCreateUpdateDto dto)
    {
        var updated = await renterService.Update(id, dto);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    /// <summary>
    /// Удалить арендатора по идентификатору
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await renterService.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
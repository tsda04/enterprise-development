using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Контроллер описывает конечные точки для работы
/// с ресурсом "Bike" (велосипед)
/// </summary>
[ApiController]
[Route("bikes")]
public sealed class BikesController(IBikeService bikeService) : ControllerBase
{
    /// <summary>
    /// Получить все ресурсы Bike
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BikeDto>>> GetAll()
    {
        var bikes = await bikeService.GetAll();
        return Ok(bikes);
    }

    /// <summary>
    /// Получить ресурс по идентификатору Bike
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BikeDto>> GetById(int id)
    {
        var bike = await bikeService.GetById(id);
        if (bike is null)
        {
            return NotFound();
        }
        return Ok(bike);
    }

    /// <summary>
    /// Создать новый ресурс Bike
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BikeDto>> Create([FromBody] BikeCreateUpdateDto dto)
    {
        var created = await bikeService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить существующий ресурс Bike
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BikeDto>> Update(int id, [FromBody] BikeCreateUpdateDto dto)
    {
        var updated = await bikeService.Update(id, dto);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    /// <summary>
    /// Удалить ресурс Bike
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await bikeService.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
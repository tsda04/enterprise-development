using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Контроллер описывает конечные точки для работы с ресурсом
/// "Lease" (договор на аренду велосипеда)
/// </summary>
[ApiController]
[Route("leases")]
public sealed class LeasesController(ILeaseService leaseService) : ControllerBase
{
    /// <summary>
    /// Получить все договора на аренду велосипедов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeaseDto>>> GetAll()
    {
        var leases = await leaseService.GetAll();
        return Ok(leases);
    }

    /// <summary>
    /// Получить договор на аренду велосипеда по идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LeaseDto>> GetById(int id)
    {
        var lease = await leaseService.GetById(id);
        if (lease is null)
        {
            return NotFound();
        }
        return Ok(lease);
    }

    /// <summary>
    /// Создать договор на аренду велосипеда
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<LeaseDto>> Create([FromBody] LeaseCreateUpdateDto dto)
    {
        var created = await leaseService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить состояние текущего договора на аренду велосипеда
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<LeaseDto>> Update(int id, [FromBody] LeaseCreateUpdateDto dto)
    {
        var updated = await leaseService.Update(id, dto);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    /// <summary>
    /// Удалить договор на аренду велосипеда по идентификатору
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await leaseService.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}
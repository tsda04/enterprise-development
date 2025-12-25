using BikeRental.Application.Contracts.Dtos;
using BikeRental.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Контроллер описывает конечные точки для работы с
/// ресурсом "BikeModel" (модель велосипеда)
/// "bikeModelService" - сервис для работы с ресурсом BikeModel
/// Зависимость от интерфейса, а не конкретной реализации в сервисе (SOLID - DIP)
/// </summary>
[ApiController]
[Route("bike-models")]
public sealed class BikeModelsController(IBikeModelService bikeModelService) : ControllerBase
{
    /// <summary>
    /// Получить все модели велосипедов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BikeModelDto>>> GetAll()
    {
        var models = await bikeModelService.GetAll();
        var sortedModels = models.OrderBy(model => model.Id).ToList();
        return Ok(sortedModels);
    }

    /// <summary>
    /// Получить модель велосипеда по идентификатору
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BikeModelDto>> GetById(int id)
    {
        var model = await bikeModelService.GetById(id);
        if (model is null)
        {
            return NotFound();
        }

        return Ok(model);
    }

    /// <summary>
    /// Создать новую модель велосипеда
    /// "dto" - модель для создания модели велосипеда
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BikeModelDto>> Create([FromBody] BikeModelCreateUpdateDto dto)
    {
        var created = await bikeModelService.Create(dto);
        
         return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить существующую модель велосипеда
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BikeModelDto>> Update(int id, [FromBody] BikeModelCreateUpdateDto dto)
    {
        var updated = await bikeModelService.Update(id, dto);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    /// <summary>
    /// Удалить модель велосипеда по идентификатору
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await bikeModelService.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent(); 
    }
}
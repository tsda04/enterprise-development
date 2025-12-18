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
        // Обратиться к репозиторию для получения всех моделей велосипедов
        var models = await bikeModelService.GetAll();
        return Ok(models);
    }

    /// <summary>
    /// Получить модель велосипеда по идентификатору
    /// </summary>
    [HttpGet("{id:int}")] // ограничение - ID должен быть числом
    public async Task<ActionResult<BikeModelDto>> GetById(int id)
    {
        // Обратиться к репозиторию для получения модели велосипеда по идентификатору
        var model = await bikeModelService.GetById(id);
        if (model is null)
        {
            // вернуть код ответа 404 Not Found (не найдено)
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
        // Обратиться к репозиторию для создания новой модели велосипеда
        // с использованием данных из dto
        var created = await bikeModelService.Create(dto);
        
        // Вернуть успешный результат обработки операции
        // с кодом ответа 201 Created (создано) и созданной моделью велосипеда
        
        // Дополнительно вернуть в заголовке Location ссылку на созданный ресурс
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Обновить существующую модель велосипеда
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BikeModelDto>> Update(int id, [FromBody] BikeModelCreateUpdateDto dto)
    {
        // Обратиться к репозиторию для обновления модели велосипеда по идентификатору
        // с использованием данных из dto
        var updated = await bikeModelService.Update(id, dto);
        if (updated is null)
        {
            // Если не найдена 
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
        // Обратиться к репозиторию для удаления модели велосипеда по идентификатору
        var deleted = await bikeModelService.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent(); 
    }
}
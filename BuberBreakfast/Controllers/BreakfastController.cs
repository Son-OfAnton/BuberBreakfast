using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using Microsoft.AspNetCore.Mvc;
using BuberBreakfast.Services.Breakfasts;

namespace BuberBreakfast.Controllers;


[ApiController]
[Route("breakfasts")]
public class BreakfastController : ControllerBase
{
  private readonly IBreakfastService _breakfastService;

  public BreakfastController(IBreakfastService breakfastService)
  {
    _breakfastService = breakfastService;
  }

  [HttpPost()]
  public IActionResult CreateBreakfast(CreateBreakfastRequest request)
  {
    // Mapping a request to a model
    var Breakfast = new Breakfast(
      Guid.NewGuid(),
      request.Name,
      request.Description,
      request.StartDateTime,
      request.EndDateTime,
      DateTime.UtcNow,
      request.Savory,
      request.Sweet
    );

    // TODO: Save breakfast to database
    _breakfastService.CreateBreakfast(Breakfast);

    var response = new BreakfastResponse(
      Breakfast.Id,
      Breakfast.Name,
      Breakfast.Description,
      Breakfast.StartDateTime,
      Breakfast.EndDateTime,
      Breakfast.LastModifiedDateTime,
      Breakfast.Savory,
      Breakfast.Sweet
    );

    return CreatedAtAction(
      nameof(GetBreakfast), 
      new { id = Breakfast.Id },
      response);
  }

  [HttpGet("{id:Guid}")]
  public IActionResult GetBreakfast(Guid id)
  {
    Breakfast breakfast = _breakfastService.GetBreakfast(id);
    
    var response = new BreakfastResponse(
      breakfast.Id,
      breakfast.Name,
      breakfast.Description,
      breakfast.StartDateTime,
      breakfast.EndDateTime,
      breakfast.LastModifiedDateTime,
      breakfast.Savory,
      breakfast.Sweet
    );
    return Ok(response);
  }

  [HttpGet]
  public IActionResult GetAllBreakfasts()
  {
    Breakfast[] breakfasts = _breakfastService.GetAllBreakfasts();
    var response = new List<BreakfastResponse>();
    foreach (Breakfast breakfast in breakfasts)
    {
      response.Add(new BreakfastResponse(
        breakfast.Id,
        breakfast.Name,
        breakfast.Description,
        breakfast.StartDateTime,
        breakfast.EndDateTime,
        breakfast.LastModifiedDateTime,
        breakfast.Savory,
        breakfast.Sweet
      ));
    }
    return Ok(response);
  }

  [HttpPut("{id:Guid}")]
  public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
  {
    var breakfast = new Breakfast(
      id,
      request.Name,
      request.Description,
      request.StartDateTime,
      request.EndDateTime,
      DateTime.UtcNow,
      request.Savory,
      request.Sweet
    );

    _breakfastService.UpsertBreakfast(breakfast);

    // TODO: Return 201 if a new breakfast is created

    return NoContent();
  }

  [HttpDelete("{id:Guid}")]
  public IActionResult DeleteBreakfast(Guid id)
  {
    _breakfastService.DeleteBreakfast(id);
        
    return NoContent();
  }
}
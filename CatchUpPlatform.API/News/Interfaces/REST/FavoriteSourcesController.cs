using System.Net.Mime;
using CatchUpPlatform.API.News.Domain.Model.Queries;
using CatchUpPlatform.API.News.Domain.Services;
using CatchUpPlatform.API.News.Interfaces.REST.Resources;
using CatchUpPlatform.API.News.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CatchUpPlatform.API.News.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Favorite Sources")]
public class FavoriteSourcesController(
    IFavoriteSourceCommandService favoriteSourceCommandService,
    IFavoriteSourceQueryService favoriteSourceQueryService
    ) : ControllerBase
{
    /// <summary>
    /// Get Favorite Source by ID 
    /// </summary>
    /// <param name="id">The Favorite Source ID provided by this API</param>
    /// <returns>
    /// A FavoriteSourceResource object
    /// </returns>
    [SwaggerOperation(
        Summary = "Get Favorite Source by ID",
        Description = "Get Favorite Source Resource by given ID",
        OperationId = "GetFavoriteSourceById")]
    [SwaggerResponse(200, "The favorite source was found", typeof(FavoriteSourceResource))]
    [SwaggerResponse(404, "The favorite source was not found")]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetFavoriteSourceById(int id)
    {
        var getFavoriteSourceByIdQuery = new GetFavoriteSourceByIdQuery(id);
        var result = await favoriteSourceQueryService.Handle(getFavoriteSourceByIdQuery);
        if (result is null) return NotFound();
        var resource = FavoriteSourceResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
}
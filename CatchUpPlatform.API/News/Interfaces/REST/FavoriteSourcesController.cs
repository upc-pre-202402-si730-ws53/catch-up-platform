using System.Net.Mime;
using CatchUpPlatform.API.News.Domain.Model.Queries;
using CatchUpPlatform.API.News.Domain.Services;
using CatchUpPlatform.API.News.Interfaces.REST.Resources;
using CatchUpPlatform.API.News.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CatchUpPlatform.API.News.Interfaces.REST;

/// <summary>
/// REST API Controller for Favorite Sources 
/// </summary>
/// <param name="favoriteSourceCommandService">The Favorite Source Command Service</param>
/// <param name="favoriteSourceQueryService">The Favorite Source Query Service</param>
/// <see cref="IFavoriteSourceCommandService"/>
/// <see cref="IFavoriteSourceQueryService"/> 
/// since 1.0.0
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

    /// <summary>
    /// Create a favorite source 
    /// </summary>
    /// <param name="resource">The <see cref="CreateFavoriteSourceResource"/> resource</param>
    /// <returns>
    /// A <see cref="FavoriteSourceResource"/> object containing the created favorite source including the ID
    /// </returns>
    [SwaggerOperation(
        Summary = "Create a favorite source",
        Description = "Create a favorite source with the given News API Key and Source ID",
        OperationId = "CreateFavoriteSource")]
    [SwaggerResponse(201, "The favorite source was created", typeof(FavoriteSourceResource))]
    [SwaggerResponse(400, "The favorite source was not created")]
    [HttpPost]
    public async Task<ActionResult> CreateFavoriteSource([FromBody] CreateFavoriteSourceResource resource)
    {
        var createFavoriteSourceCommand = CreateFavoriteSourceCommandFromResourceAssembler
            .ToCommandFromResource(resource);
        var result = await favoriteSourceCommandService.Handle(createFavoriteSourceCommand);
        if (result is null) return BadRequest();
        return CreatedAtAction(nameof(GetFavoriteSourceById), new { id = result.Id },
            FavoriteSourceResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    /// <summary>
    /// Get all favorite sources by News API Key 
    /// </summary>
    /// <param name="newsApiKey">A string containing the News API Key from the news provider</param>
    /// <returns>
    /// A list of <see cref="FavoriteSourceResource"/> objects for the given News API Key
    /// </returns>
    private async Task<ActionResult> GetAllFavoriteSourcesByNewsApiKey(string newsApiKey)
    {
        var getAllFavoriteSourcesByNewsApiKeyQuery = new GetAllFavoriteSourcesByNewsApiKeyQuery(newsApiKey);
        var result = await favoriteSourceQueryService.Handle(getAllFavoriteSourcesByNewsApiKeyQuery);
        var resources = 
            result.Select(FavoriteSourceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Get Favorite Source by News API Key and Source ID 
    /// </summary>
    /// <param name="newsApiKey">A string containing the News API Key from the news provider</param>
    /// <param name="sourceId">The Source ID from the news provider</param>
    /// <returns>
    /// A <see cref="FavoriteSourceResource"/> object for the given News API Key and Source ID if found, otherwise NotFound
    /// </returns>
    private async Task<ActionResult> GetFavoriteSourceByNewsApiKeyAndSourceId(string newsApiKey, string sourceId)
    {
        var getFavoriteSourceByNewsApiKeyAndSourceIdQuery =
            new GetFavoriteSourceByNewsApiKeyAndSourceIdQuery(newsApiKey, sourceId);
        var result = await favoriteSourceQueryService.Handle(getFavoriteSourceByNewsApiKeyAndSourceIdQuery);
        if (result is null) return NotFound();
        var resource = FavoriteSourceResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    /// Get Favorite Source(s) according to the query parameters 
    /// </summary>
    /// <param name="newsApiKey">The News API Key generated by the news service provider</param>
    /// <param name="sourceId">The Source ID from the news service provider</param>
    /// <returns>
    /// A response as an ActionResult containing the favorite source if News API Key and Source ID are provided,
    /// otherwise all favorite sources for the given News API Key
    /// </returns>
    [SwaggerOperation(
        Summary = "Get Favorite Source(s) according to the query parameters",
        Description = "Get Favorite Source(s) according to the query parameters",
        OperationId = "GetFavoriteSourceFromQuery")]
    [SwaggerResponse(200, "The favorite source(s) were found", typeof(FavoriteSourceResource))] 
    [HttpGet]
    public async Task<ActionResult> GetFavoriteSourceFromQuery(
        [FromQuery] string newsApiKey,
        [FromQuery] string sourceId)
    {
        return string.IsNullOrEmpty(sourceId)
            ? await GetAllFavoriteSourcesByNewsApiKey(newsApiKey)
            : await GetFavoriteSourceByNewsApiKeyAndSourceId(newsApiKey, sourceId);
    }
}
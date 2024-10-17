using CatchUpPlatform.API.News.Domain.Model.Commands;
using CatchUpPlatform.API.News.Interfaces.REST.Resources;

namespace CatchUpPlatform.API.News.Interfaces.REST.Transform;

/// <summary>
/// Assembler to transform a CreateFavoriteSourceResource to a CreateFavoriteSourceCommand 
/// </summary>
public static class CreateFavoriteSourceCommandFromResourceAssembler
{
    /// <summary>
    /// Transform a CreateFavoriteSourceResource to a CreateFavoriteSourceCommand 
    /// </summary>
    /// <param name="resource">The <see cref="CreateFavoriteSourceResource"/> resource</param>
    /// <returns>An instance of <see cref="CreateFavoriteSourceCommand"/></returns>
    public static CreateFavoriteSourceCommand ToCommandFromResource(CreateFavoriteSourceResource resource)
    {
        return new CreateFavoriteSourceCommand(resource.NewsApiKey, resource.SourceId);
    }
}
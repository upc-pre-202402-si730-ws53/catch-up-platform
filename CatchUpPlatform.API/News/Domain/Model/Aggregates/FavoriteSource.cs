using CatchUpPlatform.API.News.Domain.Model.Commands;

namespace CatchUpPlatform.API.News.Domain.Model.Aggregates;

/// FavoriteSource Aggregate
/// <summary>
/// This class represents the FavoriteSource aggregate. It is used to store the favorite sources of a user. 
/// </summary>
/// 
public class FavoriteSource
{
    public int Id { get; private set; }
    public string NewsApiKey { get; private set; }
    public string SourceId { get; private set; }

    protected FavoriteSource()
    {
        this.NewsApiKey = string.Empty;
        this.SourceId = string.Empty;
    }

    /// <summary>
    /// Constructor for the FavoriteSource aggregate
    /// </summary>
    /// <remarks>
    /// The constructor is the command handler for the CreateFavoriteSourceCommand. It initializes the FavoriteSource aggregate with the NewsApiKey and SourceId.
    /// </remarks>
    /// <param name="command">The CreateFavoriteSourceCommand command</param>
    public FavoriteSource(CreateFavoriteSourceCommand command)
    {
        this.NewsApiKey = command.NewsApiKey;
        this.SourceId = command.SourceId;
    }
    
}
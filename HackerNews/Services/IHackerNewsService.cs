using HackerNews.Models;

namespace HackerNews.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int n);
    }
}

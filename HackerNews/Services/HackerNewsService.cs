using HackerNews.Models;
using Microsoft.Extensions.Caching.Hybrid;
using System.Text.Json;

namespace HackerNews.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly HybridCache _hybridCache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);


        public HackerNewsService(HttpClient httpClient, HybridCache hybridCache)
        {
            _httpClient = httpClient;
            _hybridCache = hybridCache;
        }

        public async Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int n)
        {
            var cacheKey = $"BestStories_{n}";

            return await _hybridCache.GetOrCreateAsync(
                cacheKey,
                async _ => await FetchBestStoriesAsync(n),
                new HybridCacheEntryOptions
                {
                    Expiration = CacheDuration
                }
            ) ?? Enumerable.Empty<StoryDto>();
        }

        private async Task<IEnumerable<StoryDto>> FetchBestStoriesAsync(int n)
        {
            var bestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
            var idsResponse = await _httpClient.GetStringAsync(bestStoriesUrl);
            var selectedIds = JsonSerializer.Deserialize<List<int>>(idsResponse);

            var tasks = selectedIds.Take(n).Select(GetStoryAsync);
            var stories = await Task.WhenAll(tasks);

            return stories
                .Where(s => s != null)
                .OrderByDescending(s => s.Score)
                .ToList();
        }

        private async Task<StoryDto> GetStoryAsync(int id)
        {
            var url = $"https://hacker-news.firebaseio.com/v0/item/{id}.json";

            var response = await _httpClient.GetStringAsync(url);
            var item = JsonSerializer.Deserialize<HackerNewsItem>(response);

            if (item == null)
                return null;

            return new StoryDto
            {
                Title = item.title,
                Uri = item.url,
                PostedBy = item.by,
                Time = DateTimeOffset.FromUnixTimeSeconds(item.time).DateTime,
                Score = item.score,
                CommentCount = item.descendants
            };
        }

        private class HackerNewsItem
        {
            public string by { get; set; }
            public int descendants { get; set; }
            public int id { get; set; }
            public int score { get; set; }
            public long time { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public string url { get; set; }
        }
    }
}

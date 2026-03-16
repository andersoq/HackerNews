using HackerNews.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace HackerNews.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int n)
        {
            var cacheKey = $"BestStories_{n}";

            // Try to get cached stories
            if (_cache.TryGetValue(cacheKey, out IEnumerable<StoryDto> cachedStories))
            {
                return cachedStories;
            }

            // If not in cache, fetch from API
            var bestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
            var idsResponse = await _httpClient.GetStringAsync(bestStoriesUrl);
            var ids = JsonSerializer.Deserialize<List<int>>(idsResponse);
            var selectedIds = ids.Take(n);
            var tasks = selectedIds.Select(GetStoryAsync);
            var stories = await Task.WhenAll(tasks);

            // Cache the fetched stories
            _cache.Set(cacheKey, stories, CacheDuration);

            return stories
                .Where(s => s != null)
                .OrderByDescending(s => s.Score);
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

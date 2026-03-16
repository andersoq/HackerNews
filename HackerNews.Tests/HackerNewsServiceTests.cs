using HackerNews.Models;
using System.Text.Json;

namespace HackerNews.Tests
{
    public class StoryDtoMappingTests
    {
        [Fact]
        public void StoryDto_HasCorrectProperties_ForApiResponse()
        {
            // Arrange & Act
            var story = new StoryDto
            {
                Title = "Great Article",
                Uri = "https://example.com/article",
                PostedBy = "john_doe",
                Time = new DateTime(2021, 12, 20),
                Score = 250,
                CommentCount = 42
            };

            // Assert
            Assert.Equal("Great Article", story.Title);
            Assert.Equal("https://example.com/article", story.Uri);
            Assert.Equal("john_doe", story.PostedBy);
            Assert.Equal(250, story.Score);
            Assert.Equal(42, story.CommentCount);
        }

        [Fact]
        public void StoryDto_CanHandle_NullUri()
        {
            // Arrange & Act
            var story = new StoryDto
            {
                Title = "Ask HN: Question",
                Uri = null,
                PostedBy = "asker",
                Time = new DateTime(2021, 12, 20),
                Score = 50,
                CommentCount = 25
            };

            // Assert
            Assert.Null(story.Uri);
            Assert.NotNull(story.Title);
            Assert.Equal("Ask HN: Question", story.Title);
        }

        [Fact]
        public void StoryDto_SerializesCorrectly()
        {
            // Arrange
            var story = new StoryDto
            {
                Title = "Great Article",
                Uri = "https://example.com/article",
                PostedBy = "john_doe",
                Time = new DateTime(2021, 12, 20),
                Score = 250,
                CommentCount = 42
            };

            // Act
            var json = JsonSerializer.Serialize(story);

            // Assert
            Assert.Contains("Great Article", json);
            Assert.Contains("john_doe", json);
            Assert.Contains("250", json);
        }
    }
}

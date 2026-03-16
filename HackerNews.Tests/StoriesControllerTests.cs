using HackerNews.Controllers;
using HackerNews.Models;
using HackerNews.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HackerNews.Tests
{
    public class StoriesControllerTests
    {
        private readonly Mock<IHackerNewsService> _mockService;
        private readonly StoriesController _controller;

        public StoriesControllerTests()
        {
            _mockService = new Mock<IHackerNewsService>();
            _controller = new StoriesController(_mockService.Object);
        }

        [Fact]
        public async Task GetBestStories_WithValidN_ReturnsOkWithStories()
        {
            // Arrange
            var expectedStories = new List<StoryDto>
            {
                new StoryDto { Title = "Story 1", Score = 100 },
                new StoryDto { Title = "Story 2", Score = 90 }
            };
            _mockService.Setup(s => s.GetBestStoriesAsync(2)).ReturnsAsync(expectedStories);

            // Act
            var result = await _controller.GetBestStories(2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var stories = Assert.IsAssignableFrom<IEnumerable<StoryDto>>(okResult.Value);

            Assert.Equal(2, stories.Count());
            Assert.Equal(expectedStories, stories);

            _mockService.Verify(s => s.GetBestStoriesAsync(2), Times.Once);
        }

        [Fact]
        public async Task GetBestStories_WithZero_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetBestStories(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("n must be greater than zero", badRequestResult.Value);

            _mockService.Verify(s => s.GetBestStoriesAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetBestStories_WithNegativeN_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetBestStories(-5);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("n must be greater than zero", badRequestResult.Value);

            _mockService.Verify(s => s.GetBestStoriesAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetBestStories_CallsServiceWithCorrectParameter()
        {
            // Arrange
            var expectedStories = new List<StoryDto>
            {
                new StoryDto { Title = "Test Story 1", Score = 100 },
                new StoryDto { Title = "Test Story 2", Score = 90 },
                new StoryDto { Title = "Test Story 3", Score = 80 }
            };
            _mockService.Setup(s => s.GetBestStoriesAsync(10)).ReturnsAsync(expectedStories);

            // Act
            var result = await _controller.GetBestStories(10);

            // Assert
            _mockService.Verify(s => s.GetBestStoriesAsync(10), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<StoryDto>>(okResult.Value);
        }
    }
}

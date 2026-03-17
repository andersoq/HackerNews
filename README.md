# HackerNews API

RESTful API to retrieve the best stories from Hacker News, sorted by score.

## Quick Start

### Prerequisites
- .NET 8.0 SDK

### Run the Application

```powershell
dotnet restore
dotnet run --project HackerNews
```

### Test the API

Open your browser to the URL shown in the console, then add `/swagger`:
```
http://localhost:XXXX/swagger
```

Or test directly:
```
http://localhost:XXXX/stories/best?n=10
```

### Run Tests
```powershell
dotnet test
```

## API Endpoint

**GET** `/stories/best?n={number}`

Returns the first `n` best stories, sorted by score (descending).

**Response Format:**
```json
[
  {
    "title": "Story title",
    "uri": "https://example.com",
    "postedBy": "username",
    "time": "2019-10-12T13:43:01+00:00",
    "score": 1716,
    "commentCount": 572
  }
]
```

## How It Works

- **Caching**: Results cached for 5 minutes to reduce API load
- **Rate Limiting**: Maximum 5 concurrent requests to avoid overwhelming the API
- **Retry Logic**: Automatic retries failed requests
- **Parallel Fetching**: Story details fetched concurrently for better performance
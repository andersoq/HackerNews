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

- **Caching**: Responses are cached for 5 minutes to reduce load on the Hacker News API while keeping data reasonably fresh.
- **Rate Limiting**: No more than 5 requests run concurrently to avoid overwhelming the API.
- **Parallel Fetching**:Story details are fetched simultaneously for faster performance.
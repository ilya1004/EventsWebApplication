namespace EventsWebApplication.Domain.Abstractions.BlobStorage;

public record FileResponse(Stream Stream, string ContentType);

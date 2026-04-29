using CsvProcessor.Shared.DTOs;

namespace CsvProcessor.Api.Services.Interfaces;

public interface ISqsService
{
    Task PublishAsync(CsvUploadPayload payload, CancellationToken cancellationToken = default);
}
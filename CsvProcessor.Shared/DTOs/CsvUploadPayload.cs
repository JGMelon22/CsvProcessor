namespace CsvProcessor.Shared.DTOs;

public record CsvUploadPayload(
    string S3Key,
    string FileName,
    DateTime UploadedAt,
    int TotalRecords
);
namespace CustomerApi.Models;

// Det API:et skickar ut över HTTP. Vi skiljer DTO från domänobjekt
// så att interna fält (t.ex. RegisteredAt) inte läcker ut oavsiktligt.
public record CustomerDto(string Id, string Name, string Email, bool IsActive);

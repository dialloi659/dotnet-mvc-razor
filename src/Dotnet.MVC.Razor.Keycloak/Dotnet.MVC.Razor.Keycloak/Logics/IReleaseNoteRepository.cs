using Dotnet.MVC.Razor.Keycloak.Data.Entities;

namespace Dotnet.MVC.Razor.Keycloak.Logics;

public interface IReleaseNoteRepository
{
    Task<IEnumerable<ReleaseNote>> GetAllAsync();
    Task<ReleaseNote?> GetLatestAsync();
    Task<bool> HasUserSeenLatestAsync(string userId);
    Task MarkAsSeenAsync(string userId, int releaseNoteId);

    Task<ReleaseNote?> GetByIdAsync(int id);
    Task AddAsync(ReleaseNote note);
    Task UpdateAsync(ReleaseNote note);
    Task DeleteAsync(int id);
}

using Dotnet.MVC.Razor.Keycloak.Data;
using Dotnet.MVC.Razor.Keycloak.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.MVC.Razor.Keycloak.Logics;

public class ReleaseNoteRepository(AppDbContext context) : IReleaseNoteRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<ReleaseNote>> GetAllAsync()
    {
        var releaseNotes = await _context.ReleaseNotes
            .OrderByDescending(r => r.ReleaseDate)
            .ToListAsync();

        return releaseNotes;
    }

    public async Task<ReleaseNote?> GetLatestAsync()
    {
        var releaseNote = await _context.ReleaseNotes
            .OrderByDescending(r => r.ReleaseDate)
            .FirstOrDefaultAsync();

        return releaseNote;
    }

    public async Task<bool> HasUserSeenLatestAsync(string userId)
    {
        var latest = await GetLatestAsync();
        if (latest == null) return true;

        return await _context.UserReleaseViews.AnyAsync(v => v.UserId == userId && v.ReleaseNoteId == latest.Id);
    }

    public async Task MarkAsSeenAsync(string userId, int releaseNoteId)
    {
        if (!await _context.UserReleaseViews.AnyAsync(v => v.UserId == userId && v.ReleaseNoteId == releaseNoteId))
        {
            _context.UserReleaseViews.Add(new UserReleaseView
            {
                UserId = userId,
                ReleaseNoteId = releaseNoteId,
                ViewedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ReleaseNote?> GetByIdAsync(int id) =>
        await _context.ReleaseNotes.FindAsync(id);

    public async Task AddAsync(ReleaseNote note)
    {
        note.ReleaseDate = DateTime.UtcNow;
        _context.ReleaseNotes.Add(note);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReleaseNote note)
    {
        _context.ReleaseNotes.Update(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var note = await _context.ReleaseNotes.FindAsync(id);
        if (note != null)
        {
            _context.ReleaseNotes.Remove(note);
            await _context.SaveChangesAsync();
        }
    }
}

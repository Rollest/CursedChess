using CursedChess.Application.Contracts;
using CursedChess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CursedChess.Infrastructure.Repositories;

/// <summary>
/// Репозиторий решений, использующий EF Core.
/// </summary>
public sealed class SolutionRepository : ISolutionRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly BoardDbContext _context;

    /// <summary>
    /// Создаёт репозиторий решений.
    /// </summary>
    /// <param name="context">Контекст Entity Framework Core.</param>
    public SolutionRepository(BoardDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Асинхронно сохраняет набор решений.
    /// </summary>
    /// <param name="solutions">Коллекция решений для сохранения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Завершённая задача сохранения.</returns>
    public async Task SaveSolutionsAsync(IEnumerable<Solution> solutions, CancellationToken cancellationToken = default)
    {
        var materialized = solutions.ToList();
        if (materialized.Count == 0)
        {
            return;
        }

        var boardId = materialized[0].BoardId;
        var existing = await _context.Solutions
            .Where(s => s.BoardId == boardId)
            .ToListAsync(cancellationToken);
        if (existing.Count > 0)
        {
            _context.Solutions.RemoveRange(existing);
        }

        await _context.Solutions.AddRangeAsync(materialized, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Асинхронно получает решения для указанной доски.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Список решений вместе с их элементами.</returns>
    public async Task<IReadOnlyList<Solution>> GetSolutionsForBoardAsync(int boardId, CancellationToken cancellationToken = default)
    {
        return await _context.Solutions
            .Include(s => s.Items)
            .Where(s => s.BoardId == boardId)
            .ToListAsync(cancellationToken);
    }
}


using CursedChess.Application.Contracts;
using CursedChess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CursedChess.Infrastructure.Repositories;

/// <summary>
/// Репозиторий досок, использующий EF Core.
/// </summary>
public sealed class BoardRepository : IBoardRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly BoardDbContext _context;

    /// <summary>
    /// Создаёт репозиторий для работы с досками.
    /// </summary>
    /// <param name="context">Контекст Entity Framework Core.</param>
    public BoardRepository(BoardDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Загружает доску вместе со связанными сущностями.
    /// </summary>
    /// <param name="id">Идентификатор доски.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Доска или `null`, если она не найдена.</returns>
    public async Task<Board?> GetBoardAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Boards
            .AsNoTracking()
            .Include(b => b.Cells)
            .Include(b => b.Agents)
            .Include(b => b.FixedPositions)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Board?> GetLatestBoardAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Boards
            .AsNoTracking()
            .Include(b => b.Cells)
            .Include(b => b.Agents)
            .Include(b => b.FixedPositions)
                .ThenInclude(fp => fp.Agent!)
            .OrderByDescending(b => b.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Сохраняет доску и выполняет вставку или обновление в базе.
    /// </summary>
    /// <param name="board">Доска для сохранения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Завершённая задача сохранения.</returns>
    public async Task SaveBoardAsync(Board board, CancellationToken cancellationToken = default)
    {
        // In Blazor Server scoped DbContext can live across many UI actions.
        // Clear tracked graph to avoid duplicate-key tracking conflicts on repeated load/save cycles.
        _context.ChangeTracker.Clear();

        foreach (var agent in board.Agents)
        {
            agent.BoardId = board.Id;
        }

        foreach (var fixedPosition in board.FixedPositions)
        {
            fixedPosition.BoardId = board.Id;
        }

        if (board.Id != 0)
        {
            foreach (var cell in board.Cells)
            {
                cell.BoardId = board.Id;
            }
        }

        if (board.Id == 0)
        {
            await _context.Boards.AddAsync(board, cancellationToken);
        }
        else
        {
            _context.Boards.Update(board);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}


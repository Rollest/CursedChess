using CursedChess.Domain.Entities;

namespace CursedChess.Application.Contracts;

/// <summary>
/// Репозиторий для загрузки и сохранения досок.
/// </summary>
public interface IBoardRepository
{
    /// <summary>
    /// Асинхронно получает доску по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор доски.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Задача с доской или `null`, если доска не найдена.</returns>
    Task<Board?> GetBoardAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Асинхронно сохраняет доску и связанные сущности.
    /// </summary>
    /// <param name="board">Доска для сохранения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Задача завершения сохранения.</returns>
    Task SaveBoardAsync(Board board, CancellationToken cancellationToken = default);
}


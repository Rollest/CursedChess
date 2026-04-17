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
    /// Возвращает последнюю сохранённую доску (максимальный Id) со всеми клетками и агентами.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Доска или <c>null</c>, если таблица пуста.</returns>
    Task<Board?> GetLatestBoardAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Асинхронно сохраняет доску и связанные сущности.
    /// </summary>
    /// <param name="board">Доска для сохранения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Задача завершения сохранения.</returns>
    Task SaveBoardAsync(Board board, CancellationToken cancellationToken = default);
}


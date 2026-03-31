namespace CursedChess.Domain.Entities;

/// <summary>
/// Координаты позиции на доске.
/// </summary>
/// <param name="Row">Номер строки.</param>
/// <param name="Column">Номер колонки.</param>
public readonly record struct Position(int Row, int Column);


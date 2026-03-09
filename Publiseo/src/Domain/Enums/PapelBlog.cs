namespace Domain.Enums;

/// <summary>
/// Papel do usuário no blog (owner = dono; editor/viewer para vínculos futuros).
/// </summary>
public enum PapelBlog
{
    Owner = 0,
    Editor = 1,
    Viewer = 2
}

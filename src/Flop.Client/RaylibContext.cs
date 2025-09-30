using Raylib_cs;

namespace Flop.Client;

public readonly struct DrawingContext : IDisposable
{
    public DrawingContext()
    {
        Raylib.BeginDrawing();
    }

    public void Dispose()
    {
        Raylib.EndDrawing();
    }
}

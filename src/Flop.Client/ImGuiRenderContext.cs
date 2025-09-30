using rlImGui_cs;

namespace Flop.Client;

public readonly struct ImGuiRenderContext : IDisposable
{
    public ImGuiRenderContext()
    {
        rlImGui.Begin();
    }

    public void Dispose()
    {
        rlImGui.End();
    }
}

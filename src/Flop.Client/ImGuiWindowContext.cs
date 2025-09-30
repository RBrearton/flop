using ImGuiNET;

namespace Flop.Client;

public readonly struct ImGuiWindowContext : IDisposable
{
    public ImGuiWindowContext(string name)
    {
        ImGui.Begin(name);
    }

    public void Dispose()
    {
        ImGui.End();
    }
}

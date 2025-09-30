using System.Numerics;
using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

Raylib.InitWindow(800, 600, "Flop");
Raylib.SetTargetFPS(60);

rlImGui.Setup(true); // true = dark theme

bool showDemoWindow = true;
Vector3 color = new(0.2f, 0.4f, 0.8f);
int counter = 0;

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);

    // Raylib drawing
    Raylib.DrawText("Hello from Raylib!", 12, 12, 20, Color.White);
    Raylib.DrawCircle(
        700,
        100,
        30,
        new Color((int)(color.X * 255), (int)(color.Y * 255), (int)(color.Z * 255), 255)
    );

    // ImGui UI
    rlImGui.Begin();

    ImGui.Begin("Demo Window");
    ImGui.Text("Simple ImGui demo!");
    ImGui.Separator();

    if (ImGui.Button("Click me!"))
        counter++;
    ImGui.SameLine();
    ImGui.Text($"Counter: {counter}");

    ImGui.ColorEdit3("Circle Color", ref color);

    ImGui.Checkbox("Show ImGui Demo", ref showDemoWindow);

    ImGui.End();

    if (showDemoWindow)
        ImGui.ShowDemoWindow(ref showDemoWindow);

    rlImGui.End();

    Raylib.EndDrawing();
}

rlImGui.Shutdown();
Raylib.CloseWindow();

using Raylib_cs;

Raylib.InitWindow(800, 600, "Flop");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Hello from Raylib!", 12, 12, 20, Color.White);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();

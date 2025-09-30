# Flop

A game built with C# and raylib, following the "spartan programming" philosophy.

## Structure

- `src/Flop.Core` - Pure game logic class library (renderer-agnostic)
- `src/Flop.Client` - Graphical client using raylib

## Dependencies

- [Raylib-cs](https://github.com/ChrisDill/Raylib-cs) - Thin C-like bindings for raylib
- [rlImgui-cs](https://github.com/raylib-extras/rlImGui-cs) - Dear ImGui integration for raylib
- [ImGui.NET](https://github.com/ImGuiNET/ImGui.NET) - C# bindings for Dear ImGui

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run --project src/Flop.Client
```

## Notes

- `imgui.ini` will be generated automatically by Dear ImGui to save window layouts and is excluded from git

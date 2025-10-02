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

## Renderer Architecture

Flop uses a modular, GPU-efficient rendering system built around instanced rendering. The architecture separates geometry definition (in Core) from GPU resource management and rendering (in Client).

### Core Concepts

#### 1. Geometry Hierarchy (Flop.Core)

The geometry system uses a three-level hierarchy:

- **IGeometryPrimitive** - Atomic shapes (Box, Cylinder, Sphere, Hemisphere)
  - Contains geometry parameters (size, radius, etc.)
  - Contains a `Material` (color, eventually textures/shaders)
  - Has `GetMesh(IMeshGenerator)` to generate CPU-side mesh data
  - Has local position and rotation for composition

- **IGeometryComponent** - Rigid bodies composed of multiple primitives
  - Example: Capsule (1 cylinder + 2 hemispheres)
  - Primitives are positioned relative to the component

- **IGeometryRig** - Complete actors with multiple components
  - Example: Character with separate body parts
  - Components are positioned relative to the rig

#### 2. Material System (Flop.Core)

**Material** (`Flop.Core.Material`)
- Value type containing rendering properties
- Currently: `Color` (RGBA bytes)
- Future: Textures, shaders, etc.
- Lives in Core - no raylib dependency

**MaterialHandle** (`Flop.Core.Geometry.MaterialHandle`)
- Deterministic hash of material properties
- Used for deduplication and batching
- Identical materials share the same GPU resource
- The only way to get one is by uploading a material to the GPU using a MaterialManager

#### 3. Handles for Deduplication

**MeshHandle** (`Flop.Core.Geometry.MeshHandle`)
- Deterministic hash of geometry parameters
- Two cylinders with identical radius/height/segments get the same handle
- Position/rotation are NOT included (those are per-instance)
- The only way to get a MeshHandle is by uploading a mesh to the GPU using a MeshManager

Both handles enable automatic sharing of GPU resources for identical geometry/materials.

#### 4. GPU Resource Management (Flop.Client)

Important note: in the below example, imagine our primitive is actually a cylinder.
If you previously happen to have uploaded an identical cylinder mesh to the GPU, then the mesh
management system is clever enough to notice the duplicate.
It won't actually upload the mesh to the GPU again - instead, it'll return the appropriate handle
and internally increment a reference count to that mesh.
If the reference count hits zero, we'll unload the mesh from the GPU.

**MeshManager** (`Flop.Core.Geometry.MeshManager`)
```csharp
// Upload a mesh to GPU, returns handle for later retrieval
MeshHandle UploadMesh(IGeometryPrimitive primitive)

// Get the GPU mesh from a handle
Mesh GetMesh(MeshHandle handle)

// Release a reference (auto-unloads at refcount=0)
void Release(MeshHandle handle)
```

**MaterialManager** (`Flop.Client.Rendering.MaterialManager`)
```csharp
// Upload a material to GPU, returns handle for later retrieval
MaterialHandle UploadMaterial(Flop.Core.Material material)

// Get the GPU material from a handle
Raylib_cs.Material GetMaterial(MaterialHandle handle)

// Release a reference (auto-unloads at refcount=0)
void Release(MaterialHandle handle)
```

Both managers:
- Cache GPU resources by handle
- Reference counting for automatic cleanup
- Only upload once per unique geometry/material
- Thread-local (not thread-safe, designed for single-threaded rendering)

#### 5. Batching System (Flop.Client)

**RenderBatch**
- Groups instances with identical (MeshHandle, MaterialHandle) pairs
- Stores array of Matrix4x4 transforms
- Renders via `DrawMeshInstanced` (single GPU call for many instances)

**RenderBatchCollection**
- Automatically organizes primitives into optimal batches
- Dictionary keyed by (MeshHandle, MaterialHandle)
- Cleared and rebuilt each frame

#### 6. Rendering Flow

**Setup (once per actor creation):**
```csharp
// When adding an actor to the world:
foreach (var primitive in actor.AllPrimitives())
{
    meshManager.UploadMesh(primitive);
    materialManager.UploadMaterial(primitive.Material);
}
```

**Per-Frame Rendering:**
```csharp
renderer.Render(renderables, camera);

// Internally:
// 1. Clear batch collection
// 2. For each renderable (IRenderable):
//    - Calculate world transform from Position/Rotation
//    - For each component:
//      - Calculate component transform (relative to rig)
//      - For each primitive:
//        - Calculate final transform: primitive × component × rig
//        - Add to batch keyed by (MeshHandle, MaterialHandle)
// 3. For each batch:
//    - mesh = meshManager.GetMesh(batch.MeshHandle)
//    - material = materialManager.GetMaterial(batch.MaterialHandle)
//    - DrawMeshInstanced(mesh, material, transforms, count)
```

**Cleanup (when removing actors):**
```csharp
foreach (var primitive in actor.AllPrimitives())
{
    meshManager.Release(primitive);
    materialManager.Release(primitive.Material);
}
```

### Key Design Decisions

**Why separate Upload/Get in managers?**
- Upload happens once during actor creation
- Get happens every frame during rendering
- Clear separation of concerns: initialization vs. usage
- Prevents accidental uploads during hot rendering loop

**Why store Material in primitives (Core) not Client?**
- Materials are intrinsic to geometry definition
- Compiler enforces all primitives have materials
- Simplifies actor code (no separate material management)
- Core.Material has no raylib dependency (just Color struct)

**Why use handles instead of direct references?**
- Enables automatic deduplication (1000 identical boxes → 1 GPU mesh)
- Enables efficient batching by (mesh, material) pairs
- Deterministic hashing ensures identical primitives get same handle
- Reference counting tracks when GPU resources can be freed

**Why rebuild batches every frame?**
- Actors move/rotate, so transforms change every frame anyway
- Batch building is cheap (just dictionary lookups and list additions)
- Avoids complex state tracking when actors are added/removed
- Clear ownership: Renderer doesn't hold references to actors

### Performance Characteristics

**Memory:**
- Shared GPU meshes: 1000 identical boxes use 1 mesh in VRAM
- Shared GPU materials: 1000 red objects with different meshes use 1 material in VRAM
- Batch collection: 1 batch per unique (mesh, material) pair
- Reference counting: 2 dictionaries per manager (handle→resource, handle→refcount)

**CPU:**
- Upload: O(1) amortized (hash lookup + occasional GPU upload)
- Batch building: O(primitives) per frame
- Rendering: O(batches) GPU calls, not O(primitives)

**GPU:**
- Instanced rendering: 1 draw call per batch instead of 1 per primitive
- 1000 identical boxes with same material = 1 draw call
- 1000 boxes with 10 materials = 10 draw calls (batched by material)

### Extension Points

**Adding texture support:**
1. Add `Texture? Texture` to `Flop.Core.Material`
2. Update `MaterialHandle` to hash texture ID
3. Update `RaylibMaterialLoader` to set texture map

**Adding custom shaders:**
1. Add `Shader? Shader` to `Flop.Core.Material`
2. Update `MaterialHandle` to hash shader ID
3. Update `RaylibMaterialLoader` to use custom shader

**Adding animated actors:**
1. IGeometryRig position/rotation can change per-frame
2. Batch collection rebuilds automatically
3. Mesh/material handles remain unchanged

## Notes

- `imgui.ini` will be generated automatically by Dear ImGui to save window layouts and is excluded from git

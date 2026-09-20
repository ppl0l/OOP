# Graphic Editor 🎨

A **WPF (Windows Presentation Foundation)** graphic editor application written in **C# / .NET**. It allows users to draw geometric shapes on a canvas, customize their stroke and fill colors, save/load drawings to JSON, undo/redo actions, and extend functionality via dynamically loaded **DLL plugins**.

> **Project:** OOP (Object-Oriented Programming) — University Assignment

## 📋 About The Project

**Graphic Editor** is a desktop application built with WPF that demonstrates core **Object-Oriented Programming** principles: inheritance, polymorphism, encapsulation, abstract classes, interfaces, and the **Factory** pattern. It was developed as part of an OOP university course and showcases clean architecture with pluggable shape modules.

### Key Features

- **Multiple Shape Types:** Rectangle, Ellipse, Line, Polygon, Polyline, Regular Polygon, Trapezoid.
- **Plugin System:** Dynamically load external DLLs to register new shape types at runtime.
- **Stroke & Fill Customization:** Change stroke color, fill color, and stroke thickness via the toolbar.
- **Interactive Drawing:** Click and drag on the canvas to draw shapes; use the right mouse button to close polygons.
- **Undo / Redo:** Full history stack with `Undo` and `Redo` buttons.
- **Save / Load:** Serialize the canvas to JSON via `Newtonsoft.Json` and restore it.
- **Clear Canvas:** Wipe all shapes with a single click.
- **Reflection-Based Plugin Loading:** Uses `Assembly.LoadFrom` to register any class that inherits from `MainShape`.

## 🛠 Technologies

- **C# / .NET** (WPF) — Desktop UI framework.
- **XAML** — Declarative UI layout.
- **Newtonsoft.Json** — JSON serialization and deserialization.
- **System.Reflection** — Dynamic plugin loading at runtime.
- **System.Windows.Media** — Brushes, Pens, and Colors.

## 📂 Project Structure

```text
OOP/
├── PluginTrapezoid/                     # Example plugin (Trapezoid shape as DLL)
├── graphicEditor/                       # Main WPF application
│   ├── ConvertJson/
│   │   ├── IShapeSerializable.cs        # Interface for JSON serialization
│   │   ├── ShapeDTO.cs                  # Data Transfer Object for shapes
│   │   └── ShapeSerializer.cs           # Save / Load logic
│   ├── Factory/
│   │   └── ShapeFactory.cs              # Shape registry + creation via reflection
│   ├── Plugins/
│   │   └── PluginLoader.cs              # Dynamically load DLL plugins
│   ├── PointExtensions/
│   │   └── PointExtensions.cs           # Distance() helper for Point
│   ├── Shapes/
│   │   ├── BasicShapes.cs               # RectangleShape & RoundShape abstract bases
│   │   ├── Ellipse.cs                   # Ellipse shape
│   │   ├── Line.cs                      # Line shape
│   │   ├── MainShape.cs                 # Abstract base class for all shapes
│   │   ├── Polygon.cs                   # Polygon (closed)
│   │   ├── Polyline.cs                  # Polyline (open)
│   │   ├── Rectangle.cs                 # Rectangle shape
│   │   └── RegularPolygon.cs            # N-sided regular polygon
│   ├── UndoRedo/
│   │   └── Undo_redo.cs                 # Undo / Redo stack manager
│   ├── App.xaml                         # Application entry
│   ├── App.xaml.cs
│   ├── AssemblyInfo.cs
│   ├── MainWindow.xaml                  # Main window UI (toolbar + canvas)
│   ├── MainWindow.xaml.cs               # Drawing, event handling, plugin loading
│   ├── graphicEditor.csproj
│   └── graphicEditor.sln
└── .gitignore
```

## 🚀 Installation and Setup

To run the project locally, follow these steps.

### 1. Clone the repository

```bash
git clone https://github.com/your-username/OOP.git
cd OOP
```

### 2. Open the solution

Open `graphicEditor/graphicEditor.sln` in **Visual Studio 2022** (or newer).

> ⚠️ **Requirements:** .NET Framework / .NET (WPF compatible), Windows OS.

### 3. Restore NuGet packages

In Visual Studio:

```
Tools → NuGet Package Manager → Manage NuGet Packages for Solution → Restore
```

Or via the CLI:

```bash
nuget restore graphicEditor/graphicEditor.sln
```

### 4. Build the solution

```
Build → Build Solution (Ctrl + Shift + B)
```

### 5. Run the application

Press **F5** or click the green ▶ **Start** button in Visual Studio.

The main window titled **"Graphic Editor"** will open.

### 6. Load the plugin (optional)

1. Build the `PluginTrapezoid` project first (right-click → **Build**).
2. In the main app, click the **`+`** button in the toolbar.
3. Select the compiled `PluginTrapezoid.dll` file.
4. A message box confirms: *"Фигура с именем Trapezoid и типом ... успешно зарегистрирована!"*
5. Select `Trapezoid` from the shape dropdown and draw.

## ✨ Implementation Details

### Architecture Overview

The project is organized around a **Factory + Abstract Class + Interface** design:

- **`MainShape`** — abstract base class for every drawable shape. Defines `Frame` (Pen), `Fill` (Brush), `RenderedElement`, and the `Render()` methods.
- **`RectangleShape`** / **`RoundShape`** — intermediate abstract classes for bounding-box-based and center-based shapes.
- **`IShapeSerializable`** — interface with `ToDTO()` and `FromDTO()` for JSON persistence.
- **`ShapeFactory`** — central registry of shape types, instantiates them via reflection.

### Factory Pattern

`ShapeFactory` maintains a `Dictionary<string, Type>` of registered shapes:

```csharp
public static void RegisterShape(string shapeName, Type shapeType);
public static MainShape CreateShape(string shapeName, params object[] parameters);
public static bool IsPolygon(string shapeName);
```

When the app starts, `PluginLoader.RegisterAllShapes()` scans the current assembly and registers every non-abstract `MainShape` subclass.

### Plugin System

`PluginLoader.LoadFromDll(path)`:

1. Uses `Assembly.LoadFrom(path)` to load the DLL.
2. Iterates over all types via reflection.
3. Registers every non-abstract class inheriting from `MainShape` into the `ShapeFactory`.
4. Shows a `MessageBox` confirming registration.

This design allows third-party developers to extend the editor **without recompiling the main app**.

### Shape Rendering

Each shape overrides two methods:

```csharp
public abstract Shape Render(Canvas canvas);
public abstract Shape Render(Canvas canvas, Brush fill, Pen frame);
```

Inside `Render`, the shape:

1. Creates the corresponding `System.Windows.Shapes.*` object.
2. Applies stroke, thickness, fill, and geometry.
3. Wires mouse events (`MouseLeftButtonUp`, `MouseRightButtonDown`) to bubble up to the canvas.
4. Adds itself to `canvas.Children` and stores the reference in `RenderedElement`.

### Undo / Redo

`Undo_redo` maintains two stacks:

- `curStack` — all applied actions.
- `redoStack` — popped actions that can be re-applied.

- **Undo:** `canvas.Children.Remove(element.RenderedElement)` + push to redo stack.
- **Redo:** `canvas.Children.Add(element.RenderedElement)` + push back to undo stack.
- Adding a new shape clears the redo stack (standard behavior).

### JSON Serialization

Every shape implements `IShapeSerializable`:

```csharp
public interface IShapeSerializable
{
    ShapeDTO ToDTO();
    void FromDTO(ShapeDTO dto);
}
```

`ShapeDTO` carries:

- `ShapeType` — the class name used to look up the factory.
- `Data` — a dictionary of geometry, fill, stroke, and thickness.

`ShapeSerializer.Save` / `ShapeSerializer.Load` handle the JSON file I/O using `Newtonsoft.Json`.

### Drawing Interaction

Handled in `MainWindow.xaml.cs` via canvas events:

- **`MouseLeftButtonDown`** (`StartPaint`) — records the starting point.
- **`PreviewMouseMove`** (`ProcessRender`) — previews the shape during drag.
- **`MouseLeftButtonUp`** (`EndPaint`) — finalizes and pushes to undo stack.
- **`MouseRightButtonDown`** (`PolyClickPaint`) — adds points to a polygon, right-click closes it.

### Toolbar Controls

- **Shape ComboBox** — choose the shape to draw.
- **Sides TextBox** — number of vertices for `RegularPolygon` (visible only when needed).
- **`+` Button** — load a plugin DLL.
- **Undo / Redo / Save / Load / Clear** — standard editing actions.
- **Stroke / Fill ComboBoxes** — pick colors.
- **Thickness ComboBox** — pick stroke width (1, 2, 3, 5, 10).

### Extensibility Example — Trapezoid Plugin

`PluginTrapezoid/Trapezoid.cs` demonstrates how to extend the editor:

```csharp
public class Trapezoid : RectangleShape, IShapeSerializable
{
    public override Shape Render(Canvas canvas, Brush fill, Pen frame) { ... }
    public ShapeDTO ToDTO() { ... }
    public void FromDTO(ShapeDTO dto) { ... }
}
```

Once compiled into a DLL, it can be loaded at runtime through the `+` button.

## 📞 Contacts

- **Repository:** [github.com/ppl0l/OOP](https://github.com/ppl0l/OOP)

---

*Project created as part of an Object-Oriented Programming university course.*

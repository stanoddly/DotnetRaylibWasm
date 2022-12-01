using Raylib_cs;

namespace RaylibGame;

public static class Program
{
    public static void Main()
    {
        Raylib.InitWindow(600, 400, "title");
            
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(new Color(255, 255, 255, 255));
            Raylib.DrawText("Congrats! You created your first window!", 0, 0, 20, new Color(0, 0, 0, 255));
            Raylib.EndDrawing();
        }
    }
}

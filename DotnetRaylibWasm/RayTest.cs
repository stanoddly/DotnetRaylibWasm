using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Raylib_cs;

namespace WebTest;

public static partial class RayTest
{
    [JSExport]
    public static void Start()
    {
        try
        {
            Console.WriteLine("About to call Raylib.");

            // TODO: Raylib.WindowShouldClose() is causing abort
            //while (Raylib.WindowShouldClose())
            {
                Raylib.InitWindow(600, 400, "title");
                Raylib.BeginDrawing();
                Raylib.ClearBackground(new Color(255, 255, 255, 255));
                Raylib.DrawText("Congrats! You created your first window!", 0, 0, 20, new Color(0, 0, 0, 255));
                Raylib.EndDrawing();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}

using Raylib_cs;

namespace RaylibGame;

public static class Program
{
    public static async Task Main()
    {
        try
        {
            Raylib.InitWindow(600, 400, "title");
            
            // See why Raylib.WindowShouldClose() isn't used:
            // https://github.com/disketteman/DotnetRaylibWasm/issues/1
            while (true)
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(new Color(255, 255, 255, 255));
                Raylib.DrawText("Congrats! You created your first window!", 0, 0, 20, new Color(0, 0, 0, 255));
                Raylib.EndDrawing();

                await Task.Delay(5);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}

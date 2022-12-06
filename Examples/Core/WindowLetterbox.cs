/*******************************************************************************************
*
*   raylib [core] example - window scale letterbox
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Anata (@anatagawa) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Anata (@anatagawa) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.TextureFilter;

namespace Examples.Core
{
    public class WindowLetterbox
    {
        public static int Main()
        {
            const int windowWidth = 800;
            const int windowHeight = 450;

            // Enable config flags for resizable window and vertical synchro
            SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE | ConfigFlags.FLAG_VSYNC_HINT);
            InitWindow(windowWidth, windowHeight, "raylib [core] example - window scale letterbox");
            SetWindowMinSize(320, 240);

            int gameScreenWidth = 640;
            int gameScreenHeight = 480;

            // Render texture initialization, used to hold the rendering result so we can easily resize it
            RenderTexture2D target = LoadRenderTexture(gameScreenWidth, gameScreenHeight);
            SetTextureFilter(target.texture, TEXTURE_FILTER_BILINEAR);  // Texture scale filter to use

            Color[] colors = new Color[10];
            for (int i = 0; i < 10; i++)
            {
                colors[i] = new Color(GetRandomValue(100, 250), GetRandomValue(50, 150), GetRandomValue(10, 100), 255);
            }

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // Compute required framebuffer scaling
                float scale = Math.Min((float)GetScreenWidth() / gameScreenWidth, (float)GetScreenHeight() / gameScreenHeight);

                if (IsKeyPressed(KEY_SPACE))
                {
                    // Recalculate random colors for the bars
                    for (int i = 0; i < 10; i++)
                    {
                        colors[i] = new Color(GetRandomValue(100, 250), GetRandomValue(50, 150), GetRandomValue(10, 100), 255);
                    }
                }

                // Update virtual mouse (clamped mouse value behind game screen)
                Vector2 mouse = GetMousePosition();
                Vector2 virtualMouse = Vector2.Zero;
                virtualMouse.X = (mouse.X - (GetScreenWidth() - (gameScreenWidth * scale)) * 0.5f) / scale;
                virtualMouse.Y = (mouse.Y - (GetScreenHeight() - (gameScreenHeight * scale)) * 0.5f) / scale;

                Vector2 max = new Vector2((float)gameScreenWidth, (float)gameScreenHeight);
                virtualMouse = Vector2.Clamp(virtualMouse, Vector2.Zero, max);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(BLACK);

                // Draw everything in the render texture, note this will not be rendered on screen, yet
                BeginTextureMode(target);
                ClearBackground(RAYWHITE);         // Clear render texture background color

                for (int i = 0; i < 10; i++)
                {
                    DrawRectangle(0, (gameScreenHeight / 10) * i, gameScreenWidth, gameScreenHeight / 10, colors[i]);
                }

                DrawText("If executed inside a window,\nyou can resize the window,\nand see the screen scaling!", 10, 25, 20, WHITE);

                DrawText($"Default Mouse: [{(int)mouse.X} {(int)mouse.Y}]", 350, 25, 20, GREEN);
                DrawText($"Virtual Mouse: [{(int)virtualMouse.X}, {(int)virtualMouse.Y}]", 350, 55, 20, YELLOW);

                EndTextureMode();

                // Draw RenderTexture2D to window, properly scaled
                Rectangle sourceRec = new Rectangle(
                    0.0f,
                    0.0f,
                    (float)target.texture.width,
                    (float)-target.texture.height
                );
                Rectangle destRec = new Rectangle(
                    (GetScreenWidth() - ((float)gameScreenWidth * scale)) * 0.5f,
                    (GetScreenHeight() - ((float)gameScreenHeight * scale)) * 0.5f,
                    (float)gameScreenWidth * scale,
                    (float)gameScreenHeight * scale
                );
                DrawTexturePro(target.texture, sourceRec, destRec, new Vector2(0, 0), 0.0f, WHITE);

                EndDrawing();
                //--------------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadRenderTexture(target);    // Unload render texture

            CloseWindow();                  // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

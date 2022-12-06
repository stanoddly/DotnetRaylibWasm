/*******************************************************************************************
*
*   raylib [core] example - window flags
*
*   This example has been created using raylib 3.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2020 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.ConfigFlags;
using static Raylib_cs.KeyboardKey;

namespace Examples.Core
{
    public class WindowFlags
    {
        public static int Main()
        {
            // Initialization
            //---------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            // Possible window flags
            /*
            FLAG_VSYNC_HINT
            FLAG_FULLSCREEN_MODE    -> not working properly -> wrong scaling!
            FLAG_WINDOW_RESIZABLE
            FLAG_WINDOW_UNDECORATED
            FLAG_WINDOW_TRANSPARENT
            FLAG_WINDOW_HIDDEN
            FLAG_WINDOW_MINIMIZED   -> Not supported on window creation
            FLAG_WINDOW_MAXIMIZED   -> Not supported on window creation
            FLAG_WINDOW_UNFOCUSED
            FLAG_WINDOW_TOPMOST
            FLAG_WINDOW_HIGHDPI     -> errors after minimize-resize, fb size is recalculated
            FLAG_WINDOW_ALWAYS_RUN
            FLAG_MSAA_4X_HINT
            */

            // Set configuration flags for window creation
            SetConfigFlags(FLAG_VSYNC_HINT | FLAG_MSAA_4X_HINT);
            InitWindow(screenWidth, screenHeight, "raylib [core] example - window flags");

            Vector2 ballPosition = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
            Vector2 ballSpeed = new Vector2(5.0f, 4.0f);
            int ballRadius = 20;

            int framesCounter = 0;
            //----------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //-----------------------------------------------------
                if (IsKeyPressed(KEY_F))
                    ToggleFullscreen();  // modifies window size when scaling!

                if (IsKeyPressed(KEY_R))
                {
                    if (IsWindowState(FLAG_WINDOW_RESIZABLE))
                        ClearWindowState(FLAG_WINDOW_RESIZABLE);
                    else
                        SetWindowState(FLAG_WINDOW_RESIZABLE);
                }

                if (IsKeyPressed(KEY_D))
                {
                    if (IsWindowState(FLAG_WINDOW_UNDECORATED))
                        ClearWindowState(FLAG_WINDOW_UNDECORATED);
                    else
                        SetWindowState(FLAG_WINDOW_UNDECORATED);
                }

                if (IsKeyPressed(KEY_H))
                {
                    if (!IsWindowState(FLAG_WINDOW_HIDDEN))
                        SetWindowState(FLAG_WINDOW_HIDDEN);

                    framesCounter = 0;
                }

                if (IsWindowState(FLAG_WINDOW_HIDDEN))
                {
                    framesCounter++;
                    if (framesCounter >= 240)
                        ClearWindowState(FLAG_WINDOW_HIDDEN); // Show window after 3 seconds
                }

                if (IsKeyPressed(KEY_N))
                {
                    if (!IsWindowState(FLAG_WINDOW_MINIMIZED))
                        MinimizeWindow();
                    framesCounter = 0;
                }

                if (IsWindowState(FLAG_WINDOW_MINIMIZED))
                {
                    framesCounter++;
                    if (framesCounter >= 240)
                        RestoreWindow(); // Restore window after 3 seconds
                }

                if (IsKeyPressed(KEY_M))
                {
                    // NOTE: Requires FLAG_WINDOW_RESIZABLE enabled!
                    if (IsWindowState(FLAG_WINDOW_MAXIMIZED))
                        RestoreWindow();
                    else
                        MaximizeWindow();
                }

                if (IsKeyPressed(KEY_U))
                {
                    if (IsWindowState(FLAG_WINDOW_UNFOCUSED))
                        ClearWindowState(FLAG_WINDOW_UNFOCUSED);
                    else
                        SetWindowState(FLAG_WINDOW_UNFOCUSED);
                }

                if (IsKeyPressed(KEY_T))
                {
                    if (IsWindowState(FLAG_WINDOW_TOPMOST))
                        ClearWindowState(FLAG_WINDOW_TOPMOST);
                    else
                        SetWindowState(FLAG_WINDOW_TOPMOST);
                }

                if (IsKeyPressed(KEY_A))
                {
                    if (IsWindowState(FLAG_WINDOW_ALWAYS_RUN))
                        ClearWindowState(FLAG_WINDOW_ALWAYS_RUN);
                    else
                        SetWindowState(FLAG_WINDOW_ALWAYS_RUN);
                }

                if (IsKeyPressed(KEY_V))
                {
                    if (IsWindowState(FLAG_VSYNC_HINT))
                        ClearWindowState(FLAG_VSYNC_HINT);
                    else
                        SetWindowState(FLAG_VSYNC_HINT);
                }

                // Bouncing ball logic
                ballPosition.X += ballSpeed.X;
                ballPosition.Y += ballSpeed.Y;
                if ((ballPosition.X >= (GetScreenWidth() - ballRadius)) || (ballPosition.X <= ballRadius))
                {
                    ballSpeed.X *= -1.0f;
                }
                if ((ballPosition.Y >= (GetScreenHeight() - ballRadius)) || (ballPosition.Y <= ballRadius))
                {
                    ballSpeed.Y *= -1.0f;
                }
                //-----------------------------------------------------

                // Draw
                //-----------------------------------------------------
                BeginDrawing();

                if (IsWindowState(FLAG_WINDOW_TRANSPARENT))
                    ClearBackground(BLANK);
                else
                    ClearBackground(RAYWHITE);

                DrawCircleV(ballPosition, ballRadius, MAROON);
                DrawRectangleLinesEx(new Rectangle(0, 0, GetScreenWidth(), GetScreenHeight()), 4, RAYWHITE);

                DrawCircleV(GetMousePosition(), 10, DARKBLUE);

                DrawFPS(10, 10);

                DrawText($"Screen Size: [{GetScreenWidth()}, {GetScreenHeight()}]", 10, 40, 10, GREEN);

                // Draw window state info
                Color on = Color.LIME;
                Color off = Color.MAROON;

                DrawText("Following flags can be set after window creation:", 10, 60, 10, GRAY);

                DrawWindowState(FLAG_FULLSCREEN_MODE, "[F] FLAG_FULLSCREEN_MODE: ", 10, 80, 10);
                DrawWindowState(FLAG_WINDOW_RESIZABLE, "[R] FLAG_WINDOW_RESIZABLE: ", 10, 100, 10);
                DrawWindowState(FLAG_WINDOW_UNDECORATED, "[D] FLAG_WINDOW_UNDECORATED: ", 10, 120, 10);
                DrawWindowState(FLAG_WINDOW_HIDDEN, "[H] FLAG_WINDOW_HIDDEN: ", 10, 140, 10);
                DrawWindowState(FLAG_WINDOW_MINIMIZED, "[N] FLAG_WINDOW_MINIMIZED: ", 10, 160, 10);
                DrawWindowState(FLAG_WINDOW_MAXIMIZED, "[M] FLAG_WINDOW_MAXIMIZED: ", 10, 180, 10);
                DrawWindowState(FLAG_WINDOW_UNFOCUSED, "[G] FLAG_WINDOW_UNFOCUSED: ", 10, 200, 10);
                DrawWindowState(FLAG_WINDOW_TOPMOST, "[T] FLAG_WINDOW_TOPMOST: ", 10, 220, 10);
                DrawWindowState(FLAG_WINDOW_ALWAYS_RUN, "[A] FLAG_WINDOW_ALWAYS_RUN: ", 10, 240, 10);
                DrawWindowState(FLAG_VSYNC_HINT, "[V] FLAG_VSYNC_HINT: ", 10, 260, 10);

                DrawText("Following flags can only be set before window creation:", 10, 300, 10, GRAY);

                DrawWindowState(FLAG_WINDOW_HIGHDPI, "[F] FLAG_WINDOW_HIGHDPI: ", 10, 320, 10);
                DrawWindowState(FLAG_WINDOW_TRANSPARENT, "[F] FLAG_WINDOW_TRANSPARENT: ", 10, 340, 10);
                DrawWindowState(FLAG_MSAA_4X_HINT, "[F] FLAG_MSAA_4X_HINT: ", 10, 360, 10);

                EndDrawing();
                //-----------------------------------------------------
            }

            // De-Initialization
            //---------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //----------------------------------------------------------

            return 0;
        }

        static void DrawWindowState(ConfigFlags flag, string text, int posX, int posY, int fontSize)
        {
            Color onColor = Color.LIME;
            Color offColor = Color.MAROON;

            if (Raylib.IsWindowState(flag))
            {
                DrawText($"{text} on", posX, posY, fontSize, onColor);
            }
            else
            {
                DrawText($"{text} off", posX, posY, fontSize, offColor);
            }
        }
    }
}

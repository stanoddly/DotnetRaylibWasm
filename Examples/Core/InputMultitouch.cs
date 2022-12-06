/*******************************************************************************************
*
*   raylib [core] example - Input multitouch
*
*   This example has been created using raylib 2.1 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Berni (@Berni8k) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Berni (@Berni8k) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.MouseButton;

namespace Examples.Core
{
    public class InputMultitouch
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - input multitouch");

            Vector2 ballPosition = new Vector2(-100.0f, -100.0f);
            Color ballColor = BEIGE;

            int touchCounter = 0;
            Vector2 touchPosition = new Vector2(0.0f, 0.0f);

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //---------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                ballPosition = GetMousePosition();

                ballColor = BEIGE;

                if (IsMouseButtonDown(MOUSE_LEFT_BUTTON))
                    ballColor = MAROON;
                if (IsMouseButtonDown(MOUSE_MIDDLE_BUTTON))
                    ballColor = LIME;
                if (IsMouseButtonDown(MOUSE_RIGHT_BUTTON))
                    ballColor = DARKBLUE;

                if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON))
                    touchCounter = 10;
                if (IsMouseButtonPressed(MOUSE_MIDDLE_BUTTON))
                    touchCounter = 10;
                if (IsMouseButtonPressed(MOUSE_RIGHT_BUTTON))
                    touchCounter = 10;

                if (touchCounter > 0)
                    touchCounter--;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // Multitouch
                for (int i = 0; i < 10; ++i)
                {
                    touchPosition = GetTouchPosition(i);                    // Get the touch point

                    if ((touchPosition.X >= 0) && (touchPosition.Y >= 0))   // Make sure point is not (-1,-1) as this means there is no touch for it
                    {
                        // Draw circle and touch index number
                        DrawCircleV(touchPosition, 34, ORANGE);
                        DrawText($"{i}", (int)(touchPosition.X - 10), (int)(touchPosition.Y - 70), 40, BLACK);
                    }
                }

                // Draw the normal mouse location
                DrawCircleV(ballPosition, 30 + (touchCounter * 3), ballColor);

                DrawText("move ball with mouse and click mouse button to change color", 10, 10, 20, DARKGRAY);
                DrawText("touch the screen at multiple locations to get multiple balls", 10, 30, 20, DARKGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

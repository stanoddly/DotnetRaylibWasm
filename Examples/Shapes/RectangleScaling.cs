/*******************************************************************************************
*
*   raylib [shapes] example - rectangle scaling by mouse
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Vlad Adrian (@demizdor) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2018 Vlad Adrian (@demizdor) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.MouseButton;

namespace Examples.Shapes
{
    public class RectangleScaling
    {
        public const int MOUSE_SCALE_MARK_SIZE = 12;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - rectangle scaling mouse");

            Rectangle rec = new Rectangle(100, 100, 200, 80);
            Vector2 mousePosition = new Vector2(0, 0);

            bool mouseScaleReady = false;
            bool mouseScaleMode = false;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                mousePosition = GetMousePosition();

                Rectangle area = new Rectangle(
                    rec.x + rec.width - MOUSE_SCALE_MARK_SIZE,
                    rec.y + rec.height - MOUSE_SCALE_MARK_SIZE,
                    MOUSE_SCALE_MARK_SIZE,
                    MOUSE_SCALE_MARK_SIZE
                );

                if (CheckCollisionPointRec(mousePosition, rec) &&
                    CheckCollisionPointRec(mousePosition, area))
                {
                    mouseScaleReady = true;
                    if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON))
                    {
                        mouseScaleMode = true;
                    }
                }
                else
                {
                    mouseScaleReady = false;
                }

                if (mouseScaleMode)
                {
                    mouseScaleReady = true;

                    rec.width = (mousePosition.X - rec.x);
                    rec.height = (mousePosition.Y - rec.y);

                    if (rec.width < MOUSE_SCALE_MARK_SIZE)
                    {
                        rec.width = MOUSE_SCALE_MARK_SIZE;
                    }
                    if (rec.height < MOUSE_SCALE_MARK_SIZE)
                    {
                        rec.height = MOUSE_SCALE_MARK_SIZE;
                    }

                    if (IsMouseButtonReleased(MOUSE_LEFT_BUTTON))
                    {
                        mouseScaleMode = false;
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawText("Scale rectangle dragging from bottom-right corner!", 10, 10, 20, GRAY);
                DrawRectangleRec(rec, ColorAlpha(GREEN, 0.5f));

                if (mouseScaleReady)
                {
                    DrawRectangleLinesEx(rec, 1, RED);
                    DrawTriangle(
                        new Vector2(rec.x + rec.width - MOUSE_SCALE_MARK_SIZE, rec.y + rec.height),
                        new Vector2(rec.x + rec.width, rec.y + rec.height),
                        new Vector2(rec.x + rec.width, rec.y + rec.height - MOUSE_SCALE_MARK_SIZE), RED
                    );
                }

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

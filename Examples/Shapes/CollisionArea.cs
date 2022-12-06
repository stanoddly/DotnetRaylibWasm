/*******************************************************************************************
*
*   raylib [shapes] example - collision area
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2013-2019 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples.Shapes
{
    public class CollisionArea
    {
        public static int Main()
        {
            // Initialization
            //---------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - collision area");

            // Box A: Moving box
            Rectangle boxA = new Rectangle(10, GetScreenHeight() / 2 - 50, 200, 100);
            int boxASpeedX = 4;

            // Box B: Mouse moved box
            Rectangle boxB = new Rectangle(GetScreenWidth() / 2 - 30, GetScreenHeight() / 2 - 30, 60, 60);
            Rectangle boxCollision = new Rectangle();

            int screenUpperLimit = 40;

            // Movement pause
            bool pause = false;
            bool collision = false;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //----------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //-----------------------------------------------------
                // Move box if not paused
                if (!pause)
                {
                    boxA.x += boxASpeedX;
                }

                // Bounce box on x screen limits
                if (((boxA.x + boxA.width) >= GetScreenWidth()) || (boxA.x <= 0))
                {
                    boxASpeedX *= -1;
                }

                // Update player-controlled-box (box02)
                boxB.x = GetMouseX() - boxB.width / 2;
                boxB.y = GetMouseY() - boxB.height / 2;

                // Make sure Box B does not go out of move area limits
                if ((boxB.x + boxB.width) >= GetScreenWidth())
                {
                    boxB.x = GetScreenWidth() - boxB.width;
                }
                else if (boxB.x <= 0)
                {
                    boxB.x = 0;
                }

                if ((boxB.y + boxB.height) >= GetScreenHeight())
                {
                    boxB.y = GetScreenHeight() - boxB.height;
                }
                else if (boxB.y <= screenUpperLimit)
                {
                    boxB.y = screenUpperLimit;
                }

                // Check boxes collision
                collision = CheckCollisionRecs(boxA, boxB);

                // Get collision rectangle (only on collision)
                if (collision)
                {
                    boxCollision = GetCollisionRec(boxA, boxB);
                }

                // Pause Box A movement
                if (IsKeyPressed(KEY_SPACE))
                {
                    pause = !pause;
                }
                //-----------------------------------------------------

                // Draw
                //-----------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawRectangle(0, 0, screenWidth, screenUpperLimit, collision ? RED : BLACK);

                DrawRectangleRec(boxA, GOLD);
                DrawRectangleRec(boxB, BLUE);

                if (collision)
                {
                    // Draw collision area
                    DrawRectangleRec(boxCollision, LIME);

                    // Draw collision message
                    int cx = GetScreenWidth() / 2 - MeasureText("COLLISION!", 20) / 2;
                    int cy = screenUpperLimit / 2 - 10;
                    DrawText("COLLISION!", cx, cy, 20, BLACK);

                    // Draw collision area
                    string text = $"Collision Area: {(int)boxCollision.width * (int)boxCollision.height}";
                    DrawText(text, GetScreenWidth() / 2 - 100, screenUpperLimit + 10, 20, BLACK);
                }

                DrawFPS(10, 10);

                EndDrawing();
                //-----------------------------------------------------
            }

            // De-Initialization
            //---------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //----------------------------------------------------------

            return 0;
        }
    }
}

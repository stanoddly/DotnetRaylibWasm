/*******************************************************************************************
*
*   raylib [textures] example - Texture loading and drawing a part defined by a rectangle
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples.Textures
{
    public class SpriteAnim
    {
        public const int MAX_FRAME_SPEED = 15;
        public const int MIN_FRAME_SPEED = 1;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [texture] example - texture rectangle");

            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            Texture2D scarfy = LoadTexture("resources/scarfy.png");

            Vector2 position = new Vector2(350.0f, 280.0f);
            Rectangle frameRec = new Rectangle(0.0f, 0.0f, (float)scarfy.width / 6, (float)scarfy.height);
            int currentFrame = 0;

            int framesCounter = 0;

            // Number of spritesheet frames shown by second
            int framesSpeed = 8;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                framesCounter++;

                if (framesCounter >= (60 / framesSpeed))
                {
                    framesCounter = 0;
                    currentFrame++;

                    if (currentFrame > 5)
                    {
                        currentFrame = 0;
                    }

                    frameRec.x = (float)currentFrame * (float)scarfy.width / 6;
                }

                if (IsKeyPressed(KEY_RIGHT))
                {
                    framesSpeed++;
                }
                else if (IsKeyPressed(KEY_LEFT))
                {
                    framesSpeed--;
                }

                framesSpeed = Math.Clamp(framesSpeed, MIN_FRAME_SPEED, MAX_FRAME_SPEED);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawTexture(scarfy, 15, 40, WHITE);
                DrawRectangleLines(15, 40, scarfy.width, scarfy.height, LIME);
                DrawRectangleLines(
                    15 + (int)frameRec.x,
                    40 + (int)frameRec.y,
                    (int)frameRec.width,
                    (int)frameRec.height,
                    RED
                );

                DrawText("FRAME SPEED: ", 165, 210, 10, DARKGRAY);
                DrawText(string.Format("{0:00} FPS", framesSpeed), 575, 210, 10, DARKGRAY);
                DrawText("PRESS RIGHT/LEFT KEYS to CHANGE SPEED!", 290, 240, 10, DARKGRAY);

                for (int i = 0; i < MAX_FRAME_SPEED; i++)
                {
                    if (i < framesSpeed)
                    {
                        DrawRectangle(250 + 21 * i, 205, 20, 20, RED);
                    }
                    DrawRectangleLines(250 + 21 * i, 205, 20, 20, MAROON);
                }

                // Draw part of the texture
                DrawTextureRec(scarfy, frameRec, position, WHITE);
                DrawText("(c) Scarfy sprite by Eiden Marsal", screenWidth - 200, screenHeight - 20, 10, GRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(scarfy);       // Texture unloading

            CloseWindow();                // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

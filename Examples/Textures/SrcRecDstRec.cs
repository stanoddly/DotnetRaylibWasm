/*******************************************************************************************
*
*   raylib [textures] example - Texture source and destination rectangles
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace Examples.Textures
{
    public class SrcRecDstRec
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            string title = "raylib [textures] examples - texture source and destination rectangles";
            InitWindow(screenWidth, screenHeight, title);

            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            Texture2D scarfy = LoadTexture("resources/scarfy.png");

            int frameWidth = scarfy.width / 6;
            int frameHeight = scarfy.height;

            // NOTE: Source rectangle (part of the texture to use for drawing)
            Rectangle sourceRec = new Rectangle(0, 0, frameWidth, frameHeight);

            // NOTE: Destination rectangle (screen rectangle where drawing part of texture)
            Rectangle destRec = new Rectangle(screenWidth / 2, screenHeight / 2, frameWidth * 2, frameHeight * 2);

            // NOTE: Origin of the texture (rotation/scale point), it's relative to destination rectangle size
            Vector2 origin = new Vector2(frameWidth, frameHeight);

            int rotation = 0;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                rotation++;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // NOTE: Using DrawTexturePro() we can easily rotate and scale the part of the texture we draw
                // sourceRec defines the part of the texture we use for drawing
                // destRec defines the rectangle where our texture part will fit (scaling it to fit)
                // origin defines the point of the texture used as reference for rotation and scaling
                // rotation defines the texture rotation (using origin as rotation point)
                DrawTexturePro(scarfy, sourceRec, destRec, origin, rotation, WHITE);

                DrawLine((int)destRec.x, 0, (int)destRec.x, screenHeight, GRAY);
                DrawLine(0, (int)destRec.y, screenWidth, (int)destRec.y, GRAY);

                DrawText("(c) Scarfy sprite by Eiden Marsal", screenWidth - 200, screenHeight - 20, 10, GRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(scarfy);        // Texture unloading

            CloseWindow();                // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

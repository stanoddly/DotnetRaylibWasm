/*******************************************************************************************
*
*   raylib [shapes] example - Draw Textured Polygon
*
*   This example has been created using raylib 99.98 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*   Copyright (c) 2021 Chris Camacho (codifies - bedroomcoders.co.uk)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace Examples.Textures
{
    public class Polygon
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [textures] example - Textured Polygon");

            Vector2[] texcoords = new[] {
                new Vector2(0.75f, 0),
                new Vector2(0.25f, 0),
                new Vector2(0, 0.5f),
                new Vector2(0, 0.75f),
                new Vector2(0.25f, 1),
                new Vector2(0.375f, 0.875f),
                new Vector2(0.625f, 0.875f),
                new Vector2(0.75f, 1),
                new Vector2(1, 0.75f),
                new Vector2(1, 0.5f),
                // Close the poly
                new Vector2(0.75f, 0)
            };

            Vector2[] points = new Vector2[11];

            // Define the base poly vertices from the UV's
            // NOTE: They can be specified in any other way
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = (texcoords[i].X - 0.5f) * 256.0f;
                points[i].Y = (texcoords[i].Y - 0.5f) * 256.0f;
            }

            // Define the vertices drawing position
            // NOTE: Initially same as points but updated every frame
            Vector2[] positions = new Vector2[points.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = points[i];
            }

            Texture2D texture = LoadTexture("resources/cat.png");
            float angle = 0;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // Update your variables here
                //----------------------------------------------------------------------------------
                angle += 1;
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] = Raymath.Vector2Rotate(points[i], angle * Raylib.DEG2RAD);
                }

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawText("Textured Polygon", 20, 20, 20, DARKGRAY);
                Vector2 center = new Vector2(screenWidth / 2, screenHeight / 2);
                DrawTexturePoly(texture, center, positions, texcoords, positions.Length, WHITE);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            UnloadTexture(texture);

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

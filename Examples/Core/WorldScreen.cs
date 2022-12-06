/*******************************************************************************************
*
*   raylib [core] example - World to screen
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
using static Raylib_cs.CameraProjection;
using static Raylib_cs.CameraMode;
using static Raylib_cs.Color;

namespace Examples.Core
{
    public class WorldScreen
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d camera free");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(10.0f, 10.0f, 10.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            Vector3 cubePosition = new Vector3(0.0f, 0.0f, 0.0f);
            Vector2 cubeScreenPosition;

            SetCameraMode(camera, CAMERA_FREE); // Set a free camera mode

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);          // Update camera

                // Calculate cube screen space position (with a little offset to be in top)
                cubeScreenPosition = GetWorldToScreen(new Vector3(cubePosition.X, cubePosition.Y + 2.5f, cubePosition.Z), camera);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                DrawCube(cubePosition, 2.0f, 2.0f, 2.0f, RED);
                DrawCubeWires(cubePosition, 2.0f, 2.0f, 2.0f, MAROON);

                DrawGrid(10, 1.0f);

                EndMode3D();

                DrawText("Enemy: 100 / 100", (int)cubeScreenPosition.X - MeasureText("Enemy: 100 / 100", 20) / 2, (int)cubeScreenPosition.Y, 20, BLACK);
                DrawText("Text is always on top of the cube", (screenWidth - MeasureText("Text is always on top of the cube", 20)) / 2, 25, 20, GRAY);

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

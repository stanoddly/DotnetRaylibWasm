/*******************************************************************************************
*
*   raylib [core] example - Picking in 3d mode
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
using static Raylib_cs.MouseButton;
using static Raylib_cs.Color;

namespace Examples.Core
{
    public class Picking3d
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d picking");

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(10.0f, 10.0f, 10.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            Vector3 cubePosition = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 cubeSize = new Vector3(2.0f, 2.0f, 2.0f);

            // Picking line ray
            Ray ray = new Ray(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Zero);
            RayCollision collision = new RayCollision();

            SetCameraMode(camera, CAMERA_FREE); // Set a free camera mode

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose()) // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);          // Update camera

                if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON))
                {
                    if (!collision.hit)
                    {
                        ray = GetMouseRay(GetMousePosition(), camera);

                        // Check collision between ray and box
                        BoundingBox box = new BoundingBox(
                            cubePosition - cubeSize / 2,
                            cubePosition + cubeSize / 2
                        );
                        collision = GetRayCollisionBox(ray, box);
                    }
                    else
                    {
                        collision.hit = false;
                    }

                    ray = GetMouseRay(GetMousePosition(), camera);
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                if (collision.hit)
                {
                    DrawCube(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, RED);
                    DrawCubeWires(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, MAROON);

                    DrawCubeWires(cubePosition, cubeSize.X + 0.2f, cubeSize.Y + 0.2f, cubeSize.Z + 0.2f, GREEN);
                }
                else
                {
                    DrawCube(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, GRAY);
                    DrawCubeWires(cubePosition, cubeSize.X, cubeSize.Y, cubeSize.Z, DARKGRAY);
                }

                DrawRay(ray, MAROON);
                DrawGrid(10, 1.0f);

                EndMode3D();

                DrawText("Try selecting the box with mouse!", 240, 10, 20, DARKGRAY);

                if (collision.hit)
                {
                    int posX = (screenWidth - MeasureText("BOX SELECTED", 30)) / 2;
                    DrawText("BOX SELECTED", posX, (int)(screenHeight * 0.1f), 30, GREEN);
                }

                DrawFPS(10, 10);

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

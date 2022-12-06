/*******************************************************************************************
*
*   raylib [models] example - Detect basic 3d collisions (box vs sphere vs box)
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
using static Raylib_cs.KeyboardKey;

namespace Examples.Models
{
    public class BoxCollisions
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - box collisions");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D(
                new Vector3(0.0f, 10.0f, 10.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                45.0f,
                CameraProjection.CAMERA_PERSPECTIVE
            );

            Vector3 playerPosition = new Vector3(0.0f, 1.0f, 2.0f);
            Vector3 playerSize = new Vector3(1.0f, 2.0f, 1.0f);
            Color playerColor = GREEN;

            Vector3 enemyBoxPos = new Vector3(-4.0f, 1.0f, 0.0f);
            Vector3 enemyBoxSize = new Vector3(2.0f, 2.0f, 2.0f);

            Vector3 enemySpherePos = new Vector3(4.0f, 0.0f, 0.0f);
            float enemySphereSize = 1.5f;

            bool collision = false;

            SetTargetFPS(60);   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------

                // Move player
                if (IsKeyDown(KEY_RIGHT))
                    playerPosition.X += 0.2f;
                else if (IsKeyDown(KEY_LEFT))
                    playerPosition.X -= 0.2f;
                else if (IsKeyDown(KEY_DOWN))
                    playerPosition.Z += 0.2f;
                else if (IsKeyDown(KEY_UP))
                    playerPosition.Z -= 0.2f;

                collision = false;

                // Check collisions player vs enemy-box
                BoundingBox box1 = new BoundingBox(
                    playerPosition - (playerSize / 2),
                    playerPosition + (playerSize / 2)
                );
                BoundingBox box2 = new BoundingBox(
                    enemyBoxPos - (enemyBoxSize / 2),
                    enemyBoxPos + (enemyBoxSize / 2)
                );

                if (CheckCollisionBoxes(box1, box2))
                    collision = true;

                // Check collisions player vs enemy-sphere
                if (CheckCollisionBoxSphere(box1, enemySpherePos, enemySphereSize))
                    collision = true;

                if (collision)
                {
                    playerColor = RED;
                }
                else
                {
                    playerColor = GREEN;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                // Draw enemy-box
                DrawCube(enemyBoxPos, enemyBoxSize.X, enemyBoxSize.Y, enemyBoxSize.Z, GRAY);
                DrawCubeWires(enemyBoxPos, enemyBoxSize.X, enemyBoxSize.Y, enemyBoxSize.Z, DARKGRAY);

                // Draw enemy-sphere
                DrawSphere(enemySpherePos, enemySphereSize, GRAY);
                DrawSphereWires(enemySpherePos, enemySphereSize, 16, 16, DARKGRAY);

                // Draw player
                DrawCubeV(playerPosition, playerSize, playerColor);

                DrawGrid(10, 1.0f);        // Draw a grid

                EndMode3D();

                DrawText("Move player with cursors to collide", 220, 40, 20, GRAY);
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

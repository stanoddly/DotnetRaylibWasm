/*******************************************************************************************
*
*   raylib [models] example - first person maze
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2019 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.MaterialMapIndex;
using static Raylib_cs.CameraMode;

namespace Examples.Models
{
    public class FirstPersonMaze
    {
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - first person maze");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D(
                new Vector3(0.2f, 0.4f, 0.2f),
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(0.0f, 1.0f, 0.0f),
                45.0f,
                CameraProjection.CAMERA_PERSPECTIVE
            );

            Image imMap = LoadImage("resources/cubicmap.png");      // Load cubicmap image (RAM)
            Texture2D cubicmap = LoadTextureFromImage(imMap);       // Convert image to texture to display (VRAM)
            Mesh mesh = GenMeshCubicmap(imMap, new Vector3(1.0f, 1.0f, 1.0f));
            Model model = LoadModelFromMesh(mesh);

            // NOTE: By default each cube is mapped to one part of texture atlas
            Texture2D texture = LoadTexture("resources/cubicmap_atlas.png");    // Load map texture

            // Set map diffuse texture
            Raylib.SetMaterialTexture(ref model, 0, MATERIAL_MAP_ALBEDO, ref texture);

            // Get map image data to be used for collision detection
            Color* mapPixels = LoadImageColors(imMap);
            UnloadImage(imMap);             // Unload image from RAM

            Vector3 mapPosition = new Vector3(-16.0f, 0.0f, -8.0f);  // Set model position
            Vector3 playerPosition = camera.position;       // Set player position

            SetCameraMode(camera, CAMERA_FIRST_PERSON);     // Set camera mode

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                Vector3 oldCamPos = camera.position;    // Store old camera position

                UpdateCamera(ref camera);      // Update camera

                // Check player collision (we simplify to 2D collision detection)
                Vector2 playerPos = new Vector2(camera.position.X, camera.position.Z);
                float playerRadius = 0.1f;  // Collision radius (player is modelled as a cilinder for collision)

                int playerCellX = (int)(playerPos.X - mapPosition.X + 0.5f);
                int playerCellY = (int)(playerPos.Y - mapPosition.Z + 0.5f);

                // Out-of-limits security check
                if (playerCellX < 0)
                    playerCellX = 0;
                else if (playerCellX >= cubicmap.width)
                    playerCellX = cubicmap.width - 1;

                if (playerCellY < 0)
                    playerCellY = 0;
                else if (playerCellY >= cubicmap.height)
                    playerCellY = cubicmap.height - 1;

                // Check map collisions using image data and player position
                // TODO: Improvement: Just check player surrounding cells for collision
                for (int y = 0; y < cubicmap.height; y++)
                {
                    for (int x = 0; x < cubicmap.width; x++)
                    {
                        Color* mapPixelsData = mapPixels;

                        // Collision: white pixel, only check R channel
                        Rectangle rec = new Rectangle(
                            mapPosition.X - 0.5f + x * 1.0f,
                            mapPosition.Z - 0.5f + y * 1.0f,
                            1.0f,
                            1.0f
                        );

                        bool collision = CheckCollisionCircleRec(playerPos, playerRadius, rec);
                        if ((mapPixelsData[y * cubicmap.width + x].r == 255) && collision)
                        {
                            // Collision detected, reset camera position
                            camera.position = oldCamPos;
                        }
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // Draw maze map
                BeginMode3D(camera);
                DrawModel(model, mapPosition, 1.0f, WHITE);
                EndMode3D();

                DrawTextureEx(cubicmap, new Vector2(GetScreenWidth() - cubicmap.width * 4 - 20, 20), 0.0f, 4.0f, WHITE);
                DrawRectangleLines(GetScreenWidth() - cubicmap.width * 4 - 20, 20, cubicmap.width * 4, cubicmap.height * 4, GREEN);

                // Draw player position radar
                DrawRectangle(GetScreenWidth() - cubicmap.width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4, RED);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadImageColors(mapPixels);

            UnloadTexture(cubicmap);    // Unload cubicmap texture
            UnloadTexture(texture);     // Unload map texture
            UnloadModel(model);         // Unload map model

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

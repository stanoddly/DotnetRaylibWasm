/*******************************************************************************************
*
*   raylib [models] example - Plane rotations (yaw, pitch, roll)
*
*   This example has been created using raylib 1.8 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Berni (@Berni8k) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2017-2021 Berni (@Berni8k) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MaterialMapIndex;

namespace Examples.Models
{
    public class YawPitchRoll
    {
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - plane rotations (yaw, pitch, roll)");

            Camera3D camera = new Camera3D();
            camera.position = new Vector3(0.0f, 50.0f, -120.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 30.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            // Model loading
            Model model = LoadModel("resources/models/obj/plane.obj");
            Texture2D texture = LoadTexture("resources/models/obj/plane_diffuse.png");
            model.materials[0].maps[(int)MATERIAL_MAP_DIFFUSE].texture = texture;

            float pitch = 0.0f;
            float roll = 0.0f;
            float yaw = 0.0f;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------

                // Plane roll (x-axis) controls
                if (IsKeyDown(KEY_DOWN))
                {
                    pitch += 0.6f;
                }
                else if (IsKeyDown(KEY_UP))
                {
                    pitch -= 0.6f;
                }
                else
                {
                    if (pitch > 0.3f)
                    {
                        pitch -= 0.3f;
                    }
                    else if (pitch < -0.3f)
                    {
                        pitch += 0.3f;
                    }
                }

                // Plane yaw (y-axis) controls
                if (IsKeyDown(KEY_S))
                {
                    yaw += 1.0f;
                }
                else if (IsKeyDown(KEY_A))
                {
                    yaw -= 1.0f;
                }
                else
                {
                    if (yaw > 0.0f)
                    {
                        yaw -= 0.5f;
                    }
                    else if (yaw < 0.0f)
                    {
                        yaw += 0.5f;
                    }
                }

                // Plane pitch (z-axis) controls
                if (IsKeyDown(KEY_LEFT))
                {
                    roll += 1.0f;
                }
                else if (IsKeyDown(KEY_RIGHT))
                {
                    roll -= 1.0f;
                }
                else
                {
                    if (roll > 0.0f)
                    {
                        roll -= 0.5f;
                    }
                    else if (roll < 0.0f)
                    {
                        roll += 0.5f;
                    }
                }

                // Tranformation matrix for rotations
                model.transform = MatrixRotateXYZ(new Vector3(DEG2RAD * pitch, DEG2RAD * yaw, DEG2RAD * roll));
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // Draw 3D model (recomended to draw 3D always before 2D)
                BeginMode3D(camera);

                // Draw 3d model with texture
                DrawModel(model, new Vector3(0.0f, -8.0f, 0.0f), 1.0f, WHITE);
                DrawGrid(10, 10.0f);

                EndMode3D();

                // Draw controls info
                DrawRectangle(30, 370, 260, 70, Fade(GREEN, 0.5f));
                DrawRectangleLines(30, 370, 260, 70, Fade(DARKGREEN, 0.5f));
                DrawText("Pitch controlled with: KEY_UP / KEY_DOWN", 40, 380, 10, DARKGRAY);
                DrawText("Roll controlled with: KEY_LEFT / KEY_RIGHT", 40, 400, 10, DARKGRAY);
                DrawText("Yaw controlled with: KEY_A / KEY_S", 40, 420, 10, DARKGRAY);

                DrawText("(c) WWI Plane Model created by GiaHanLam", screenWidth - 240, screenHeight - 20, 10, DARKGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(model);   // Unload model data

            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

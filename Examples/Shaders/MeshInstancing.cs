/*******************************************************************************************
*
*   raylib [shaders] example - rlgl module usage for instanced meshes
*
*   This example uses [rlgl] module funtionality (pseudo-OpenGL 1.1 style coding)
*
*   This example has been created using raylib 3.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by @seanpringle and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2020 @seanpringle
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.ConfigFlags;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.ShaderLocationIndex;
using static Raylib_cs.ShaderUniformDataType;
using static Raylib_cs.MaterialMapIndex;
using static Raylib_cs.CameraMode;
using static Raylib_cs.KeyboardKey;

namespace Examples.Shaders
{
    public class MeshInstancing
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;
            const int fps = 60;

            SetConfigFlags(FLAG_MSAA_4X_HINT);  // Enable Multi Sampling Anti Aliasing 4x (if available)
            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - rlgl mesh instanced");

            int speed = 30;                 // Speed of jump animation
            int groups = 2;                 // Count of separate groups jumping around
            float amp = 10;                 // Maximum amplitude of jump
            float variance = 0.8f;          // Global variance in jump height
            float loop = 0.0f;              // Individual cube's computed loop timer

            // Used for various 3D coordinate & vector ops
            float x = 0.0f;
            float y = 0.0f;
            float z = 0.0f;

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(-125.0f, 125.0f, -125.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            // Number of instances to display
            const int instances = 10000;
            Mesh cube = GenMeshCube(1.0f, 1.0f, 1.0f);

            Matrix4x4[] rotations = new Matrix4x4[instances];    // Rotation state of instances
            Matrix4x4[] rotationsInc = new Matrix4x4[instances]; // Per-frame rotation animation of instances
            Matrix4x4[] translations = new Matrix4x4[instances]; // Locations of instances

            // Scatter random cubes around
            for (int i = 0; i < instances; i++)
            {
                x = GetRandomValue(-50, 50);
                y = GetRandomValue(-50, 50);
                z = GetRandomValue(-50, 50);
                translations[i] = Matrix4x4.CreateTranslation(x, y, z);

                x = GetRandomValue(0, 360);
                y = GetRandomValue(0, 360);
                z = GetRandomValue(0, 360);
                Vector3 axis = Vector3.Normalize(new Vector3(x, y, z));
                float angle = (float)GetRandomValue(0, 10) * DEG2RAD;

                rotationsInc[i] = Matrix4x4.CreateFromAxisAngle(axis, angle);
                rotations[i] = Matrix4x4.Identity;
            }

            Matrix4x4[] transforms = new Matrix4x4[instances];   // Pre-multiplied transformations passed to rlgl
            Shader shader = LoadShader("resources/shaders/glsl330/base_lighting_instanced.vs", "resources/shaders/glsl330/lighting.fs");

            // Get some shader loactions
            unsafe
            {
                int* locs = (int*)shader.locs;
                locs[(int)SHADER_LOC_MATRIX_MVP] = GetShaderLocation(shader, "mvp");
                locs[(int)SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");
                locs[(int)SHADER_LOC_MATRIX_MODEL] = GetShaderLocationAttrib(shader, "instanceTransform");
            }

            // Ambient light level
            int ambientLoc = GetShaderLocation(shader, "ambient");
            Raylib.SetShaderValue(shader, ambientLoc, new float[] { 0.2f, 0.2f, 0.2f, 1.0f }, SHADER_UNIFORM_VEC4);

            Rlights.CreateLight(0, LightType.LIGHT_DIRECTIONAL, new Vector3(50, 50, 0), Vector3.Zero, WHITE, shader);

            Material material = LoadMaterialDefault();
            material.shader = shader;
            unsafe
            {
                material.maps[(int)MATERIAL_MAP_DIFFUSE].color = RED;
            }

            SetCameraMode(camera, CAMERA_FREE); // Set a free camera mode

            int textPositionY = 300;

            // Simple frames counter to manage animation
            int framesCounter = 0;

            SetTargetFPS(fps);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);

                textPositionY = 300;
                framesCounter += 1;

                if (IsKeyDown(KEY_UP))
                    amp += 0.5f;
                if (IsKeyDown(KEY_DOWN))
                    amp = (amp <= 1) ? 1.0f : (amp - 1.0f);
                if (IsKeyDown(KEY_LEFT))
                    variance = (variance <= 0.0f) ? 0.0f : (variance - 0.01f);
                if (IsKeyDown(KEY_RIGHT))
                    variance = (variance >= 1.0f) ? 1.0f : (variance + 0.01f);
                if (IsKeyDown(KEY_ONE))
                    groups = 1;
                if (IsKeyDown(KEY_TWO))
                    groups = 2;
                if (IsKeyDown(KEY_THREE))
                    groups = 3;
                if (IsKeyDown(KEY_FOUR))
                    groups = 4;
                if (IsKeyDown(KEY_FIVE))
                    groups = 5;
                if (IsKeyDown(KEY_SIX))
                    groups = 6;
                if (IsKeyDown(KEY_SEVEN))
                    groups = 7;
                if (IsKeyDown(KEY_EIGHT))
                    groups = 8;
                if (IsKeyDown(KEY_NINE))
                    groups = 9;

                if (IsKeyDown(KEY_W))
                {
                    groups = 7;
                    amp = 25;
                    speed = 18;
                    variance = 0.70f;
                }

                if (IsKeyDown(KEY_EQUAL))
                    speed = (speed <= (int)(fps * 0.25f)) ? (int)(fps * 0.25f) : (int)(speed * 0.95f);
                if (IsKeyDown(KEY_KP_ADD))
                    speed = (speed <= (int)(fps * 0.25f)) ? (int)(fps * 0.25f) : (int)(speed * 0.95f);

                if (IsKeyDown(KEY_MINUS))
                    speed = (int)MathF.Max(speed * 1.02f, speed + 1);
                if (IsKeyDown(KEY_KP_SUBTRACT))
                    speed = (int)MathF.Max(speed * 1.02f, speed + 1);

                // Update the light shader with the camera view position
                float[] cameraPos = { camera.position.X, camera.position.Y, camera.position.Z };
                Raylib.SetShaderValue(shader, (int)SHADER_LOC_VECTOR_VIEW, cameraPos, SHADER_UNIFORM_VEC3);

                // Apply per-instance transformations
                for (int i = 0; i < instances; i++)
                {
                    rotations[i] = Matrix4x4.Multiply(rotations[i], rotationsInc[i]);
                    transforms[i] = Matrix4x4.Multiply(rotations[i], translations[i]);

                    // Get the animation cycle's framesCounter for this instance
                    loop = (float)((framesCounter + (int)(((float)(i % groups) / groups) * speed)) % speed) / speed;

                    // Calculate the y according to loop cycle
                    y = (MathF.Sin(loop * MathF.PI * 2)) * amp * ((1 - variance) + (variance * (float)(i % (groups * 10)) / (groups * 10)));

                    // Clamp to floor
                    y = (y < 0) ? 0.0f : y;

                    transforms[i] = Matrix4x4.Multiply(transforms[i], Matrix4x4.CreateTranslation(0.0f, y, 0.0f));
                    transforms[i] = Matrix4x4.Transpose(transforms[i]);
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);
                DrawMeshInstanced(cube, material, transforms, instances);
                EndMode3D();

                DrawText("A CUBE OF DANCING CUBES!", 490, 10, 20, MAROON);
                DrawText("PRESS KEYS:", 10, textPositionY, 20, BLACK);

                DrawText("1 - 9", 10, textPositionY += 25, 10, BLACK);
                DrawText(": Number of groups", 50, textPositionY, 10, BLACK);
                DrawText($": {groups}", 160, textPositionY, 10, BLACK);

                DrawText("UP", 10, textPositionY += 15, 10, BLACK);
                DrawText(": increase amplitude", 50, textPositionY, 10, BLACK);
                DrawText($": {amp}%.2f", 160, textPositionY, 10, BLACK);

                DrawText("DOWN", 10, textPositionY += 15, 10, BLACK);
                DrawText(": decrease amplitude", 50, textPositionY, 10, BLACK);

                DrawText("LEFT", 10, textPositionY += 15, 10, BLACK);
                DrawText(": decrease variance", 50, textPositionY, 10, BLACK);
                DrawText($": {variance}.2f", 160, textPositionY, 10, BLACK);

                DrawText("RIGHT", 10, textPositionY += 15, 10, BLACK);
                DrawText(": increase variance", 50, textPositionY, 10, BLACK);

                DrawText("+/=", 10, textPositionY += 15, 10, BLACK);
                DrawText(": increase speed", 50, textPositionY, 10, BLACK);
                DrawText($": {speed} = {((float)fps / speed)} loops/sec", 160, textPositionY, 10, BLACK);

                DrawText("-", 10, textPositionY += 15, 10, BLACK);
                DrawText(": decrease speed", 50, textPositionY, 10, BLACK);

                DrawText("W", 10, textPositionY += 15, 10, BLACK);
                DrawText(": Wild setup!", 50, textPositionY, 10, BLACK);

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

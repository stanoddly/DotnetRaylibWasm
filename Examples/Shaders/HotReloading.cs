/*******************************************************************************************
*
*   raylib [shaders] example - Hot reloading
*
*   NOTE: This example requires raylib OpenGL 3.3 for shaders support and only #version 330
*         is currently supported. OpenGL ES 2.0 platforms are not supported at the moment.
*
*   This example has been created using raylib 3.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2020 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;
using static Raylib_cs.ShaderUniformDataType;

namespace Examples.Shaders
{
    public class HotReloading
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            int screenWidth = 800;
            int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - hot reloading");

            string fragShaderFileName = "resources/shaders/glsl330/reload.fs";
            long fragShaderFileModTime = GetFileModTime(fragShaderFileName);

            // Load raymarching shader
            // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
            Shader shader = LoadShader(null, fragShaderFileName);

            // Get shader locations for required uniforms
            int resolutionLoc = GetShaderLocation(shader, "resolution");
            int mouseLoc = GetShaderLocation(shader, "mouse");
            int timeLoc = GetShaderLocation(shader, "time");

            float[] resolution = new[] { (float)screenWidth, (float)screenHeight };
            Raylib.SetShaderValue(shader, resolutionLoc, resolution, SHADER_UNIFORM_VEC2);

            float totalTime = 0.0f;
            bool shaderAutoReloading = false;

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())            // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                totalTime += GetFrameTime();
                Vector2 mouse = GetMousePosition();
                float[] mousePos = new[] { mouse.X, mouse.Y };

                // Set shader required uniform values
                Raylib.SetShaderValue(shader, timeLoc, totalTime, SHADER_UNIFORM_FLOAT);
                Raylib.SetShaderValue(shader, mouseLoc, mousePos, SHADER_UNIFORM_VEC2);

                // Hot shader reloading
                if (shaderAutoReloading || (IsMouseButtonPressed(MOUSE_LEFT_BUTTON)))
                {
                    long currentFragShaderModTime = GetFileModTime(fragShaderFileName);

                    // Check if shader file has been modified
                    if (currentFragShaderModTime != fragShaderFileModTime)
                    {
                        // Try reloading updated shader
                        Shader updatedShader = LoadShader(null, fragShaderFileName);

                        // It was correctly loaded
                        if (updatedShader.id != 0) //rlGetShaderIdDefault())
                        {
                            UnloadShader(shader);
                            shader = updatedShader;

                            // Get shader locations for required uniforms
                            resolutionLoc = GetShaderLocation(shader, "resolution");
                            mouseLoc = GetShaderLocation(shader, "mouse");
                            timeLoc = GetShaderLocation(shader, "time");

                            // Reset required uniforms
                            Raylib.SetShaderValue(shader, resolutionLoc, resolution, SHADER_UNIFORM_VEC2);
                        }

                        fragShaderFileModTime = currentFragShaderModTime;
                    }
                }

                if (IsKeyPressed(KEY_A))
                    shaderAutoReloading = !shaderAutoReloading;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // We only draw a white full-screen rectangle, frame is generated in shader
                BeginShaderMode(shader);
                DrawRectangle(0, 0, screenWidth, screenHeight, WHITE);
                EndShaderMode();

                string info = $"PRESS [A] to TOGGLE SHADER AUTOLOADING: {(shaderAutoReloading ? "AUTO" : "MANUAL")}";
                DrawText(info, 10, 10, 10, shaderAutoReloading ? RED : BLACK);
                if (!shaderAutoReloading)
                    DrawText("MOUSE CLICK to SHADER RE-LOADING", 10, 30, 10, BLACK);

                // DrawText($"Shader last modification: ", 10, 430, 10, BLACK);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadShader(shader);           // Unload shader

            CloseWindow();                  // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

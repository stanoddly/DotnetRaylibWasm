/*******************************************************************************************
*
*   raylib [textures] example - Texture drawing
*
*   This example illustrates how to draw on a blank texture using a shader
*
*   This example has been created using raylib 2.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Michał Ciesielski and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Michał Ciesielski and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.ShaderUniformDataType;

namespace Examples.Shaders
{
    public class TextureDrawing
    {
        const int GLSL_VERSION = 330;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - texture drawing");

            Image imBlank = GenImageColor(1024, 1024, BLANK);
            Texture2D texture = LoadTextureFromImage(imBlank);  // Load blank texture to fill on shader
            UnloadImage(imBlank);

            // NOTE: Using GLSL 330 shader version, on OpenGL ES 2.0 use GLSL 100 shader version
            Shader shader = LoadShader(null, string.Format("resources/shaders/glsl{0}/cubes_panning.fs", GLSL_VERSION));

            float time = 0.0f;
            int timeLoc = GetShaderLocation(shader, "uTime");
            Raylib.SetShaderValue(shader, timeLoc, time, SHADER_UNIFORM_FLOAT);

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            // -------------------------------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                time = (float)GetTime();
                Raylib.SetShaderValue(shader, timeLoc, time, SHADER_UNIFORM_FLOAT);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginShaderMode(shader);    // Enable our custom shader for next shapes/textures drawings
                DrawTexture(texture, 0, 0, WHITE);  // Drawing BLANK texture, all magic happens on shader
                EndShaderMode();            // Disable our custom shader, return to default shader

                DrawText("BACKGROUND is PAINTED and ANIMATED on SHADER!", 10, 10, 20, MAROON);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadShader(shader);

            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

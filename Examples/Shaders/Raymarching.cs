/*******************************************************************************************
*
*   raylib [shaders] example - Raymarching shapes generation
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version.
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3), to test this example
*         on OpenGL ES 2.0 platforms (Android, Raspberry Pi, HTML5), use #version 100 shaders
*         raylib comes with shaders ready for both versions, check raylib/shaders install folder
*
*   This example has been created using raylib 2.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2018 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.ConfigFlags;
using static Raylib_cs.CameraMode;
using static Raylib_cs.Color;
using static Raylib_cs.ShaderUniformDataType;

namespace Examples.Shaders
{
    public class Raymarching
    {
        public const int GLSL_VERSION = 330;
        // public const int GLSL_VERSION = 100;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            int screenWidth = 800;
            int screenHeight = 450;

            SetConfigFlags(FLAG_WINDOW_RESIZABLE);
            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - raymarching shapes");

            Camera3D camera = new Camera3D();
            camera.position = new Vector3(2.5f, 2.5f, 3.0f);    // Camera position
            camera.target = new Vector3(0.0f, 0.0f, 0.7f);      // Camera looking at point
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);          // Camera up vector (rotation towards target)
            camera.fovy = 65.0f;                                // Camera field-of-view Y

            SetCameraMode(camera, CAMERA_FREE);                 // Set camera mode

            // Load raymarching shader
            // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
            Shader shader = LoadShader(null, string.Format("resources/shaders/glsl{0}/raymarching.fs", GLSL_VERSION));

            // Get shader locations for required uniforms
            int viewEyeLoc = GetShaderLocation(shader, "viewEye");
            int viewCenterLoc = GetShaderLocation(shader, "viewCenter");
            int runTimeLoc = GetShaderLocation(shader, "runTime");
            int resolutionLoc = GetShaderLocation(shader, "resolution");

            float[] resolution = { (float)screenWidth, (float)screenHeight };
            Raylib.SetShaderValue(shader, resolutionLoc, resolution, SHADER_UNIFORM_VEC2);

            float runTime = 0.0f;

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())            // Detect window close button or ESC key
            {
                // Check if screen is resized
                //----------------------------------------------------------------------------------
                if (IsWindowResized())
                {
                    screenWidth = GetScreenWidth();
                    screenHeight = GetScreenHeight();
                    resolution = new float[] { (float)screenWidth, (float)screenHeight };
                    Raylib.SetShaderValue(shader, resolutionLoc, resolution, SHADER_UNIFORM_VEC2);
                }

                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);              // Update camera

                float deltaTime = GetFrameTime();
                runTime += deltaTime;

                // Set shader required uniform values
                Raylib.SetShaderValue(shader, viewEyeLoc, camera.position, SHADER_UNIFORM_VEC3);
                Raylib.SetShaderValue(shader, viewCenterLoc, camera.target, SHADER_UNIFORM_VEC3);
                Raylib.SetShaderValue(shader, runTimeLoc, runTime, SHADER_UNIFORM_FLOAT);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // We only draw a white full-screen rectangle,
                // frame is generated in shader using raymarching
                BeginShaderMode(shader);
                DrawRectangle(0, 0, screenWidth, screenHeight, WHITE);
                EndShaderMode();

                DrawText("(c) Raymarching shader by Iñigo Quilez. MIT License.", screenWidth - 280, screenHeight - 20, 10, BLACK);

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

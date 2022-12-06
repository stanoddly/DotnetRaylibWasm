/*******************************************************************************************
*
*   raylib [shaders] example - basic lighting
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version.
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3).
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Chris Camacho (@codifies) and reviewed by Ramon Santamaria (@raysan5)
*
*   Chris Camacho (@codifies -  http://bedroomcoders.co.uk/) notes:
*
*   This is based on the PBR lighting example, but greatly simplified to aid learning...
*   actually there is very little of the PBR example left!
*   When I first looked at the bewildering complexity of the PBR example I feared
*   I would never understand how I could do simple lighting with raylib however its
*   a testement to the authors of raylib (including rlights.h) that the example
*   came together fairly quickly.
*
*   Copyright (c) 2019 Chris Camacho (@codifies) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.ConfigFlags;
using static Raylib_cs.CameraMode;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.ShaderLocationIndex;
using static Examples.Rlights;

namespace Examples.Shaders
{
    public class BasicLighting
    {
        const int GLSL_VERSION = 330;

        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(FLAG_MSAA_4X_HINT);  // Enable Multi Sampling Anti Aliasing 4x (if available)
            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - basic lighting");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(2.0f, 4.0f, 6.0f);
            camera.target = new Vector3(0.0f, 0.5f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            // Load plane model from a generated mesh
            Model model = LoadModelFromMesh(GenMeshPlane(10.0f, 10.0f, 3, 3));
            Model cube = LoadModelFromMesh(GenMeshCube(2.0f, 4.0f, 2.0f));

            Shader shader = LoadShader("resources/shaders/glsl330/base_lighting.vs",
                                       "resources/shaders/glsl330/lighting.fs");

            // Get some required shader loactions
            shader.locs[(int)SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");

            // ambient light level
            int ambientLoc = GetShaderLocation(shader, "ambient");
            float[] ambient = new[] { 0.1f, 0.1f, 0.1f, 1.0f };
            Raylib.SetShaderValue(shader, ambientLoc, ambient, ShaderUniformDataType.SHADER_UNIFORM_VEC4);

            // Assign out lighting shader to model
            model.materials[0].shader = shader;
            cube.materials[0].shader = shader;

            // Using 4 point lights: gold, red, green and blue
            Light[] lights = new Light[4];
            lights[0] = CreateLight(0, LightType.LIGHT_POINT, new Vector3(-2, 1, -2), Vector3.Zero, YELLOW, shader);
            lights[1] = CreateLight(1, LightType.LIGHT_POINT, new Vector3(2, 1, 2), Vector3.Zero, RED, shader);
            lights[2] = CreateLight(2, LightType.LIGHT_POINT, new Vector3(-2, 1, 2), Vector3.Zero, GREEN, shader);
            lights[3] = CreateLight(3, LightType.LIGHT_POINT, new Vector3(2, 1, -2), Vector3.Zero, BLUE, shader);

            SetCameraMode(camera, CAMERA_ORBITAL);  // Set an orbital camera mode

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())            // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);              // Update camera

                if (IsKeyPressed(KEY_Y))
                {
                    lights[0].enabled = !lights[0].enabled;
                }
                if (IsKeyPressed(KEY_R))
                {
                    lights[1].enabled = !lights[1].enabled;
                }
                if (IsKeyPressed(KEY_G))
                {
                    lights[2].enabled = !lights[2].enabled;
                }
                if (IsKeyPressed(KEY_B))
                {
                    lights[3].enabled = !lights[3].enabled;
                }

                // Update light values (actually, only enable/disable them)
                UpdateLightValues(shader, lights[0]);
                UpdateLightValues(shader, lights[1]);
                UpdateLightValues(shader, lights[2]);
                UpdateLightValues(shader, lights[3]);

                // Update the light shader with the camera view position
                Raylib.SetShaderValue(shader, shader.locs[(int)SHADER_LOC_VECTOR_VIEW], camera.position, ShaderUniformDataType.SHADER_UNIFORM_VEC3);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                DrawModel(model, Vector3Zero(), 1.0f, WHITE);
                DrawModel(cube, Vector3Zero(), 1.0f, WHITE);

                // Draw markers to show where the lights are
                if (lights[0].enabled)
                {
                    DrawSphereEx(lights[0].position, 0.2f, 8, 8, YELLOW);
                }
                else
                {
                    DrawSphereWires(lights[0].position, 0.2f, 8, 8, ColorAlpha(YELLOW, 0.3f));
                }

                if (lights[1].enabled)
                {
                    DrawSphereEx(lights[1].position, 0.2f, 8, 8, RED);
                }
                else
                {
                    DrawSphereWires(lights[1].position, 0.2f, 8, 8, ColorAlpha(RED, 0.3f));
                }

                if (lights[2].enabled)
                {
                    DrawSphereEx(lights[2].position, 0.2f, 8, 8, GREEN);
                }
                else
                {
                    DrawSphereWires(lights[2].position, 0.2f, 8, 8, ColorAlpha(GREEN, 0.3f));
                }

                if (lights[3].enabled)
                {
                    DrawSphereEx(lights[3].position, 0.2f, 8, 8, BLUE);
                }
                else
                {
                    DrawSphereWires(lights[3].position, 0.2f, 8, 8, ColorAlpha(BLUE, 0.3f));
                }

                DrawGrid(10, 1.0f);

                EndMode3D();

                DrawFPS(10, 10);
                DrawText("Use keys [Y][R][G][B] to toggle lights", 10, 40, 20, DARKGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(model);     // Unload the model
            UnloadModel(cube);      // Unload the model
            UnloadShader(shader);   // Unload shader

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

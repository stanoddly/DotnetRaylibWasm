/*******************************************************************************************
*
*   raylib [shaders] example - fog
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
using static Raylib_cs.MaterialMapIndex;
using static Raylib_cs.ShaderLocationIndex;
using static Raylib_cs.ShaderUniformDataType;
using static Examples.Rlights;

namespace Examples.Shaders
{
    public class Fog
    {
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(FLAG_MSAA_4X_HINT);  // Enable Multi Sampling Anti Aliasing 4x (if available)
            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - fog");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D(
                new Vector3(2.0f, 2.0f, 6.0f),      // position
                new Vector3(0.0f, 0.5f, 0.0f),      // target
                new Vector3(0.0f, 1.0f, 0.0f),      // up
                45.0f, CAMERA_PERSPECTIVE);         // fov, type

            // Load models and texture
            Model modelA = LoadModelFromMesh(GenMeshTorus(0.4f, 1.0f, 16, 32));
            Model modelB = LoadModelFromMesh(GenMeshCube(1.0f, 1.0f, 1.0f));
            Model modelC = LoadModelFromMesh(GenMeshSphere(0.5f, 32, 32));
            Texture2D texture = LoadTexture("resources/texel_checker.png");

            // Assign texture to default model material
            Raylib.SetMaterialTexture(ref modelA, 0, MATERIAL_MAP_ALBEDO, ref texture);
            Raylib.SetMaterialTexture(ref modelB, 0, MATERIAL_MAP_ALBEDO, ref texture);
            Raylib.SetMaterialTexture(ref modelC, 0, MATERIAL_MAP_ALBEDO, ref texture);

            // Load shader and set up some uniforms
            Shader shader = LoadShader("resources/shaders/glsl330/base_lighting.vs", "resources/shaders/glsl330/fog.fs");
            shader.locs[(int)SHADER_LOC_MATRIX_MODEL] = GetShaderLocation(shader, "matModel");
            shader.locs[(int)SHADER_LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");

            // Ambient light level
            int ambientLoc = GetShaderLocation(shader, "ambient");
            Raylib.SetShaderValue(shader, ambientLoc, new float[] { 0.2f, 0.2f, 0.2f, 1.0f }, SHADER_UNIFORM_VEC4);

            float fogDensity = 0.15f;
            int fogDensityLoc = GetShaderLocation(shader, "fogDensity");
            Raylib.SetShaderValue(shader, fogDensityLoc, fogDensity, SHADER_UNIFORM_FLOAT);

            // NOTE: All models share the same shader
            Raylib.SetMaterialShader(ref modelA, 0, ref shader);
            Raylib.SetMaterialShader(ref modelB, 0, ref shader);
            Raylib.SetMaterialShader(ref modelC, 0, ref shader);

            // Using just 1 point lights
            CreateLight(0, LightType.LIGHT_POINT, new Vector3(0, 2, 6), Vector3.Zero, WHITE, shader);

            SetCameraMode(camera, CAMERA_ORBITAL);  // Set an orbital camera mode

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())            // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);              // Update camera

                if (IsKeyDown(KEY_UP))
                {
                    fogDensity += 0.001f;
                    if (fogDensity > 1.0)
                        fogDensity = 1.0f;
                }

                if (IsKeyDown(KEY_DOWN))
                {
                    fogDensity -= 0.001f;
                    if (fogDensity < 0.0)
                        fogDensity = 0.0f;
                }

                Raylib.SetShaderValue(shader, fogDensityLoc, fogDensity, SHADER_UNIFORM_FLOAT);

                // Rotate the torus
                modelA.transform = MatrixMultiply(modelA.transform, MatrixRotateX(-0.025f));
                modelA.transform = MatrixMultiply(modelA.transform, MatrixRotateZ(0.012f));

                // Update the light shader with the camera view position
                Raylib.SetShaderValue(shader, shader.locs[(int)SHADER_LOC_VECTOR_VIEW], camera.position.X, ShaderUniformDataType.SHADER_UNIFORM_VEC3);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(GRAY);

                BeginMode3D(camera);

                // Draw the three models
                DrawModel(modelA, Vector3.Zero, 1.0f, WHITE);
                DrawModel(modelB, new Vector3(-2.6f, 0, 0), 1.0f, WHITE);
                DrawModel(modelC, new Vector3(2.6f, 0, 0), 1.0f, WHITE);

                for (int i = -20; i < 20; i += 2)
                    DrawModel(modelA, new Vector3(i, 0, 2), 1.0f, WHITE);

                EndMode3D();

                DrawText(string.Format("Use KEY_UP/KEY_DOWN to change fog density [{0}]", fogDensity), 10, 10, 20, RAYWHITE);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(modelA);        // Unload the model A
            UnloadModel(modelB);        // Unload the model B
            UnloadModel(modelC);        // Unload the model C
            UnloadTexture(texture);     // Unload the texture
            UnloadShader(shader);       // Unload shader

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

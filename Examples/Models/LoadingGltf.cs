/*******************************************************************************************
*
*   raylib [models] example - Load 3d gltf model
*
*   This example has been created using raylib 3.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Hristo Stamenov (@object71) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2021 Hristo Stamenov (@object71) and Ramon Santamaria (@raysan5)
*
********************************************************************************************
*
* To export a model from blender, make sure it is not posed, the vertices need to be in the
* same position as they would be in edit mode.
* and that the scale of your models is set to 0. Scaling can be done from the export menu.
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.CameraMode;

namespace Examples.Models
{
    public class LoadingGltf
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - model animation");

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(10.0f, 10.0f, 10.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            const int modelCount = 6;
            Model[] model = new Model[modelCount];

            model[0] = LoadModel("resources/models/gltf/raylib_32x32.glb");
            model[1] = LoadModel("resources/models/gltf/rigged_figure.glb");
            model[2] = LoadModel("resources/models/gltf/GearboxAssy.glb");
            model[3] = LoadModel("resources/models/gltf/BoxAnimated.glb");
            model[4] = LoadModel("resources/models/gltf/AnimatedTriangle.gltf");
            model[5] = LoadModel("resources/models/gltf/AnimatedMorphCube.glb");

            int currentModel = 0;

            // Set model position
            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);

            SetCameraMode(camera, CAMERA_FREE);

            SetTargetFPS(30);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);

                if (IsKeyReleased(KEY_RIGHT))
                {
                    currentModel += 1;
                    if (currentModel == modelCount)
                        currentModel = 0;
                }

                if (IsKeyReleased(KEY_LEFT))
                {
                    currentModel -= 1;
                    if (currentModel < 0)
                        currentModel = modelCount - 1;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(SKYBLUE);

                BeginMode3D(camera);

                DrawModelEx(model[currentModel], position, new Vector3(0.0f, 1.0f, 0.0f), 180.0f, new Vector3(2.0f, 2.0f, 2.0f), WHITE);
                DrawGrid(10, 1.0f);

                EndMode3D();

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------

            // Unload model animations data
            for (int i = 0; i < modelCount; i++)
            {
                UnloadModel(model[i]);  // Unload models
            }

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

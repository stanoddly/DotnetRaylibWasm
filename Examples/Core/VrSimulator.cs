/*******************************************************************************************
*
*   raylib [core] example - VR Simulator (Oculus Rift CV1 parameters)
*
*   This example has been created using raylib 1.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.CameraMode;
using static Raylib_cs.Color;
using static Raylib_cs.ShaderUniformDataType;

namespace Examples.Core
{
    public class VrSimulator
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 1080;
            const int screenHeight = 600;

            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(screenWidth, screenHeight, "raylib [core] example - vr simulator");

            // VR device parameters definition
            VrDeviceInfo device = new VrDeviceInfo
            {
                // Oculus Rift CV1 parameters for simulator
                hResolution = 2160,                 // Horizontal resolution in pixels
                vResolution = 1200,                 // Vertical resolution in pixels
                hScreenSize = 0.133793f,            // Horizontal size in meters
                vScreenSize = 0.0669f,              // Vertical size in meters
                vScreenCenter = 0.04678f,           // Screen center in meters
                eyeToScreenDistance = 0.041f,       // Distance between eye and display in meters
                lensSeparationDistance = 0.07f,     // Lens separation distance in meters
                interpupillaryDistance = 0.07f,     // IPD (distance between pupils) in meters
            };

            // NOTE: CV1 uses a Fresnel-hybrid-asymmetric lenses with specific distortion compute shaders.
            // Following parameters are an approximation to distortion stereo rendering but results differ from actual device.
            unsafe
            {
                device.lensDistortionValues[0] = 1.0f;     // Lens distortion constant parameter 0
                device.lensDistortionValues[1] = 0.22f;    // Lens distortion constant parameter 1
                device.lensDistortionValues[2] = 0.24f;    // Lens distortion constant parameter 2
                device.lensDistortionValues[3] = 0.0f;     // Lens distortion constant parameter 3
                device.chromaAbCorrection[0] = 0.996f;     // Chromatic aberration correction parameter 0
                device.chromaAbCorrection[1] = -0.004f;    // Chromatic aberration correction parameter 1
                device.chromaAbCorrection[2] = 1.014f;     // Chromatic aberration correction parameter 2
                device.chromaAbCorrection[3] = 0.0f;       // Chromatic aberration correction parameter 3
            }

            // Load VR stereo config for VR device parameteres (Oculus Rift CV1 parameters)
            VrStereoConfig config = LoadVrStereoConfig(device);

            // Distortion shader (uses device lens distortion and chroma)
            Shader distortion = LoadShader(null, "resources/distortion330.fs");

            // Update distortion shader with lens and distortion-scale parameters
            Raylib.SetShaderValue(distortion, GetShaderLocation(distortion, "leftLensCenter"),
                        config.leftLensCenter, SHADER_UNIFORM_VEC2);
            Raylib.SetShaderValue(distortion, GetShaderLocation(distortion, "rightLensCenter"),
                        config.rightLensCenter, SHADER_UNIFORM_VEC2);
            Raylib.SetShaderValue(distortion, GetShaderLocation(distortion, "leftScreenCenter"),
                        config.leftScreenCenter, SHADER_UNIFORM_VEC2);
            Raylib.SetShaderValue(distortion, GetShaderLocation(distortion, "rightScreenCenter"),
                        config.rightScreenCenter, SHADER_UNIFORM_VEC2);

            Raylib.SetShaderValue(distortion, GetShaderLocation(distortion, "scale"),
                        config.scale, SHADER_UNIFORM_VEC2);
            Raylib.SetShaderValue(distortion, GetShaderLocation(distortion, "scaleIn"),
                        config.scaleIn, SHADER_UNIFORM_VEC2);

            unsafe
            {
                SetShaderValue(distortion, GetShaderLocation(distortion, "deviceWarpParam"),
                            device.lensDistortionValues, SHADER_UNIFORM_VEC4);
                SetShaderValue(distortion, GetShaderLocation(distortion, "chromaAbParam"),
                            device.chromaAbCorrection, SHADER_UNIFORM_VEC4);
            }

            // Initialize framebuffer for stereo rendering
            // NOTE: Screen size should match HMD aspect ratio
            RenderTexture2D target = LoadRenderTexture(GetScreenWidth(), GetScreenHeight());

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(5.0f, 2.0f, 5.0f);    // Camera position
            camera.target = new Vector3(0.0f, 2.0f, 0.0f);      // Camera looking at point
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);          // Camera up vector (rotation towards target)
            camera.fovy = 60.0f;                                // Camera field-of-view Y
            camera.projection = CAMERA_PERSPECTIVE;             // Camera type

            Vector3 cubePosition = new Vector3(0.0f, 0.0f, 0.0f);

            SetCameraMode(camera, CAMERA_FIRST_PERSON);         // Set first person camera mode

            SetTargetFPS(90);                   // Set our game to run at 90 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);          // Update camera (simulator mode)
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginTextureMode(target);
                ClearBackground(RAYWHITE);

                BeginVrStereoMode(config);
                BeginMode3D(camera);

                DrawCube(cubePosition, 2.0f, 2.0f, 2.0f, RED);
                DrawCubeWires(cubePosition, 2.0f, 2.0f, 2.0f, MAROON);
                DrawGrid(40, 1.0f);

                EndMode3D();
                EndVrStereoMode();
                EndTextureMode();

                BeginShaderMode(distortion);
                DrawTextureRec(target.texture, new Rectangle(0, 0, (float)target.texture.width,
                              (float)-target.texture.height), new Vector2(0.0f, 0.0f), WHITE);
                EndShaderMode();

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadVrStereoConfig(config);   // Unload stereo config

            UnloadRenderTexture(target);    // Unload stereo render fbo
            UnloadShader(distortion);       // Unload distortion shader

            CloseWindow();          // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

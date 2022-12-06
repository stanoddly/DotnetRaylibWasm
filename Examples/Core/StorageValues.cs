/*******************************************************************************************
*
*   raylib [core] example - Storage save/load values
*
*   This example has been created using raylib 1.4 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using System.Runtime.InteropServices;

namespace Examples.Core
{
    public class StorageValues
    {
        // NOTE: Storage positions must start with 0, directly related to file memory layout
        enum StorageData
        {
            STORAGE_SCORE = 0,
            STORAGE_HISCORE
        }

        // Load integer value from storage file (from defined position), returns true on success
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool SaveStorageValue(uint position, int value);

        // Load integer value from storage file (from defined position)
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int LoadStorageValue(uint position);

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - storage save/load values");

            int score = 0;
            int hiscore = 0;
            int framesCounter = 0;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyPressed(KEY_R))
                {
                    score = GetRandomValue(1000, 2000);
                    hiscore = GetRandomValue(2000, 4000);
                }

                if (IsKeyPressed(KEY_ENTER))
                {
                    SaveStorageValue((int)StorageData.STORAGE_SCORE, score);
                    SaveStorageValue((int)StorageData.STORAGE_HISCORE, hiscore);
                }
                else if (IsKeyPressed(KEY_SPACE))
                {
                    // NOTE: If requested position could not be found, value 0 is returned
                    score = LoadStorageValue((int)StorageData.STORAGE_SCORE);
                    hiscore = LoadStorageValue((int)StorageData.STORAGE_HISCORE);
                }

                framesCounter++;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawText($"SCORE: {score}", 280, 130, 40, MAROON);
                DrawText($"HI-SCORE: {hiscore}", 210, 200, 50, BLACK);

                DrawText($"frames: {framesCounter}", 10, 10, 20, LIME);

                DrawText("Press R to generate random numbers", 220, 40, 20, LIGHTGRAY);
                DrawText("Press ENTER to SAVE values", 250, 310, 20, LIGHTGRAY);
                DrawText("Press SPACE to LOAD values", 252, 350, 20, LIGHTGRAY);

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

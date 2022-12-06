/*******************************************************************************************
*
*   raylib [core] example - Gamepad input
*
*   NOTE: This example requires a Gamepad connected to the system
*         raylib is configured to work with the following gamepads:
*                - Xbox 360 Controller (Xbox 360, Xbox One)
*                - PLAYSTATION(R)3 Controller
*         Check raylib.h for buttons configuration
*
*   This example has been created using raylib 1.6 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2013-2016 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.GamepadAxis;
using static Raylib_cs.GamepadButton;
using static Raylib_cs.Color;

namespace Examples.Core
{
    public class InputGamepad
    {
        // NOTE: Gamepad name ID depends on drivers and OS
        // public const string XBOX360_NAME_ID = "Microsoft;
        // public const string PS3_NAME_ID = "PLAYSTATION(R)3;
        public const string XBOX360_NAME_ID = "Xbox";
        public const string PS3_NAME_ID = "PLAYSTATION(R)3";

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            // Set MSAA 4X hint before windows creation
            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(screenWidth, screenHeight, "raylib [core] example - gamepad input");

            Texture2D texPs3Pad = LoadTexture("resources/ps3.png");
            Texture2D texXboxPad = LoadTexture("resources/xbox.png");

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // ...
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                if (IsGamepadAvailable(0))
                {
                    string gamepadName = GetGamepadName_(0);
                    DrawText($"GP1: {gamepadName}", 10, 10, 10, BLACK);

                    if (gamepadName == XBOX360_NAME_ID)
                    {
                        DrawTexture(texXboxPad, 0, 0, DARKGRAY);

                        // Draw buttons: xbox home
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_MIDDLE))
                            DrawCircle(394, 89, 19, RED);

                        // Draw buttons: basic
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_MIDDLE_RIGHT))
                            DrawCircle(436, 150, 9, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_MIDDLE_LEFT))
                            DrawCircle(352, 150, 9, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_FACE_LEFT))
                            DrawCircle(501, 151, 15, BLUE);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_FACE_DOWN))
                            DrawCircle(536, 187, 15, LIME);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_FACE_RIGHT))
                            DrawCircle(572, 151, 15, MAROON);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_FACE_UP))
                            DrawCircle(536, 115, 15, GOLD);

                        // Draw buttons: d-pad
                        DrawRectangle(317, 202, 19, 71, BLACK);
                        DrawRectangle(293, 228, 69, 19, BLACK);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_FACE_UP))
                            DrawRectangle(317, 202, 19, 26, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_FACE_DOWN))
                            DrawRectangle(317, 202 + 45, 19, 26, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_FACE_LEFT))
                            DrawRectangle(292, 228, 25, 19, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_FACE_RIGHT))
                            DrawRectangle(292 + 44, 228, 26, 19, RED);

                        // Draw buttons: left-right back
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_TRIGGER_1))
                            DrawCircle(259, 61, 20, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_TRIGGER_1))
                            DrawCircle(536, 61, 20, RED);

                        // Draw axis: left joystick
                        DrawCircle(259, 152, 39, BLACK);
                        DrawCircle(259, 152, 34, LIGHTGRAY);
                        DrawCircle(259 + (int)(GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_X) * 20),
                                   152 - (int)(GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_Y) * 20), 25, BLACK);

                        // Draw axis: right joystick
                        DrawCircle(461, 237, 38, BLACK);
                        DrawCircle(461, 237, 33, LIGHTGRAY);
                        DrawCircle(461 + (int)(GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_X) * 20),
                                   237 - (int)(GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_Y) * 20), 25, BLACK);

                        // Draw axis: left-right triggers
                        DrawRectangle(170, 30, 15, 70, GRAY);
                        DrawRectangle(604, 30, 15, 70, GRAY);
                        DrawRectangle(170, 30, 15, (int)(((1.0f + GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_TRIGGER)) / 2.0f) * 70), RED);
                        DrawRectangle(604, 30, 15, (int)(((1.0f + GetGamepadAxisMovement(0, GAMEPAD_AXIS_RIGHT_TRIGGER)) / 2.0f) * 70), RED);
                    }
                    else if (gamepadName == PS3_NAME_ID)
                    {
                        DrawTexture(texPs3Pad, 0, 0, DARKGRAY);

                        // Draw buttons: ps
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_MIDDLE))
                            DrawCircle(396, 222, 13, RED);

                        // Draw buttons: basic
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_MIDDLE_LEFT))
                            DrawRectangle(328, 170, 32, 13, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_MIDDLE_RIGHT))
                            DrawTriangle(new Vector2(436, 168), new Vector2(436, 185), new Vector2(
                        464, 177), RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_FACE_UP))
                            DrawCircle(557, 144, 13, LIME);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_FACE_RIGHT))
                            DrawCircle(586, 173, 13, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_FACE_DOWN))
                            DrawCircle(557, 203, 13, VIOLET);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_FACE_LEFT))
                            DrawCircle(527, 173, 13, PINK);

                        // Draw buttons: d-pad
                        DrawRectangle(225, 132, 24, 84, BLACK);
                        DrawRectangle(195, 161, 84, 25, BLACK);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_FACE_UP))
                            DrawRectangle(225, 132, 24, 29, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_FACE_DOWN))
                            DrawRectangle(225, 132 + 54, 24, 30, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_FACE_LEFT))
                            DrawRectangle(195, 161, 30, 25, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_FACE_RIGHT))
                            DrawRectangle(195 + 54, 161, 30, 25, RED);

                        // Draw buttons: left-right back buttons
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_TRIGGER_1))
                            DrawCircle(239, 82, 20, RED);
                        if (IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_TRIGGER_1))
                            DrawCircle(557, 82, 20, RED);

                        // Draw axis: left joystick
                        DrawCircle(319, 255, 35, BLACK);
                        DrawCircle(319, 255, 31, LIGHTGRAY);
                        DrawCircle(319 + (int)(GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_X) * 20),
                                   255 + (int)(GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_Y) * 20), 25, BLACK);

                        // Draw axis: right joystick
                        DrawCircle(475, 255, 35, BLACK);
                        DrawCircle(475, 255, 31, LIGHTGRAY);
                        DrawCircle(475 + (int)(GetGamepadAxisMovement(0, GAMEPAD_AXIS_RIGHT_X) * 20),
                                   255 + (int)(GetGamepadAxisMovement(0, GAMEPAD_AXIS_RIGHT_Y) * 20), 25, BLACK);

                        // Draw axis: left-right triggers
                        DrawRectangle(169, 48, 15, 70, GRAY);
                        DrawRectangle(611, 48, 15, 70, GRAY);
                        DrawRectangle(169, 48, 15, (int)(((1.0f - GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_TRIGGER)) / 2.0f) * 70), RED);
                        DrawRectangle(611, 48, 15, (int)(((1.0f - GetGamepadAxisMovement(0, GAMEPAD_AXIS_RIGHT_TRIGGER)) / 2.0f) * 70), RED);
                    }
                    else
                    {
                        DrawText("- GENERIC GAMEPAD -", 280, 180, 20, GRAY);
                        // TODO: Draw generic gamepad
                    }

                    DrawText($"DETECTED AXIS [{GetGamepadAxisCount(0)}]:", 10, 50, 10, MAROON);

                    for (int i = 0; i < GetGamepadAxisCount(0); i++)
                    {
                        DrawText($"AXIS {i}: {GetGamepadAxisMovement(0, (GamepadAxis)i)}", 20, 70 + 20 * i, 10, DARKGRAY);
                    }

                    if (GetGamepadButtonPressed() != -1)
                    {
                        DrawText($"DETECTED BUTTON: {GetGamepadButtonPressed()}", 10, 430, 10, RED);
                    }
                    else
                    {
                        DrawText("DETECTED BUTTON: NONE", 10, 430, 10, GRAY);
                    }
                }
                else
                {
                    DrawText("GP1: NOT DETECTED", 10, 10, 10, GRAY);
                    DrawTexture(texXboxPad, 0, 0, LIGHTGRAY);
                }

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texPs3Pad);
            UnloadTexture(texXboxPad);

            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

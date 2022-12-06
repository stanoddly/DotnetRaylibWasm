/*******************************************************************************************
*
*   raylib [easings] example - Easings Testbed
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Juan Miguel López (@flashback-fx) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Juan Miguel López (@flashback-fx ) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class easings_testbed
    {
        const int FONT_SIZE = 20;

        const float D_STEP = 20.0f;
        const float D_STEP_FINE = 2.0f;
        const float D_MIN = 1.0f;
        const float D_MAX = 10000.0f;

        // Easing types
        enum EasingTypes
        {
            EASE_LINEAR_NONE = 0,
            EASE_LINEAR_IN,
            EASE_LINEAR_OUT,
            EASE_LINEAR_IN_OUT,
            EASE_SINE_IN,
            EASE_SINE_OUT,
            EASE_SINE_IN_OUT,
            EASE_CIRC_IN,
            EASE_CIRC_OUT,
            EASE_CIRC_IN_OUT,
            EASE_CUBIC_IN,
            EASE_CUBIC_OUT,
            EASE_CUBIC_IN_OUT,
            EASE_QUAD_IN,
            EASE_QUAD_OUT,
            EASE_QUAD_IN_OUT,
            EASE_EXPO_IN,
            EASE_EXPO_OUT,
            EASE_EXPO_IN_OUT,
            EASE_BACK_IN,
            EASE_BACK_OUT,
            EASE_BACK_IN_OUT,
            EASE_BOUNCE_OUT,
            EASE_BOUNCE_IN,
            EASE_BOUNCE_IN_OUT,
            EASE_ELASTIC_IN,
            EASE_ELASTIC_OUT,
            EASE_ELASTIC_IN_OUT,
            NUM_EASING_TYPES,
            EASING_NONE = NUM_EASING_TYPES
        };

        public delegate float EaseFunc(float a, float b, float c, float d);

        // Easing functions reference data
        public struct EasingFunc
        {
            public string name;
            public EaseFunc func;
        }

        static EasingFunc[] easings = new EasingFunc[] {
            new EasingFunc { name = "EaseLinearNone", func = Easings.EaseLinearNone },
            new EasingFunc { name = "EaseLinearIn", func = Easings.EaseLinearIn },
            new EasingFunc { name = "EaseLinearOut", func = Easings.EaseLinearOut },
            new EasingFunc { name = "EaseLinearInOut", func = Easings.EaseLinearInOut },
            new EasingFunc { name = "EaseSineIn", func = Easings.EaseSineIn },
            new EasingFunc { name = "EaseSineOut", func = Easings.EaseSineOut },
            new EasingFunc { name = "EaseSineInOut", func = Easings.EaseSineInOut },
            new EasingFunc { name = "EaseCircIn", func = Easings.EaseCircIn },
            new EasingFunc { name = "EaseCircOut", func = Easings.EaseCircOut },
            new EasingFunc { name = "EaseCircInOut", func = Easings.EaseCircInOut },
            new EasingFunc { name = "EaseCubicIn", func = Easings.EaseCubicIn },
            new EasingFunc { name = "EaseCubicOut", func = Easings.EaseCubicOut },
            new EasingFunc { name = "EaseCubicInOut", func = Easings.EaseCubicInOut },
            new EasingFunc { name = "EaseQuadIn", func = Easings.EaseQuadIn },
            new EasingFunc { name = "EaseQuadOut", func = Easings.EaseQuadOut },
            new EasingFunc { name = "EaseQuadInOut", func = Easings.EaseQuadInOut },
            new EasingFunc { name = "EaseExpoIn", func = Easings.EaseExpoIn },
            new EasingFunc { name = "EaseExpoOut", func = Easings.EaseExpoOut },
            new EasingFunc { name = "EaseExpoInOut", func = Easings.EaseExpoInOut },
            new EasingFunc { name = "EaseBackIn", func = Easings.EaseBackIn },
            new EasingFunc { name = "EaseBackOut", func = Easings.EaseBackOut },
            new EasingFunc { name = "EaseBackInOut", func = Easings.EaseBackInOut },
            new EasingFunc { name = "EaseBounceOut", func = Easings.EaseBounceOut },
            new EasingFunc { name = "EaseBounceIn", func = Easings.EaseBounceIn },
            new EasingFunc { name = "EaseBounceInOut", func = Easings.EaseBounceInOut },
            new EasingFunc { name = "EaseElasticIn", func = Easings.EaseElasticIn },
            new EasingFunc { name = "EaseElasticOut", func = Easings.EaseElasticOut },
            new EasingFunc { name = "EaseElasticInOut", func = Easings.EaseElasticInOut },
            new EasingFunc { name = "None", func = NoEase },
        };

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [easings] example - easings testbed");

            Vector2 ballPosition = new Vector2(100.0f, 200.0f);

            float t = 0.0f;             // Current time (in any unit measure, but same unit as duration)
            float d = 300.0f;           // Total time it should take to complete (duration)
            bool paused = true;
            bool boundedT = true;       // If true, t will stop when d >= td, otherwise t will keep adding td to its value every loop

            EasingTypes easingX = EasingTypes.EASING_NONE;  // Easing selected for x axis
            EasingTypes easingY = EasingTypes.EASING_NONE;  // Easing selected for y axis

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyPressed(KEY_T))
                    boundedT = !boundedT;

                // Choose easing for the X axis
                if (IsKeyPressed(KEY_RIGHT))
                {
                    easingX++;

                    if (easingX > EasingTypes.EASING_NONE)
                        easingX = 0;
                }
                else if (IsKeyPressed(KEY_LEFT))
                {
                    if (easingX == 0)
                        easingX = EasingTypes.EASING_NONE;
                    else
                        easingX--;
                }

                // Choose easing for the Y axis
                if (IsKeyPressed(KEY_DOWN))
                {
                    easingY++;

                    if (easingY > EasingTypes.EASING_NONE)
                        easingY = 0;
                }
                else if (IsKeyPressed(KEY_UP))
                {
                    if (easingY == 0)
                        easingY = EasingTypes.EASING_NONE;
                    else
                        easingY--;
                }

                // Change d (duration) value
                if (IsKeyPressed(KEY_W) && d < D_MAX - D_STEP)
                    d += D_STEP;
                else if (IsKeyPressed(KEY_Q) && d > D_MIN + D_STEP)
                    d -= D_STEP;

                if (IsKeyDown(KEY_S) && d < D_MAX - D_STEP_FINE)
                    d += D_STEP_FINE;
                else if (IsKeyDown(KEY_A) && d > D_MIN + D_STEP_FINE)
                    d -= D_STEP_FINE;

                // Play, pause and restart controls
                if (IsKeyPressed(KEY_SPACE) || IsKeyPressed(KEY_T) ||
                    IsKeyPressed(KEY_RIGHT) || IsKeyPressed(KEY_LEFT) ||
                    IsKeyPressed(KEY_DOWN) || IsKeyPressed(KEY_UP) ||
                    IsKeyPressed(KEY_W) || IsKeyPressed(KEY_Q) ||
                    IsKeyDown(KEY_S) || IsKeyDown(KEY_A) ||
                    (IsKeyPressed(KEY_ENTER) && (boundedT == true) && (t >= d)))
                {
                    t = 0.0f;
                    ballPosition.X = 100.0f;
                    ballPosition.Y = 100.0f;
                    paused = true;
                }

                if (IsKeyPressed(KEY_ENTER))
                    paused = !paused;

                // Movement computation
                if (!paused && ((boundedT && t < d) || !boundedT))
                {
                    ballPosition.X = easings[(int)easingX].func(t, 100.0f, 700.0f - 100.0f, d);
                    ballPosition.Y = easings[(int)easingY].func(t, 100.0f, 400.0f - 100.0f, d);
                    t += 1.0f;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // Draw information text
                DrawText(string.Format("Easing x: {0}", easings[(int)easingX].name), 0, FONT_SIZE * 2, FONT_SIZE, LIGHTGRAY);
                DrawText(string.Format("Easing y: {0}", easings[(int)easingY].name), 0, FONT_SIZE * 3, FONT_SIZE, LIGHTGRAY);
                DrawText(string.Format("t ({0}) = {1:0.##} d = {2:0.##}", (boundedT == true) ? 'b' : 'u', t, d), 0, FONT_SIZE * 4, FONT_SIZE, LIGHTGRAY);

                // Draw instructions text
                DrawText("Use ENTER to play or pause movement, use SPACE to restart", 0, GetScreenHeight() - FONT_SIZE * 2, FONT_SIZE, LIGHTGRAY);
                DrawText("Use D and W or A and S keys to change duration", 0, GetScreenHeight() - FONT_SIZE * 3, FONT_SIZE, LIGHTGRAY);
                DrawText("Use LEFT or RIGHT keys to choose easing for the x axis", 0, GetScreenHeight() - FONT_SIZE * 4, FONT_SIZE, LIGHTGRAY);
                DrawText("Use UP or DOWN keys to choose easing for the y axis", 0, GetScreenHeight() - FONT_SIZE * 5, FONT_SIZE, LIGHTGRAY);

                // Draw ball
                DrawCircleV(ballPosition, 16.0f, MAROON);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }

        // NoEase function, used when "no easing" is selected for any axis. It just ignores all parameters besides b.
        static float NoEase(float t, float b, float c, float d)
        {
            float burn = t + b + c + d;  // Hack to avoid compiler warning (about unused variables)
            d += burn;

            return b;
        }
    }
}

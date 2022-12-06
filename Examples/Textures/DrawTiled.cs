/*******************************************************************************************
*
*   raylib [textures] example - Draw part of the texture tiled
*
*   This example has been created using raylib 3.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2020 Vlad Adrian (@demizdor) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.ConfigFlags;
using static Raylib_cs.TextureFilter;
using static Raylib_cs.MouseButton;

namespace Examples.Textures
{
    public class DrawTiled
    {
        const int OPT_WIDTH = 220;
        const int MARGIN_SIZE = 8;
        const int COLOR_SIZE = 16;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            int screenWidth = 800;
            int screenHeight = 450;

            SetConfigFlags(FLAG_WINDOW_RESIZABLE);
            InitWindow(screenWidth, screenHeight, "raylib [textures] example - Draw part of a texture tiled");

            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            Texture2D texPattern = LoadTexture("resources/patterns.png");

            // Makes the texture smoother when upscaled
            SetTextureFilter(texPattern, TEXTURE_FILTER_TRILINEAR);

            // Coordinates for all patterns inside the texture
            Rectangle[] recPattern = new[] {
                new Rectangle(3, 3, 66, 66),
                new Rectangle(75, 3, 100, 100),
                new Rectangle(3, 75, 66, 66),
                new Rectangle(7, 156, 50, 50),
                new Rectangle(85, 106, 90, 45),
                new Rectangle(75, 154, 100, 60)
            };

            // Setup colors
            Color[] colors = new[] { BLACK, MAROON, ORANGE, BLUE, PURPLE, BEIGE, LIME, RED, DARKGRAY, SKYBLUE };
            Rectangle[] colorRec = new Rectangle[colors.Length];

            // Calculate rectangle for each color
            for (int i = 0, x = 0, y = 0; i < colors.Length; i++)
            {
                colorRec[i].x = 2 + MARGIN_SIZE + x;
                colorRec[i].y = 22 + 256 + MARGIN_SIZE + y;
                colorRec[i].width = COLOR_SIZE * 2;
                colorRec[i].height = COLOR_SIZE;

                if (i == (colors.Length / 2 - 1))
                {
                    x = 0;
                    y += COLOR_SIZE + MARGIN_SIZE;
                }
                else
                {
                    x += (COLOR_SIZE * 2 + MARGIN_SIZE);
                }
            }

            int activePattern = 0, activeCol = 0;
            float scale = 1.0f, rotation = 0.0f;

            SetTargetFPS(60);
            //---------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                screenWidth = GetScreenWidth();
                screenHeight = GetScreenHeight();

                // Handle mouse
                if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON))
                {
                    Vector2 mouse = GetMousePosition();

                    // Check which pattern was clicked and set it as the active pattern
                    for (int i = 0; i < recPattern.Length; i++)
                    {
                        Rectangle rec = new Rectangle(
                            2 + MARGIN_SIZE + recPattern[i].x,
                            40 + MARGIN_SIZE + recPattern[i].y,
                            recPattern[i].width,
                            recPattern[i].height
                        );
                        if (CheckCollisionPointRec(mouse, rec))
                        {
                            activePattern = i;
                            break;
                        }
                    }

                    // Check to see which color was clicked and set it as the active color
                    for (int i = 0; i < colors.Length; ++i)
                    {
                        if (CheckCollisionPointRec(mouse, colorRec[i]))
                        {
                            activeCol = i;
                            break;
                        }
                    }
                }

                // Handle keys

                // Change scale
                if (IsKeyPressed(KEY_UP))
                {
                    scale += 0.25f;
                }
                if (IsKeyPressed(KEY_DOWN))
                {
                    scale -= 0.25f;
                }
                if (scale > 10.0f)
                {
                    scale = 10.0f;
                }
                else if (scale <= 0.0f)
                {
                    scale = 0.25f;
                }

                // Change rotation
                if (IsKeyPressed(KEY_LEFT))
                {
                    rotation -= 25.0f;
                }
                if (IsKeyPressed(KEY_RIGHT))
                {
                    rotation += 25.0f;
                }

                // Reset
                if (IsKeyPressed(KEY_SPACE))
                {
                    rotation = 0.0f;
                    scale = 1.0f;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // Draw the tiled area
                Rectangle source = recPattern[activePattern];
                Rectangle dest = new Rectangle(
                    OPT_WIDTH + MARGIN_SIZE,
                    MARGIN_SIZE,
                    screenWidth - OPT_WIDTH - 2 * MARGIN_SIZE,
                    screenHeight - 2 * MARGIN_SIZE
                );
                DrawTextureTiled(texPattern, source, dest, Vector2.Zero, rotation, scale, colors[activeCol]);

                // Draw options
                Color color = ColorAlpha(LIGHTGRAY, 0.5f);
                DrawRectangle(MARGIN_SIZE, MARGIN_SIZE, OPT_WIDTH - MARGIN_SIZE, screenHeight - 2 * MARGIN_SIZE, color);

                DrawText("Select Pattern", 2 + MARGIN_SIZE, 30 + MARGIN_SIZE, 10, BLACK);
                DrawTexture(texPattern, 2 + MARGIN_SIZE, 40 + MARGIN_SIZE, BLACK);
                DrawRectangle(
                    2 + MARGIN_SIZE + (int)recPattern[activePattern].x,
                    40 + MARGIN_SIZE + (int)recPattern[activePattern].y,
                    (int)recPattern[activePattern].width,
                    (int)recPattern[activePattern].height,
                    ColorAlpha(DARKBLUE, 0.3f)
                );

                DrawText("Select Color", 2 + MARGIN_SIZE, 10 + 256 + MARGIN_SIZE, 10, BLACK);
                for (int i = 0; i < colors.Length; i++)
                {
                    DrawRectangleRec(colorRec[i], colors[i]);
                    if (activeCol == i)
                    {
                        DrawRectangleLinesEx(colorRec[i], 3, ColorAlpha(WHITE, 0.5f));
                    }
                }

                DrawText("Scale (UP/DOWN to change)", 2 + MARGIN_SIZE, 80 + 256 + MARGIN_SIZE, 10, BLACK);
                DrawText($"{scale}x", 2 + MARGIN_SIZE, 92 + 256 + MARGIN_SIZE, 20, BLACK);

                DrawText("Rotation (LEFT/RIGHT to change)", 2 + MARGIN_SIZE, 122 + 256 + MARGIN_SIZE, 10, BLACK);
                DrawText($"{rotation} degrees", 2 + MARGIN_SIZE, 134 + 256 + MARGIN_SIZE, 20, BLACK);

                DrawText("Press [SPACE] to reset", 2 + MARGIN_SIZE, 164 + 256 + MARGIN_SIZE, 10, DARKBLUE);

                // Draw FPS
                DrawText($"{GetFPS()}", 2 + MARGIN_SIZE, 2 + MARGIN_SIZE, 20, BLACK);
                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texPattern);

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

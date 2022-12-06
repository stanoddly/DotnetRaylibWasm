/*******************************************************************************************
*
*   raylib [text] example - Draw text inside a rectangle
*
*   This example has been created using raylib 2.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Vlad Adrian (@demizdor) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2018 Vlad Adrian (@demizdor) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;

namespace Examples.Text
{
    public class RectangleBounds
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [text] example - draw text inside a rectangle");

            string text = "";
            text += "Text cannot escape this container ...word wrap also works when active so here's a long text for testing.";
            text += "\n\nLorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor ";
            text += "incididunt ut labore et dolore magna aliqua. Nec ullamcorper sit amet risus nullam eget felis eget.";

            bool resizing = false;
            bool wordWrap = true;

            Rectangle container = new Rectangle(25, 25, screenWidth - 50, screenHeight - 250);
            Rectangle resizer = new Rectangle(container.x + container.width - 17, container.y + container.height - 17, 14, 14);

            // Minimum width and heigh for the container rectangle
            const int minWidth = 60;
            const int minHeight = 60;
            const int maxWidth = screenWidth - 50;
            const int maxHeight = screenHeight - 160;

            Vector2 lastMouse = new Vector2(0.0f, 0.0f); // Stores last mouse coordinates
            Color borderColor = MAROON;         // Container border color
            Font font = GetFontDefault();       // Get default system font

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyPressed(KEY_SPACE))
                {
                    wordWrap = !wordWrap;
                }

                Vector2 mouse = GetMousePosition();

                // Check if the mouse is inside the container and toggle border color
                if (CheckCollisionPointRec(mouse, container))
                {
                    borderColor = ColorAlpha(MAROON, 0.4f);
                }
                else if (!resizing)
                {
                    borderColor = MAROON;
                }

                // Container resizing logic
                if (resizing)
                {
                    if (IsMouseButtonReleased(MOUSE_LEFT_BUTTON))
                    {
                        resizing = false;
                    }

                    int width = (int)(container.width + (mouse.X - lastMouse.X));
                    container.width = (width > minWidth) ? ((width < maxWidth) ? width : maxWidth) : minWidth;

                    int height = (int)(container.height + (mouse.Y - lastMouse.Y));
                    container.height = (height > minHeight) ? ((height < maxHeight) ? height : maxHeight) : minHeight;
                }
                else
                {
                    // Check if we're resizing
                    if (IsMouseButtonDown(MOUSE_LEFT_BUTTON) && CheckCollisionPointRec(mouse, resizer))
                    {
                        resizing = true;
                    }
                }

                // Move resizer rectangle properly
                resizer.x = container.x + container.width - 17;
                resizer.y = container.y + container.height - 17;

                lastMouse = mouse; // Update mouse
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawRectangleLinesEx(container, 3, borderColor); // Draw container border

                // Draw text in container (add some padding)
                /*DrawTextRec(font, text,
                           new Rectangle(container.x + 4, container.y + 4, container.width - 4, container.height - 4),
                           20.0f, 2.0f, wordWrap, GRAY);


                DrawTextBoxed(font, text, (Rectangle){ container.x + 4, container.y + 4, container.width - 4, container.height - 4 }, 20.0f, 2.0f, wordWrap, GRAY);
                */

                DrawRectangleRec(resizer, borderColor);         // Draw the resize box

                // Draw bottom info
                DrawRectangle(0, screenHeight - 54, screenWidth, 54, GRAY);
                DrawRectangleRec(new Rectangle(382, screenHeight - 34, 12, 12), MAROON);

                DrawText("Word Wrap: ", 313, screenHeight - 115, 20, BLACK);

                if (wordWrap)
                {
                    DrawText("ON", 447, screenHeight - 115, 20, RED);
                }
                else
                {
                    DrawText("OFF", 447, screenHeight - 115, 20, BLACK);
                }

                DrawText("Press [SPACE] to toggle word wrap", 218, screenHeight - 86, 20, GRAY);
                DrawText("Click hold & drag the    to resize the container", 155, screenHeight - 38, 20, RAYWHITE);

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

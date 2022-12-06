/*******************************************************************************************
*
*   raylib [core] example - 2d camera platformer
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by arvyy (@arvyy) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 arvyy (@arvyy)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples.Core
{
    public class Camera2dPlatformer
    {
        const int G = 400;
        const float PLAYER_JUMP_SPD = 350.0f;
        const float PLAYER_HOR_SPD = 200.0f;

        struct Player
        {
            public Vector2 position;
            public float speed;
            public bool canJump;
        }

        struct EnvItem
        {
            public Rectangle rect;
            public int blocking;
            public Color color;

            public EnvItem(Rectangle rect, int blocking, Color color)
            {
                this.rect = rect;
                this.blocking = blocking;
                this.color = color;
            }
        }

        delegate void CameraUpdaterCallback(ref Camera2D camera, ref Player player, EnvItem[] envItems, float delta, int width, int height);

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - 2d camera");

            Player player = new Player();
            player.position = new Vector2(400, 280);
            player.speed = 0;
            player.canJump = false;

            EnvItem[] envItems = new EnvItem[] {
                new EnvItem(new Rectangle(0, 0, 1000, 400), 0, LIGHTGRAY),
                new EnvItem(new Rectangle(0, 400, 1000, 200), 1, GRAY),
                new EnvItem(new Rectangle(300, 200, 400, 10), 1, GRAY),
                new EnvItem(new Rectangle(250, 300, 100, 10), 1, GRAY),
                new EnvItem(new Rectangle(650, 300, 100, 10), 1, GRAY)
            };

            Camera2D camera = new Camera2D();
            camera.target = player.position;
            camera.offset = new Vector2(screenWidth / 2, screenHeight / 2);
            camera.rotation = 0.0f;
            camera.zoom = 1.0f;

            // Store callbacks to the multiple update camera functions
            CameraUpdaterCallback[] cameraUpdaters = new CameraUpdaterCallback[] {
                UpdateCameraCenter,
                UpdateCameraCenterInsideMap,
                UpdateCameraCenterSmoothFollow,
                UpdateCameraEvenOutOnLanding,
                UpdateCameraPlayerBoundsPush
            };

            int cameraOption = 0;
            int cameraUpdatersLength = cameraUpdaters.Length;

            string[] cameraDescriptions = new string[]{
                "Follow player center",
                "Follow player center, but clamp to map edges",
                "Follow player center; smoothed",
                "Follow player center horizontally; updateplayer center vertically after landing",
                "Player push camera on getting too close to screen edge"
            };

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                float deltaTime = GetFrameTime();

                UpdatePlayer(ref player, envItems, deltaTime);

                camera.zoom += ((float)GetMouseWheelMove() * 0.05f);

                if (camera.zoom > 3.0f)
                    camera.zoom = 3.0f;
                else if (camera.zoom < 0.25f)
                    camera.zoom = 0.25f;

                if (IsKeyPressed(KEY_R))
                {
                    camera.zoom = 1.0f;
                    player.position = new Vector2(400, 280);
                }

                if (IsKeyPressed(KEY_C))
                    cameraOption = (cameraOption + 1) % cameraUpdatersLength;

                // Call update camera function by its pointer
                cameraUpdaters[cameraOption](ref camera, ref player, envItems, deltaTime, screenWidth, screenHeight);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(LIGHTGRAY);

                BeginMode2D(camera);

                for (int i = 0; i < envItems.Length; i++)
                    DrawRectangleRec(envItems[i].rect, envItems[i].color);

                Rectangle playerRect = new Rectangle(player.position.X - 20, player.position.Y - 40, 40, 40);
                DrawRectangleRec(playerRect, RED);

                EndMode2D();

                DrawText("Controls:", 20, 20, 10, BLACK);
                DrawText("- Right/Left to move", 40, 40, 10, DARKGRAY);
                DrawText("- Space to jump", 40, 60, 10, DARKGRAY);
                DrawText("- Mouse Wheel to Zoom in-out, R to reset zoom", 40, 80, 10, DARKGRAY);
                DrawText("- C to change camera mode", 40, 100, 10, DARKGRAY);
                DrawText("Current camera mode:", 20, 120, 10, BLACK);
                DrawText(cameraDescriptions[cameraOption], 40, 140, 10, DARKGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }

        static void UpdatePlayer(ref Player player, EnvItem[] envItems, float delta)
        {
            if (IsKeyDown(KEY_LEFT))
                player.position.X -= PLAYER_HOR_SPD * delta;
            if (IsKeyDown(KEY_RIGHT))
                player.position.X += PLAYER_HOR_SPD * delta;
            if (IsKeyDown(KEY_SPACE) && player.canJump)
            {
                player.speed = -PLAYER_JUMP_SPD;
                player.canJump = false;
            }

            int hitObstacle = 0;
            for (int i = 0; i < envItems.Length; i++)
            {
                EnvItem ei = envItems[i];
                Vector2 p = player.position;
                if (ei.blocking != 0 &&
                    ei.rect.x <= p.X &&
                    ei.rect.x + ei.rect.width >= p.X &&
                    ei.rect.y >= p.Y &&
                    ei.rect.y < p.Y + player.speed * delta)
                {
                    hitObstacle = 1;
                    player.speed = 0.0f;
                    player.position.Y = ei.rect.y;
                }
            }

            if (hitObstacle == 0)
            {
                player.position.Y += player.speed * delta;
                player.speed += G * delta;
                player.canJump = false;
            }
            else
                player.canJump = true;
        }

        static void UpdateCameraCenter(ref Camera2D camera, ref Player player, EnvItem[] envItems, float delta, int width, int height)
        {
            camera.offset = new Vector2(width / 2, height / 2);
            camera.target = player.position;
        }

        static void UpdateCameraCenterInsideMap(ref Camera2D camera, ref Player player, EnvItem[] envItems, float delta, int width, int height)
        {
            camera.target = player.position;
            camera.offset = new Vector2(width / 2, height / 2);
            float minX = 1000, minY = 1000, maxX = -1000, maxY = -1000;

            for (int i = 0; i < envItems.Length; i++)
            {
                EnvItem ei = envItems[i];
                minX = Math.Min(ei.rect.x, minX);
                maxX = Math.Max(ei.rect.x + ei.rect.width, maxX);
                minY = Math.Min(ei.rect.y, minY);
                maxY = Math.Max(ei.rect.y + ei.rect.height, maxY);
            }

            Vector2 max = GetWorldToScreen2D(new Vector2(maxX, maxY), camera);
            Vector2 min = GetWorldToScreen2D(new Vector2(minX, minY), camera);

            if (max.X < width)
                camera.offset.X = width - (max.X - width / 2);
            if (max.Y < height)
                camera.offset.Y = height - (max.Y - height / 2);
            if (min.X > 0)
                camera.offset.X = width / 2 - min.X;
            if (min.Y > 0)
                camera.offset.Y = height / 2 - min.Y;
        }

        static void UpdateCameraCenterSmoothFollow(ref Camera2D camera, ref Player player, EnvItem[] envItems, float delta, int width, int height)
        {
            const float minSpeed = 30;
            const float minEffectLength = 10;
            const float fractionSpeed = 0.8f;

            camera.offset = new Vector2(width / 2, height / 2);
            Vector2 diff = Vector2Subtract(player.position, camera.target);
            float length = Vector2Length(diff);

            if (length > minEffectLength)
            {
                float speed = Math.Max(fractionSpeed * length, minSpeed);
                camera.target = Vector2Add(camera.target, Vector2Scale(diff, speed * delta / length));
            }
        }

        static void UpdateCameraEvenOutOnLanding(ref Camera2D camera, ref Player player, EnvItem[] envItems, float delta, int width, int height)
        {
            float evenOutSpeed = 700;
            int eveningOut = 0;
            float evenOutTarget = 0.0f;

            camera.offset = new Vector2(width / 2, height / 2);
            camera.target.X = player.position.X;

            if (eveningOut != 0)
            {
                if (evenOutTarget > camera.target.Y)
                {
                    camera.target.Y += evenOutSpeed * delta;

                    if (camera.target.Y > evenOutTarget)
                    {
                        camera.target.Y = evenOutTarget;
                        eveningOut = 0;
                    }
                }
                else
                {
                    camera.target.Y -= evenOutSpeed * delta;

                    if (camera.target.Y < evenOutTarget)
                    {
                        camera.target.Y = evenOutTarget;
                        eveningOut = 0;
                    }
                }
            }
            else
            {
                if (player.canJump && (player.speed == 0) && (player.position.Y != camera.target.Y))
                {
                    eveningOut = 1;
                    evenOutTarget = player.position.Y;
                }
            }
        }

        static void UpdateCameraPlayerBoundsPush(ref Camera2D camera, ref Player player, EnvItem[] envItems, float delta, int width, int height)
        {
            Vector2 bbox = new Vector2(0.2f, 0.2f);

            Vector2 bboxWorldMin = GetScreenToWorld2D(new Vector2((1 - bbox.X) * 0.5f * width, (1 - bbox.Y) * 0.5f * height), camera);
            Vector2 bboxWorldMax = GetScreenToWorld2D(new Vector2((1 + bbox.X) * 0.5f * width, (1 + bbox.Y) * 0.5f * height), camera);
            camera.offset = new Vector2((1 - bbox.X) * 0.5f * width, (1 - bbox.Y) * 0.5f * height);

            if (player.position.X < bboxWorldMin.X)
                camera.target.X = player.position.X;
            if (player.position.Y < bboxWorldMin.Y)
                camera.target.Y = player.position.Y;
            if (player.position.X > bboxWorldMax.X)
                camera.target.X = bboxWorldMin.X + (player.position.X - bboxWorldMax.X);
            if (player.position.Y > bboxWorldMax.Y)
                camera.target.Y = bboxWorldMin.Y + (player.position.Y - bboxWorldMax.Y);
        }
    }
}

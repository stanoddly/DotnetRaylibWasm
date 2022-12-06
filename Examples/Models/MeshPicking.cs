/*******************************************************************************************
*
*   raylib [models] example - Mesh picking in 3d mode, ground plane, triangle, mesh
*
*   This example has been created using raylib 1.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*   Example contributed by Joel Davis (@joeld42)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.CameraMode;
using static Raylib_cs.MaterialMapIndex;

namespace Examples.Models
{
    public class MeshPicking
    {
        public const float FLT_MAX = 3.40282347E+38F;

        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - mesh picking");

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(20.0f, 20.0f, 20.0f);
            camera.target = new Vector3(0.0f, 8.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.6f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            // Picking ray
            Ray ray = new Ray();

            Model tower = LoadModel("resources/models/obj/turret.obj");
            Texture2D texture = LoadTexture("resources/models/obj/turret_diffuse.png");
            Raylib.SetMaterialTexture(ref tower, 0, MATERIAL_MAP_ALBEDO, ref texture);

            Vector3 towerPos = new Vector3(0.0f, 0.0f, 0.0f);
            BoundingBox towerBBox = GetMeshBoundingBox(tower.meshes[0]);
            bool hitMeshBBox = false;
            bool hitTriangle = false;

            // Ground quad
            Vector3 g0 = new Vector3(-50.0f, 0.0f, -50.0f);
            Vector3 g1 = new Vector3(-50.0f, 0.0f, 50.0f);
            Vector3 g2 = new Vector3(50.0f, 0.0f, 50.0f);
            Vector3 g3 = new Vector3(50.0f, 0.0f, -50.0f);

            // Test triangle
            Vector3 ta = new Vector3(-25.0f, 0.5f, 0.0f);
            Vector3 tb = new Vector3(-4.0f, 2.5f, 1.0f);
            Vector3 tc = new Vector3(-8.0f, 6.5f, 0.0f);

            Vector3 bary = new Vector3(0.0f, 0.0f, 0.0f);

            SetCameraMode(camera, CAMERA_FREE);

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second

            //----------------------------------------------------------------------------------
            // Main game loop
            //--------------------------------------------------------------------------------------
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                //----------------------------------------------------------------------------------
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);       // Update camera

                // Display information about closest hit
                RayCollision collision = new RayCollision();
                string hitObjectName = "None";
                collision.distance = FLT_MAX;
                collision.hit = false;
                Color cursorColor = WHITE;

                // Get ray and test against ground, triangle, and mesh
                ray = GetMouseRay(GetMousePosition(), camera);

                // Check ray collision aginst ground plane
                RayCollision groundHitInfo = GetRayCollisionQuad(ray, g0, g1, g2, g3);

                if (groundHitInfo.hit && (groundHitInfo.distance < collision.distance))
                {
                    collision = groundHitInfo;
                    cursorColor = GREEN;
                    hitObjectName = "Ground";
                }

                // Check ray collision against test triangle
                RayCollision triHitInfo = GetRayCollisionTriangle(ray, ta, tb, tc);

                if (triHitInfo.hit && (triHitInfo.distance < collision.distance))
                {
                    collision = triHitInfo;
                    cursorColor = PURPLE;
                    hitObjectName = "Triangle";

                    bary = Vector3Barycenter(collision.point, ta, tb, tc);
                    hitTriangle = true;
                }
                else
                {
                    hitTriangle = false;
                }

                // Check ray collision against bounding box first, before trying the full ray-mesh test
                RayCollision boxHitInfo = GetRayCollisionBox(ray, towerBBox);
                if (boxHitInfo.hit && boxHitInfo.distance < collision.distance)
                {
                    hitMeshBBox = true;

                    // Check ray collision against model
                    // NOTE: It considers model.transform matrix!
                    RayCollision meshHitInfo = GetRayCollisionModel(ray, tower);

                    if (meshHitInfo.hit && (meshHitInfo.distance < collision.distance))
                    {
                        collision = meshHitInfo;
                        cursorColor = ORANGE;
                        hitObjectName = "Mesh";
                    }

                }
                hitMeshBBox = false;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                // Draw the tower
                // WARNING: If scale is different than 1.0f,
                // not considered by GetCollisionRayModel()
                DrawModel(tower, towerPos, 1.0f, WHITE);

                // Draw the test triangle
                DrawLine3D(ta, tb, PURPLE);
                DrawLine3D(tb, tc, PURPLE);
                DrawLine3D(tc, ta, PURPLE);

                // Draw the mesh bbox if we hit it
                if (hitMeshBBox)
                    DrawBoundingBox(towerBBox, LIME);

                // If we hit something, draw the cursor at the hit point
                if (collision.hit)
                {
                    DrawCube(collision.point, 0.3f, 0.3f, 0.3f, cursorColor);
                    DrawCubeWires(collision.point, 0.3f, 0.3f, 0.3f, RED);

                    Vector3 normalEnd = collision.point + collision.normal;
                    DrawLine3D(collision.point, normalEnd, RED);
                }

                DrawRay(ray, MAROON);

                DrawGrid(10, 10.0f);

                EndMode3D();

                // Draw some debug GUI text
                DrawText(string.Format("Hit Object: {0}", hitObjectName), 10, 50, 10, BLACK);

                if (collision.hit)
                {
                    int ypos = 70;

                    var x = string.Format("Distance: {0:000.00}", collision.distance);
                    DrawText(string.Format("Distance: {0:000.00}", collision.distance), 10, ypos, 10, BLACK);

                    DrawText(string.Format("Hit Pos: {0:000.00} {1:000.00} {2:000.00}",
                                        collision.point.X,
                                        collision.point.Y,
                                        collision.point.Z), 10, ypos + 15, 10, BLACK);

                    DrawText(string.Format("Hit Norm: {0:000.00} {1:000.00} {2:000.00}",
                                        collision.normal.X,
                                        collision.normal.Y,
                                        collision.normal.Z), 10, ypos + 30, 10, BLACK);

                    if (hitTriangle)
                        DrawText(string.Format("Barycenter:{0:000.00} {1:000.00} {2:000.00}", bary.X, bary.Y, bary.Z), 10, ypos + 45, 10, BLACK);
                }

                DrawText("Use Mouse to Move Camera", 10, 430, 10, GRAY);

                DrawText("(c) Turret 3D model by Alberto Cano", screenWidth - 200, screenHeight - 20, 10, GRAY);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(tower);         // Unload model
            UnloadTexture(texture);     // Unload texture

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}

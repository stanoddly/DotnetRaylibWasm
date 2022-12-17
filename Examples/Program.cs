using Raylib_cs;
using Examples.Core;
using Examples.Shapes;
using Examples.Textures;
using Examples.Text;
using Examples.Models;
using Examples.Shaders;
using Examples.Audio;

namespace Examples
{
    class Program
    {
        static void Main2(string[] args)
        {
            //Raylib.SetTraceLogCallback(&Logging.LogConsole);

            RunCoreExamples();
            RunShapesExamples();
            RunTextureExamples();
            RunTextExamples();
            RunModelExamples();
            RunShaderExamples();
            RunAudioExamples();
        }

        static void RunCoreExamples()
        {
            Camera2dPlatformer.Main();
            Camera2dDemo.Main();
            Camera3dFirstPerson.Main();
            Camera3dFree.Main();
            Camera3dMode.Main();
            Picking3d.Main();
            BasicScreenManager.Main();
            BasicWindow.Main();
            CustomLogging.Main();
            DropFiles.Main();
            InputGamepad.Main();
            InputGestures.Main();
            InputKeys.Main();
            InputMouseWheel.Main();
            InputMouse.Main();
            InputMultitouch.Main();
            RandomValues.Main();
            ScissorTest.Main();
            SmoothPixelPerfect.Main();
            VrSimulator.Main();
            WindowFlags.Main();
            WindowLetterbox.Main();
            WorldScreen.Main();
        }

        static void RunShapesExamples()
        {
            BasicShapes.Main();
            BouncingBall.Main();
            CollisionArea.Main();
            ColorsPalette.Main();
            EasingsBallAnim.Main();
            // EasingsBoxAnim.Main();
            EasingsRectangleArray.Main();
            FollowingEyes.Main();
            LinesBezier.Main();
            LogoRaylibAnim.Main();
            LogoRaylibShape.Main();
            RectangleScaling.Main();
        }

        static void RunTextureExamples()
        {
            BackgroundScrolling.Main();
            BlendModes.Main();
            Bunnymark.Main();
            ImageDrawing.Main();
            ImageGeneration.Main();
            ImageLoading.Main();
            ImageProcessing.Main();
            ImageText.Main();
            LogoRaylibTexture.Main();
            MousePainting.Main();
            NpatchDrawing.Main();
            ParticlesBlending.Main();
            RawData.Main();
            SpriteAnim.Main();
            SpriteButton.Main();
            SpriteExplosion.Main();
            SrcRecDstRec.Main();
            ToImage.Main();
        }

        static void RunTextExamples()
        {
            // Draw3d.Main();
            FontFilters.Main();
            FontLoading.Main();
            FontSdf.Main();
            FontSpritefont.Main();
            FormatText.Main();
            InputBox.Main();
            RaylibFonts.Main();
            RectangleBounds.Main();
            WritingAnim.Main();
        }

        static void RunModelExamples()
        {
            AnimationDemo.Main();
            BillboardDemo.Main();
            BoxCollisions.Main();
            CubicmapDemo.Main();
            FirstPersonMaze.Main();
            GeometricShapes.Main();
            HeightmapDemo.Main();
            LoadingGltf.Main();
            LoadingVox.Main();
            LoadingDemo.Main();
            MeshGeneration.Main();
            // MeshPicking.Main();
            OrthographicProjection.Main();
            SolarSystem.Main();
            SkyboxDemo.Main();
            WavingCubes.Main();
            YawPitchRoll.Main();
        }

        static void RunShaderExamples()
        {
            BasicLighting.Main();
            CustomUniform.Main();
            Eratosthenes.Main();
            Fog.Main();
            HotReloading.Main();
            JuliaSet.Main();
            ModelShader.Main();
            MultiSample2d.Main();
            PaletteSwitch.Main();
            PostProcessing.Main();
            Raymarching.Main();
            MeshInstancing.Main();
            ShapesTextures.Main();
            SimpleMask.Main();
            Spotlight.Main();
            TextureDrawing.Main();
            TextureOutline.Main();
            TextureWaves.Main();
        }

        static void RunAudioExamples()
        {
            ModulePlaying.Main();
            MultichannelSound.Main();
            MusicStreamDemo.Main();
            RawStream.Main();
            SoundLoading.Main();
        }
    }
}

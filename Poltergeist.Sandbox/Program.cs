using Poltergeist.Core;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.Glfw.Structures;

namespace Poltergeist.Sandbox
{
	public sealed class Program
	{
		public static unsafe void Main()
		{
			using (ExitHandler.LifetimeHandle _ = new())
			{
				const int glfwTrue = 1;
				const int glfwFalse = 0;
				const int glfwContextVersionMajor = 0x00022002;
				const int glfwContextVersionMinor = 0x00022003;
				const int glfwOpenGlProfile = 0x00022008;
				const int glfwOpenGlCoreProfile = 0x00032001;
				const int glfwOpenGlForwardCompat = 0x00022006;
				const int glfwResizable = 0x00020003;

				GlfwNative.Init();
				GlfwNative.WindowHint(glfwContextVersionMajor, 3);
				GlfwNative.WindowHint(glfwContextVersionMinor, 3);
				GlfwNative.WindowHint(glfwOpenGlProfile, glfwOpenGlCoreProfile);
				GlfwNative.WindowHint(glfwOpenGlForwardCompat, glfwTrue);
				GlfwNative.WindowHint(glfwResizable, glfwFalse);

				GlfwWindow* window = GlfwNative.CreateWindow(800, 600, "Poltergeist Editor", null, null);

				GlfwNative.MakeContextCurrent(window);

				while (GlfwNative.WindowShouldClose(window) == glfwFalse)
				{
					GlfwNative.PollEvents();

					GlfwNative.SwapBuffers(window);
				}

				GlfwNative.Terminate();
			}
		}
	}
}

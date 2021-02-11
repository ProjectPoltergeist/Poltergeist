using Poltergeist.Core;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.Glfw.Structures;
using Poltergeist.Core.Bindings.OpenGl;

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

				const int glFalse = 0;
				const int glFloat = 0x1406;
				const int glArrayBuffer = 0x8892;
				const int glStaticDraw = 0x88E4;
				const int glColorBufferBit = 0x00004000;
				const int glTriangles = 0x0004;

				GlfwNative.Init();
				GlfwNative.WindowHint(glfwContextVersionMajor, 3);
				GlfwNative.WindowHint(glfwContextVersionMinor, 3);
				GlfwNative.WindowHint(glfwOpenGlProfile, glfwOpenGlCoreProfile);
				GlfwNative.WindowHint(glfwOpenGlForwardCompat, glfwTrue);
				GlfwNative.WindowHint(glfwResizable, glfwFalse);

				GlfwWindow* window = GlfwNative.CreateWindow(800, 600, "Poltergeist Editor", null, null);

				GlfwNative.MakeContextCurrent(window);

				var openGl = new OpenGlNative(&GlfwNative.GetProcessAddress);

				var vertexArrayObjects = stackalloc uint[1];

				openGl.GenerateVertexArrays(1, vertexArrayObjects);
				openGl.BindVertexArray(vertexArrayObjects[0]);

				var vertexBufferObjects = stackalloc uint[1];

				openGl.GenerateBuffers(1, vertexBufferObjects);
				openGl.BindBuffer(glArrayBuffer, vertexBufferObjects[0]);

				var vertices = stackalloc float[]
				{
					-0.5f, -0.5f, 0.0f,
					0.5f, -0.5f, 0.0f,
					0.0f, 0.5f, 0.0f
				};

				openGl.BufferData(glArrayBuffer, 9 * sizeof(float), vertices, glStaticDraw);

				openGl.VertexAttributePointer(0, 3, glFloat, glFalse, 3 * sizeof(float), null);
				openGl.EnableVertexAttributeArray(0);

				while (GlfwNative.WindowShouldClose(window) == glfwFalse)
				{
					GlfwNative.PollEvents();

					openGl.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
					openGl.Clear(glColorBufferBit);
					openGl.DrawArrays(glTriangles, 0, 3);

					GlfwNative.SwapBuffers(window);
				}

				openGl.DeleteVertexArrays(1, vertexArrayObjects);
				openGl.DeleteBuffers(1, vertexBufferObjects);

				GlfwNative.DestroyWindow(window);
				GlfwNative.Terminate();
			}
		}
	}
}

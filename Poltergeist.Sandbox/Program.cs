using Poltergeist.Core;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.OpenGl;
using Poltergeist.Core.Windowing;

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

				using (var window = new Window("Poltergeist Editor"))
				{
					var vertexArrayObjects = stackalloc uint[1];

					OpenGl3Native.GenerateVertexArrays(1, vertexArrayObjects);
					OpenGl3Native.BindVertexArray(vertexArrayObjects[0]);

					var vertexBufferObjects = stackalloc uint[1];

					OpenGl3Native.GenerateBuffers(1, vertexBufferObjects);
					OpenGl3Native.BindBuffer(glArrayBuffer, vertexBufferObjects[0]);

					var vertices = stackalloc float[]
					{
						-0.5f, -0.5f, 0.0f,
						0.5f, -0.5f, 0.0f,
						0.0f, 0.5f, 0.0f
					};
				
					OpenGl3Native.BufferData(glArrayBuffer, 9 * sizeof(float), vertices, glStaticDraw);

					OpenGl3Native.VertexAttributePointer(0, 3, glFloat, glFalse, 3 * sizeof(float), null);
					OpenGl3Native.EnableVertexAttributeArray(0);

					while (window.IsOpen)
					{
						window.PollEvents();

						OpenGl3Native.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
						OpenGl3Native.Clear(glColorBufferBit);
						OpenGl3Native.DrawArrays(glTriangles, 0, 3);

						window.SwapBuffers();
					}

					OpenGl3Native.DeleteVertexArrays(1, vertexArrayObjects);
					OpenGl3Native.DeleteBuffers(1, vertexBufferObjects);
				}

				GlfwNative.Terminate();
			}
		}
	}
}

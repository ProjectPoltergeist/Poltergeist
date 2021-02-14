using System;
using Poltergeist.Core;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.Glfw.Structures;
using Poltergeist.Core.Bindings.OpenGl;
using Poltergeist.Core.Rendering;

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

				var vertexArray = VertexArray.Create();
				vertexArray.Bind();

				Span<float> vertices = stackalloc float[]
				{
					-0.5f, -0.5f, 0.0f,
					0.5f, -0.5f, 0.0f,
					0.0f, 0.5f, 0.0f
				};
				
				Span<VertexBufferElement> layout = stackalloc VertexBufferElement[]
				{
					new(OpenGlType.Float, 3)	
				};

				using (var vertexBuffer = VertexBuffer.Create<float>(vertices, layout)) {}

				while (GlfwNative.WindowShouldClose(window) == glfwFalse)
				{
					GlfwNative.PollEvents();

					OpenGl3Native.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
					OpenGl3Native.Clear(glColorBufferBit);
					OpenGl3Native.DrawArrays(glTriangles, 0, 3);

					GlfwNative.SwapBuffers(window);
				}

				vertexArray.Unbind();
				vertexArray.Dispose();

				GlfwNative.DestroyWindow(window);
				GlfwNative.Terminate();
			}
		}
	}
}

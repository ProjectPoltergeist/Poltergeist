using System;
using System.Runtime.InteropServices;
using Poltergeist.Core;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.OpenGl;
using Poltergeist.Core.Rendering;
using Poltergeist.Core.Windowing;

namespace Poltergeist.Sandbox
{
	public sealed class Program
	{
		public static unsafe void Main()
		{
			using (new ExitHandler.LifetimeHandle())
			{
				const int glfwTrue = 1;
				const int glfwFalse = 0;
				const int glfwOpenGlCoreProfile = 0x00032001;

				const int glColorBufferBit = 0x00004000;
				const int glTriangles = 0x0004;

				GlfwNative.Init();
				GlfwNative.WindowHint(GlfwHint.ContextVersionMajor, 3);
				GlfwNative.WindowHint(GlfwHint.ContextVersionMinor, 3);
				GlfwNative.WindowHint(GlfwHint.OpenglProfile, glfwOpenGlCoreProfile);
				GlfwNative.WindowHint(GlfwHint.OpenglForwardCompat, glfwTrue);
				GlfwNative.WindowHint(GlfwHint.Resizable, glfwFalse);

				using (var window = new Window("Poltergeist Editor"))
				{
					window.SetWindowTitle("( ͡° ͜ʖ ͡°)");
					using (var vertexArray = VertexArray.Create())
					{
						vertexArray.Bind();

						Span<float> vertices = stackalloc float[]
						{
							-0.5f, -0.5f, 0.0f,
							0.5f, -0.5f, 0.0f,
							0.0f, 0.5f, 0.0f
						};

						Span<VertexBufferElement> layout = stackalloc VertexBufferElement[]
						{
							new VertexBufferElement(OpenGlType.Float, 3)
						};

						using (VertexBuffer.Create<float>(vertices, layout)) { }

						while (window.IsOpen)
						{
							window.PollEvents();

							OpenGl3Native.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
							OpenGl3Native.Clear(glColorBufferBit);
							OpenGl3Native.DrawArrays(glTriangles, 0, 3);

							window.SwapBuffers();
						}

						vertexArray.Unbind();
					}

				}

				GlfwNative.Terminate();
			}
		}
	}
}

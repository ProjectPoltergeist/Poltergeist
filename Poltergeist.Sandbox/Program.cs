using Poltergeist.Core;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.OpenGl;
using Poltergeist.Core.Rendering;
using Poltergeist.Core.Windowing;
using System;

namespace Poltergeist.Sandbox
{
	public sealed class Program
	{
		private static float colorR, colorB;
		private static int xscale, yscale;

		public static unsafe void Main()
		{
			using (new ExitHandler.LifetimeHandle())
			{

				const int glColorBufferBit = 0x00004000;
				const int glTriangles = 0x0004;

				GlfwNative.Init();
				GlfwNative.WindowHint(GlfwHint.ContextVersionMajor, 3);
				GlfwNative.WindowHint(GlfwHint.ContextVersionMinor, 3);
				GlfwNative.WindowHint(GlfwHint.OpenGlProfile, GlfwArg.OpenGLCoreProfile);
				GlfwNative.WindowHint(GlfwHint.OpenGlForwardCompat, GlfwArg.True);
				GlfwNative.WindowHint(GlfwHint.Resizable, GlfwArg.False);


				using (var window = new Window("Poltergeist Sandbox"))
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

						using (VertexBuffer.Create<float>(vertices, layout))
						{
							while (window.IsOpen)
							{
								window.PollEvents();
								Console.WriteLine(yscale);
								OpenGl3Native.ClearColor(colorR, colorB, 0f,1f);
								OpenGl3Native.Clear(glColorBufferBit);
								OpenGl3Native.DrawArrays(glTriangles, 0, 3);

								window.SwapBuffers();
							}

							vertexArray.Unbind();
						}
					}
				}

				GlfwNative.Terminate();
			}
		}
	}
}

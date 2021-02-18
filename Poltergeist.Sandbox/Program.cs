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
							0.5f, 0.5f, 0.0f,
							0.5f, -0.5f, 0.0f,
							-0.5f, -0.5f, 0.0f,
							-0.5f, 0.5f, 0.0f
						};

						Span<VertexBufferElement> layout = stackalloc VertexBufferElement[]
						{
							new VertexBufferElement(OpenGlType.Float, 3)
						};

						using (VertexBuffer.Create<float>(vertices, layout))
						{
							Span<int> indices = stackalloc int[]
							{
								0, 1, 3, 1, 2, 3	
							};

							using (var indexBuffer = IndexBuffer.Create<int>(indices))
							{
								indexBuffer.Bind();
								
								while (window.IsOpen)
								{
									window.PollEvents();

									OpenGl3Native.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
									OpenGl3Native.Clear(OpenGlClearMask.ColorBufferBit);
									OpenGl3Native.DrawElements(OpenGlPrimitive.Triangles, 6, OpenGlType.UnsignedInt, null);

									window.SwapBuffers();
								}
								
								vertexArray.Unbind();
								indexBuffer.Unbind();
							}
						}
					}
				}

				GlfwNative.Terminate();
			}
		}
	}
}

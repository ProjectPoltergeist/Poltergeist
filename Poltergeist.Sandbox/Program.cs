using System;
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

				using (var window = new Window("Poltergeist Sandbox"))
				{
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

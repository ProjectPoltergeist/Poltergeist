using System;
using System.IO;
using Poltergeist.Core;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.Glfw.Enums;
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
				GlfwNative.Initialize();
				GlfwNative.WindowHint(GlfwHint.ContextVersionMajor, 3);
				GlfwNative.WindowHint(GlfwHint.ContextVersionMinor, 3);
				GlfwNative.WindowHint(GlfwHint.OpenGlProfile, GlfwProfile.OpenGLCoreProfile);
				GlfwNative.WindowHint(GlfwHint.OpenGlForwardCompat, true);
				GlfwNative.WindowHint(GlfwHint.Resizable, false);

				using (var window = new Window("Poltergeist Sandbox"))
				{
					var fragmentShaderSource = File.ReadAllText("core.frag");
					var vertexShaderSource = File.ReadAllText("core.vert");
					
					using (var shader = Shader.Create(fragmentShaderSource, vertexShaderSource))
					{
						shader.Bind();

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

						shader.Unbind();
					}
				}

				GlfwNative.Terminate();
			}
		}
	}
}

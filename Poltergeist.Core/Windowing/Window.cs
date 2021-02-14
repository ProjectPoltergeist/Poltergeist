using System;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.Glfw.Structures;

namespace Poltergeist.Core.Windowing
{
	public class Window : IDisposable
	{
		public unsafe bool IsOpen => GlfwNative.WindowShouldClose(_window) == 0;
	
		private readonly unsafe GlfwWindow* _window;
		
		public unsafe Window(string title, int width = 800, int height = 600)
		{
			_window = GlfwNative.CreateWindow(width, height, title, null, null);
			
			GlfwNative.MakeContextCurrent(_window);
		}

		public void PollEvents() => GlfwNative.PollEvents();
		
		public unsafe void SwapBuffers() => GlfwNative.SwapBuffers(_window);
		
		public unsafe void Dispose()
		{
			GlfwNative.DestroyWindow(_window);
		}
	}
}

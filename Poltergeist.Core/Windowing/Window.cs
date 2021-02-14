using System;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Bindings.Glfw.Structures;

namespace Poltergeist.Core.Windowing
{
	public unsafe class Window : IDisposable
	{
		public bool IsOpen => GlfwNative.WindowShouldClose(_window) == 0;
	
		private readonly GlfwWindow* _window;
		
		public Window(string title, int width = 800, int height = 600)
		{
			_window = GlfwNative.CreateWindow(width, height, title, null, null);
			
			GlfwNative.MakeContextCurrent(_window);
		}

		public void PollEvents() => GlfwNative.PollEvents();
		
		public void SwapBuffers() => GlfwNative.SwapBuffers(_window);
		
		public void Dispose()
		{
			GlfwNative.DestroyWindow(_window);
		}
	}
}

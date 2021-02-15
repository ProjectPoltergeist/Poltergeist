using Poltergeist.Core.Bindings.Glfw.Structures;

namespace Poltergeist.Core.Bindings.Glfw.Callbacks
{
	public unsafe struct GlfwWindowCloseFun
	{
		public delegate void WindowCloseCallback(GlfwWindow* window, int x, int y);
		public readonly WindowCloseCallback windowCloseCallback;
	}
}

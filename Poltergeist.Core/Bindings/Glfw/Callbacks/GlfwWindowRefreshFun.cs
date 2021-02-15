using Poltergeist.Core.Bindings.Glfw.Structures;

namespace Poltergeist.Core.Bindings.Glfw.Callbacks
{
	public unsafe struct GlfwWindowRefreshFun
	{
		public delegate void WindowRefreshCallback(GlfwWindow* window, int x, int y);
		public readonly WindowRefreshCallback windowRefreshCallback;
	}
}

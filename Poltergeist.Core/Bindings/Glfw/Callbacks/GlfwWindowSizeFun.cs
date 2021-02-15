using Poltergeist.Core.Bindings.Glfw.Structures;

namespace Poltergeist.Core.Bindings.Glfw.Callbacks
{
	public unsafe struct GlfwWindowSizeFun
	{
		public delegate void WindowSizeCallback(GlfwWindow* window, int x, int y);
		public readonly WindowSizeCallback windowSizeCallback;
	}
}

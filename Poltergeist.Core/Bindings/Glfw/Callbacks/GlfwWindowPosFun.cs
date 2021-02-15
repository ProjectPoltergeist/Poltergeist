using Poltergeist.Core.Bindings.Glfw.Structures;

namespace Poltergeist.Core.Bindings.Glfw.Callbacks
{
	public unsafe struct GlfwWindowPosFun
	{
		public  delegate void WindowPosCallback(GlfwWindow* window, int x, int y);
		public readonly WindowPosCallback windowPosCallback;
	}
}

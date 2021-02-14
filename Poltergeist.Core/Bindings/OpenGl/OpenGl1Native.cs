using Poltergeist.Core.Bindings.Glfw;

namespace Poltergeist.Core.Bindings.OpenGl
{
	public abstract unsafe class OpenGl1Native
	{
		public static readonly bool IsSupported = true;

		public static readonly delegate* unmanaged[Cdecl]<int, uint*, void> GenerateBuffers =
			(delegate* unmanaged[Cdecl]<int, uint*, void>)GlfwNative.GetProcessAddress("glGenBuffers");

		public static readonly delegate* unmanaged[Cdecl]<int, uint*, void> DeleteBuffers =
			(delegate* unmanaged[Cdecl]<int, uint*, void>)GlfwNative.GetProcessAddress("glDeleteBuffers");

		public static readonly delegate* unmanaged[Cdecl]<int, uint, void> BindBuffer =
			(delegate* unmanaged[Cdecl]<int, uint, void>)GlfwNative.GetProcessAddress("glBindBuffer");

		public static readonly delegate* unmanaged[Cdecl]<int, long, void*, int, void> BufferData =
			(delegate* unmanaged[Cdecl]<int, long, void*, int, void>)GlfwNative.GetProcessAddress("glBufferData");

		public static readonly delegate* unmanaged[Cdecl]<int, void> Clear =
			(delegate* unmanaged[Cdecl]<int, void>)GlfwNative.GetProcessAddress("glClear");

		public static readonly delegate* unmanaged[Cdecl]<float, float, float, float, void> ClearColor =
			(delegate* unmanaged[Cdecl]<float, float, float, float, void>)GlfwNative.GetProcessAddress("glClearColor");

		public static readonly delegate* unmanaged[Cdecl]<int, int, int, void> DrawArrays =
			(delegate* unmanaged[Cdecl]<int, int, int, void>)GlfwNative.GetProcessAddress("glDrawArrays");
	}
}

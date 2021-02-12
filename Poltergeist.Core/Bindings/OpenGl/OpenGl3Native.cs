using Poltergeist.Core.Bindings.Glfw;

namespace Poltergeist.Core.Bindings.OpenGl
{
	public abstract unsafe class OpenGl3Native : OpenGl2Native
	{
		public static readonly delegate* unmanaged[Cdecl]<int, uint*, void> GenerateVertexArrays 
			= (delegate* unmanaged[Cdecl]<int, uint*, void>)GlfwNative.GetProcessAddress("glGenVertexArrays");
		
		public static readonly delegate* unmanaged[Cdecl]<int, uint*, void> DeleteVertexArrays 
			= (delegate* unmanaged[Cdecl]<int, uint*, void>)GlfwNative.GetProcessAddress("glDeleteVertexArrays");
		
		public static readonly delegate* unmanaged[Cdecl]<uint, void> BindVertexArray 
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glBindVertexArray");
	}
}

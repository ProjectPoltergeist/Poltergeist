using Poltergeist.Core.Bindings.Glfw;

namespace Poltergeist.Core.Bindings.OpenGl
{
	public abstract unsafe class OpenGl2Native : OpenGl1Native
	{
		public static readonly delegate* unmanaged[Cdecl]<uint, int, int, int, int, void*, void> VertexAttributePointer
			= (delegate* unmanaged[Cdecl]<uint, int, int, int, int, void*, void>)GlfwNative.GetProcessAddress(
				"glVertexAttribPointer");

		public static readonly delegate* unmanaged[Cdecl]<uint, void> EnableVertexAttributeArray
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glEnableVertexAttribArray");
	}
}
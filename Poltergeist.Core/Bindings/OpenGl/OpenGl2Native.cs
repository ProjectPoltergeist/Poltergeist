using Poltergeist.Core.Bindings.Glfw;

namespace Poltergeist.Core.Bindings.OpenGl
{
	public abstract unsafe class OpenGl2Native : OpenGl1Native
	{
		public static readonly delegate* unmanaged[Cdecl]<int, int*, void> GetIntegerV
			= (delegate* unmanaged[Cdecl]<int, int*, void>)GlfwNative.GetProcessAddress("glGetIntegerv");

		public static (int major, int minor) GetVersion()
		{
			if (GetIntegerV == null)
				return (0, 0);
			int major = 0;
			const int glMajorVersion = 0x821B;
			GetIntegerV(glMajorVersion, &major);
			int minor = 0;
			const int glMinorVersion = 0x821C;
			GetIntegerV(glMinorVersion, &minor);
			return (major, minor);
		}

		public new static readonly bool IsSupported = OpenGl1Native.IsSupported && GetIntegerV != null;

		public static readonly delegate* unmanaged[Cdecl]<uint, int, int, int, int, nuint, void> VertexAttributePointer
			= (delegate* unmanaged[Cdecl]<uint, int, int, int, int, nuint, void>)GlfwNative.GetProcessAddress(
				"glVertexAttribPointer");

		public static readonly delegate* unmanaged[Cdecl]<uint, void> EnableVertexAttributeArray
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glEnableVertexAttribArray");
	}
}

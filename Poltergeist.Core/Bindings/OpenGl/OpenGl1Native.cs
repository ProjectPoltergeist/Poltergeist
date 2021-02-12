namespace Poltergeist.Core.Bindings.OpenGl
{
	public unsafe class OpenGl1Native
	{
		public readonly delegate* unmanaged[Cdecl]<int, uint*, void> GenerateBuffers;
		public readonly delegate* unmanaged[Cdecl]<int, uint*, void> DeleteBuffers;
		public readonly delegate* unmanaged[Cdecl]<int, uint, void> BindBuffer;
		public readonly delegate* unmanaged[Cdecl]<int, long, void*, int, void> BufferData;
		public readonly delegate* unmanaged[Cdecl]<int, void> Clear;
		public readonly delegate* unmanaged[Cdecl]<float, float, float, float, void> ClearColor;
		public readonly delegate* unmanaged[Cdecl]<int, int, int, void> DrawArrays;
	
		public OpenGl1Native(delegate*<string, void*> loader)
		{
			GenerateBuffers = (delegate* unmanaged[Cdecl]<int, uint*, void>)loader("glGenBuffers");
			DeleteBuffers = (delegate* unmanaged[Cdecl]<int, uint*, void>)loader("glDeleteBuffers");
			BindBuffer = (delegate* unmanaged[Cdecl]<int, uint, void>)loader("glBindBuffer");
			BufferData = (delegate* unmanaged[Cdecl]<int, long, void*, int, void>)loader("glBufferData");
			Clear = (delegate* unmanaged[Cdecl]<int, void>)loader("glClear");
			ClearColor = (delegate* unmanaged[Cdecl]<float, float, float, float, void>)loader("glClearColor");
			DrawArrays = (delegate* unmanaged[Cdecl]<int, int, int, void>)loader("glDrawArrays");
		}
	}
}

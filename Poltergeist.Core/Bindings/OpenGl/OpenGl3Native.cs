namespace Poltergeist.Core.Bindings.OpenGl
{
	public unsafe class OpenGl3Native : OpenGl2Native
	{
		public readonly delegate* unmanaged[Cdecl]<int, uint*, void> GenerateVertexArrays;
		public readonly delegate* unmanaged[Cdecl]<int, uint*, void> DeleteVertexArrays;
		public readonly delegate* unmanaged[Cdecl]<uint, void> BindVertexArray;
	
		public OpenGl3Native(delegate*<string, void*> loader) : base(loader)
		{
			GenerateVertexArrays = (delegate* unmanaged[Cdecl]<int, uint*, void>)loader("glGenVertexArrays");
			DeleteVertexArrays = (delegate* unmanaged[Cdecl]<int, uint*, void>)loader("glDeleteVertexArrays");
			BindVertexArray = (delegate* unmanaged[Cdecl]<uint, void>)loader("glBindVertexArray");
		}
	}
}

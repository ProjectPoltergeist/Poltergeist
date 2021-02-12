namespace Poltergeist.Core.Bindings.OpenGl
{
	public unsafe class OpenGl2Native : OpenGl1Native
	{
		public readonly delegate* unmanaged[Cdecl]<uint, int, int, int, int, void*, void> VertexAttributePointer;
		public readonly delegate* unmanaged[Cdecl]<uint, void> EnableVertexAttributeArray;
		
		public OpenGl2Native(delegate*<string, void*> loader) : base(loader)
		{
			VertexAttributePointer = (delegate* unmanaged[Cdecl]<uint, int, int, int, int, void*, void>)loader("glVertexAttribPointer");
			EnableVertexAttributeArray = (delegate* unmanaged[Cdecl]<uint, void>)loader("glEnableVertexAttribArray");
		}
	}
}

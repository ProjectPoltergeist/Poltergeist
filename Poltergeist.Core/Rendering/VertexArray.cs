using System;
using Poltergeist.Core.Bindings.OpenGl;

namespace Poltergeist.Core.Rendering
{
	public unsafe struct VertexArray : IDisposable
	{
		private uint _vertexArrayId;
		
		private VertexArray(uint vertexArrayId)
		{
			_vertexArrayId = vertexArrayId;
		}

		public static VertexArray Create()
		{
			uint vertexArrayId;
			
			OpenGl3Native.GenerateVertexArrays(1, &vertexArrayId);

			return new VertexArray(vertexArrayId);
		}
		
		public void Bind()
		{
			OpenGl3Native.BindVertexArray(_vertexArrayId);
		}

		public void Unbind()
		{
			OpenGl3Native.BindVertexArray(0);
		}

		public void Dispose()
		{
			fixed (uint* vertexArrayIdPointer = &_vertexArrayId)
				OpenGl3Native.DeleteVertexArrays(1, vertexArrayIdPointer);
		}
	}
}

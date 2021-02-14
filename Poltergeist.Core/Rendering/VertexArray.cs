using System;
using Poltergeist.Core.Bindings.OpenGl;

namespace Poltergeist.Core.Rendering
{
	public readonly struct VertexArray : IDisposable
	{
		private readonly uint _vertexArrayId;
		
		private VertexArray(uint vertexArrayId)
		{
			_vertexArrayId = vertexArrayId;
		}

		public static unsafe VertexArray Create()
		{
			uint vertexArrayId;
			
			OpenGl3Native.GenerateVertexArrays(1, &vertexArrayId);

			return new VertexArray(vertexArrayId);
		}
		
		public unsafe void Bind()
		{
			OpenGl3Native.BindVertexArray(_vertexArrayId);
		}

		public unsafe void Unbind()
		{
			OpenGl3Native.BindVertexArray(0);
		}

		public unsafe void Dispose()
		{
			fixed (uint* vertexArrayIdPointer = &_vertexArrayId)
			{
				OpenGl3Native.DeleteVertexArrays(1, vertexArrayIdPointer);
			}
		}
	}
}

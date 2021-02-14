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

		public static VertexArray Create()
		{
			uint vertexArrayId;
			
			unsafe
			{
				OpenGl3Native.GenerateVertexArrays(1, &vertexArrayId);
			}

			return new VertexArray(vertexArrayId);
		}
		
		public void Bind()
		{
			unsafe
			{
				OpenGl3Native.BindVertexArray(_vertexArrayId);
			}
		}

		public void Unbind()
		{
			unsafe
			{
				OpenGl3Native.BindVertexArray(0);
			}
		}

		public void Dispose()
		{
			unsafe
			{
				fixed (uint* vertexArrayIdPointer = &_vertexArrayId)
				{
					OpenGl3Native.DeleteVertexArrays(1, vertexArrayIdPointer);
				}
			}
		}
	}
}
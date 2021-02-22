using System;
using OpenTK.Graphics.ES30;

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
			GL.GenVertexArrays(1, &vertexArrayId);
			return new VertexArray(vertexArrayId);
		}

		public void Bind()
		{
			GL.BindVertexArray(_vertexArrayId);
		}

		public void Unbind()
		{
			GL.BindVertexArray(0);
		}

		public void Dispose()
		{
			fixed (uint* vertexArrayIdPointer = &_vertexArrayId)
				GL.DeleteVertexArrays(1, vertexArrayIdPointer);
		}
	}
}

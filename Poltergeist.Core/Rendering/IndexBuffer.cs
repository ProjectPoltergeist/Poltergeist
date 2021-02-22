using System;
using OpenTK.Graphics.ES20;

namespace Poltergeist.Core.Rendering
{
	public unsafe struct IndexBuffer : IDisposable
	{
		private uint _indexBufferId;

		private IndexBuffer(uint indexBufferId)
		{
			_indexBufferId = indexBufferId;
		}

		public static IndexBuffer Create<T>(T[] indices) where T : unmanaged
		{
			var indexBufferId = 0u;
			GL.GenBuffers(1, &indexBufferId);

			var indexBuffer = new IndexBuffer(indexBufferId);
			indexBuffer.Bind();

			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(T), indices, BufferUsageHint.StaticDraw);

			indexBuffer.Unbind();
			return indexBuffer;
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBufferId);
		}

		public void Unbind()
		{
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}

		public void Dispose()
		{
			fixed (uint* indexBufferIdPointer = &_indexBufferId)
				GL.DeleteBuffers(1, indexBufferIdPointer);
		}
	}
}

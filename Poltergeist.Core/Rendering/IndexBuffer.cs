using System;
using Poltergeist.Core.Bindings.OpenGl;

namespace Poltergeist.Core.Rendering
{
	public unsafe struct IndexBuffer : IDisposable
	{
		private const int glElementArrayBuffer = 0x8893;
		private const int glStaticDraw = 0x88E4;
		
		private uint _indexBufferId;

		private IndexBuffer(uint indexBufferId)
		{
			_indexBufferId = indexBufferId;
		}
		
		public static IndexBuffer Create<T>(ReadOnlySpan<T> indices) where T : unmanaged
		{
			uint indexBufferId;

			OpenGl3Native.GenerateBuffers(1, &indexBufferId);

			var indexBuffer = new IndexBuffer(indexBufferId);

			indexBuffer.Bind();
			
			fixed (T* dataPointer = indices)
				OpenGl3Native.BufferData(glElementArrayBuffer, indices.Length * sizeof(T), dataPointer, glStaticDraw);

			indexBuffer.Unbind();

			return indexBuffer;
		}
		
		public void Bind()
		{
			OpenGl3Native.BindBuffer(glElementArrayBuffer, _indexBufferId);
		}

		public void Unbind()
		{
			OpenGl3Native.BindBuffer(glElementArrayBuffer, 0);
		}
		
		public void Dispose()
		{
			fixed (uint* indexBufferIdPointer = &_indexBufferId)
				OpenGl3Native.DeleteBuffers(1, indexBufferIdPointer);
		}
	}
}
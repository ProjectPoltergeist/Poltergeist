using System;
using Poltergeist.Core.Bindings.OpenGl;

namespace Poltergeist.Core.Rendering
{
	public unsafe struct IndexBuffer : IDisposable
	{
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
				OpenGl3Native.BufferData(OpenGlBufferType.ElementArray, indices.Length * sizeof(T), dataPointer, OpenGlUsageHint.StaticDraw);

			indexBuffer.Unbind();

			return indexBuffer;
		}
		
		public void Bind()
		{
			OpenGl3Native.BindBuffer(OpenGlBufferType.ElementArray, _indexBufferId);
		}

		public void Unbind()
		{
			OpenGl3Native.BindBuffer(OpenGlBufferType.ElementArray, 0);
		}
		
		public void Dispose()
		{
			fixed (uint* indexBufferIdPointer = &_indexBufferId)
				OpenGl3Native.DeleteBuffers(1, indexBufferIdPointer);
		}
	}
}

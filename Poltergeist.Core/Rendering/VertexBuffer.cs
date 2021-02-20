using System;
using Poltergeist.Core.Bindings.OpenGl;

namespace Poltergeist.Core.Rendering
{
	public unsafe struct VertexBuffer : IDisposable
	{
		private uint _vertexBufferId;

		private VertexBuffer(uint vertexBufferId)
		{
			_vertexBufferId = vertexBufferId;
		}

		public static VertexBuffer Create<T>(ReadOnlySpan<T> data, ReadOnlySpan<VertexBufferElement> layout) where T : unmanaged
		{
			uint vertexBufferId;

			OpenGl3Native.GenerateBuffers(1, &vertexBufferId);

			var vertexBuffer = new VertexBuffer(vertexBufferId);

			vertexBuffer.Bind();

			fixed (T* dataPointer = data)
				OpenGl3Native.BufferData(OpenGlBufferType.Array, data.Length * sizeof(T), dataPointer, OpenGlUsageHint.StaticDraw);

			int stride = 0;

			for (uint i = 0; i < layout.Length; i++)
			{
				var element = layout[(int)i];
				
				stride += element.Count * OpenGlTypeUtils.SizeOf(element.Type);
			}

			nuint offset = 0;

			for (uint i = 0; i < layout.Length; i++)
			{
				var element = layout[(int)i];
				var size = element.Count * OpenGlTypeUtils.SizeOf(element.Type);

				OpenGl3Native.VertexAttributePointer(i, element.Count, element.Type, false, stride, offset);
				OpenGl3Native.EnableVertexAttributeArray(i);

				offset += (nuint)size;
			}

			vertexBuffer.Unbind();

			return vertexBuffer;
		}

		public void Bind()
		{
			OpenGl3Native.BindBuffer(OpenGlBufferType.Array, _vertexBufferId);
		}

		public void Unbind()
		{
			OpenGl3Native.BindBuffer(OpenGlBufferType.Array, 0);
		}

		public void Dispose()
		{
			fixed (uint* vertexBufferIdPointer = &_vertexBufferId)
				OpenGl3Native.DeleteBuffers(1, vertexBufferIdPointer);
		}
	}
}

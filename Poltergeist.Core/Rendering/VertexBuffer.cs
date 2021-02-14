using System;
using Poltergeist.Core.Bindings.OpenGl;

namespace Poltergeist.Core.Rendering
{
	public readonly struct VertexBuffer : IDisposable
	{
		private readonly uint _vertexBufferId;
		
		private const int glArrayBuffer = 0x8892;
		private const int glStaticDraw = 0x88E4;
		private const int glFalse = 0;
		
		private VertexBuffer(uint vertexBufferId)
		{
			_vertexBufferId = vertexBufferId;
		}

		public static unsafe VertexBuffer Create<T>(ReadOnlySpan<T> data, ReadOnlySpan<VertexBufferElement> layout) where T : unmanaged
		{
			uint vertexBufferId;
		
			OpenGl3Native.GenerateBuffers(1, &vertexBufferId);
			
			var vertexBuffer = new VertexBuffer(vertexBufferId);

			vertexBuffer.Bind();

			fixed (T* dataPointer = data)
			{
				OpenGl3Native.BufferData(glArrayBuffer, data.Length * sizeof(T), dataPointer, glStaticDraw);
			}

			nuint offset = 0;
				
			for (uint i = 0; i < layout.Length; i++)
			{
				var element = layout[(int)i];
				var typeSize = element.Type switch
				{
					OpenGlType.Float => sizeof(float),
					_ => throw new ArgumentOutOfRangeException()
				};
				var size = element.Count * typeSize;
					
				OpenGl3Native.VertexAttributePointer(i, element.Count, (int)element.Type, glFalse, size, offset);
				OpenGl3Native.EnableVertexAttributeArray(i);

				offset += (nuint)size;
			}

			vertexBuffer.Unbind();
			
			return vertexBuffer;
		}
		
		public unsafe void Bind()
		{
			OpenGl3Native.BindBuffer(glArrayBuffer, _vertexBufferId);
		}

		public unsafe void Unbind()
		{
			OpenGl3Native.BindBuffer(glArrayBuffer, 0);
		}

		public unsafe void Dispose()
		{
			fixed (uint* vertexBufferIdPointer = &_vertexBufferId)
			{
				OpenGl3Native.DeleteBuffers(1, vertexBufferIdPointer);
			}
		}
	}
}

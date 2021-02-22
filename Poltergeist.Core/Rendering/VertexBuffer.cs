using System;
using OpenTK.Graphics.ES20;

namespace Poltergeist.Core.Rendering
{
	public unsafe struct VertexBuffer : IDisposable
	{
		private uint _vertexBufferId;

		private VertexBuffer(uint vertexBufferId)
		{
			_vertexBufferId = vertexBufferId;
		}

		public static VertexBuffer Create<T>(T[] data, ReadOnlySpan<VertexBufferElement> layout) where T : unmanaged
		{
			var vertexBufferId = 0u;
			GL.GenBuffers(1, &vertexBufferId);

			var vertexBuffer = new VertexBuffer(vertexBufferId);
			vertexBuffer.Bind();

			GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(T), data, BufferUsageHint.StaticDraw);

			var stride = 0;

			for (uint i = 0; i < layout.Length; i++)
			{
				var element = layout[(int)i];
				
				stride += element.Count * element.Size;
			}

			var offset = 0;

			for (int i = 0; i < layout.Length; i++)
			{
				var element = layout[(int)i];
				var size = element.Count * element.Size;

				GL.VertexAttribPointer(i, element.Count, (All)element.Type, false, stride, new(offset)); //??
				GL.EnableVertexAttribArray(i);

				offset += size;
			}

			vertexBuffer.Unbind();
			return vertexBuffer;
		}

		public void Bind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferId);
		}

		public void Unbind()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void Dispose()
		{
			fixed (uint* vertexBufferIdPointer = &_vertexBufferId)
				GL.DeleteBuffers(1, vertexBufferIdPointer);
		}
	}
}

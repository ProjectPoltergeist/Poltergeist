using Poltergeist.Core.Bindings.OpenGl;
using OpenTK.Graphics.ES30;
using System;

namespace Poltergeist.Core.Rendering
{
	public readonly struct VertexBufferElement
	{
		public readonly VertexAttribType Type;
		public readonly int Count;
		public readonly int Size;

		public VertexBufferElement(VertexAttribType type, int count)
		{
			Type = type;
			Count = count;
			Size = 0;
			Size = SizeOf();
		}

		private int SizeOf()
		{
			return Type switch
			{
				VertexAttribType.UnsignedByte => sizeof(byte),
				VertexAttribType.Short => sizeof(short),
				VertexAttribType.UnsignedShort => sizeof(ushort),
				VertexAttribType.Int => sizeof(int),
				VertexAttribType.UnsignedInt => sizeof(uint),
				VertexAttribType.Float => sizeof(float),
				VertexAttribType.Double => sizeof(double),
				_ => throw new NotImplementedException()
			};
		}
	}
}

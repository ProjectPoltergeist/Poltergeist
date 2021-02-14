using Poltergeist.Core.Bindings.OpenGl;

namespace Poltergeist.Core.Rendering
{
	public readonly struct VertexBufferElement
	{
		public readonly OpenGlType Type;
		public readonly int Count;
		
		public VertexBufferElement(OpenGlType type, int count)
		{
			Type = type;
			Count = count;
		}
	}
}

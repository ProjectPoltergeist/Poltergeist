using System;
using Poltergeist.Core.Bindings.Glfw;

namespace Poltergeist.Core.Bindings.OpenGl
{
	public abstract unsafe class OpenGl3Native : OpenGl2Native
	{
		public new static readonly bool IsSupported = OpenGl2Native.IsSupported && GetVersion().major >= 3;

		private static readonly delegate* unmanaged[Cdecl]<int, uint*, void> _generateVertexArrays
			= (delegate* unmanaged[Cdecl]<int, uint*, void>)GlfwNative.GetProcessAddress("glGenVertexArrays");

		private static readonly delegate* unmanaged[Cdecl]<int, uint*, void> _deleteVertexArrays
			= (delegate* unmanaged[Cdecl]<int, uint*, void>)GlfwNative.GetProcessAddress("glDeleteVertexArrays");

		private static readonly delegate* unmanaged[Cdecl]<uint, void> _bindVertexArray
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glBindVertexArray");

		public static void GenerateVertexArrays(int count, uint* vertexArrayIds)
		{
			if (_generateVertexArrays == null)
			{
				Console.WriteLine($"[{nameof(GenerateVertexArrays)}]: Function is not supported.");
				return;
			}
			
			if (count <= 0)
			{
				Console.WriteLine($"[{nameof(GenerateVertexArrays)}]: Count must be greater than 0.");
				return;
			}

			if (vertexArrayIds == null)
			{
				Console.WriteLine($"[{nameof(GenerateVertexArrays)}]: VertexArrayIds must not be null.");
				return;
			}

			try
			{
				_ = *vertexArrayIds;
			}
			catch (AccessViolationException)
			{
				Console.WriteLine($"[{nameof(GenerateVertexArrays)}]: VertexArrayIds must point to a valid memory region.");
				return;
			}

			_generateVertexArrays(count, vertexArrayIds);
			
			HandleOpenGlErrors(nameof(GenerateVertexArrays));
		}

		public static void DeleteVertexArrays(int count, uint* vertexArrayIds)
		{
			if (_deleteVertexArrays == null)
			{
				Console.WriteLine($"[{nameof(DeleteVertexArrays)}]: Function is not supported.");
				return;
			}
			
			if (count <= 0)
			{
				Console.WriteLine($"[{nameof(DeleteVertexArrays)}]: Count must be greater than 0.");
				return;
			}

			if (vertexArrayIds == null)
			{
				Console.WriteLine($"[{nameof(DeleteVertexArrays)}]: VertexArrayIds must not be null.");
				return;
			}

			try
			{
				_ = *vertexArrayIds;
			}
			catch (AccessViolationException)
			{
				Console.WriteLine("[DeleteVertexArrays]: VertexArrayIds must point to a valid memory region.");
				return;
			}
			
			_deleteVertexArrays(count, vertexArrayIds);
			
			HandleOpenGlErrors("DeleteVertexArrays");
		}

		public static void BindVertexArray(uint vertexArrayId)
		{
			if (_bindVertexArray == null)
			{
				Console.WriteLine("[BindVertexArray]: Function is not supported.");
				return;
			}

			_bindVertexArray(vertexArrayId);
			
			HandleOpenGlErrors("BindVertexArray");
		}
	}
}

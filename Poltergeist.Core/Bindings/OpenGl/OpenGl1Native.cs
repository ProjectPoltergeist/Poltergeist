using System;
using Poltergeist.Core.Bindings.Glfw;

namespace Poltergeist.Core.Bindings.OpenGl
{
	public abstract unsafe class OpenGl1Native
	{
		public static readonly bool IsSupported = true;

		private static readonly delegate* unmanaged[Cdecl]<OpenGlError> _getError = 
			(delegate* unmanaged[Cdecl]<OpenGlError>)GlfwNative.GetProcessAddress("glGetError");

		private static readonly delegate* unmanaged[Cdecl]<int, uint*, void> _generateBuffers =
			(delegate* unmanaged[Cdecl]<int, uint*, void>)GlfwNative.GetProcessAddress("glGenBuffers");

		private static readonly delegate* unmanaged[Cdecl]<int, uint*, void> _deleteBuffers =
			(delegate* unmanaged[Cdecl]<int, uint*, void>)GlfwNative.GetProcessAddress("glDeleteBuffers");

		private static readonly delegate* unmanaged[Cdecl]<OpenGlBufferType, uint, void> _bindBuffer =
			(delegate* unmanaged[Cdecl]<OpenGlBufferType, uint, void>)GlfwNative.GetProcessAddress("glBindBuffer");

		private static readonly delegate* unmanaged[Cdecl]<OpenGlBufferType, long, void*, OpenGlUsageHint, void> _bufferData =
			(delegate* unmanaged[Cdecl]<OpenGlBufferType, long, void*, OpenGlUsageHint, void>)GlfwNative.GetProcessAddress("glBufferData");

		private static readonly delegate* unmanaged[Cdecl]<OpenGlClearMask, void> _clear =
			(delegate* unmanaged[Cdecl]<OpenGlClearMask, void>)GlfwNative.GetProcessAddress("glClear");

		private static readonly delegate* unmanaged[Cdecl]<float, float, float, float, void> _clearColor =
			(delegate* unmanaged[Cdecl]<float, float, float, float, void>)GlfwNative.GetProcessAddress("glClearColor");

		private static readonly delegate* unmanaged[Cdecl]<OpenGlPrimitive, int, int, void> _drawArrays =
			(delegate* unmanaged[Cdecl]<OpenGlPrimitive, int, int, void>)GlfwNative.GetProcessAddress("glDrawArrays");
		
		private static readonly delegate* unmanaged[Cdecl]<OpenGlPrimitive, int, OpenGlType, void*, void> _drawElements =
			(delegate* unmanaged[Cdecl]<OpenGlPrimitive, int, OpenGlType, void*, void>)GlfwNative.GetProcessAddress("glDrawElements");

		public static OpenGlError GetError()
		{
			if (_getError == null)
			{
				Console.WriteLine($"[{nameof(GetError)}]: Function is not supported.");
				return OpenGlError.None;
			}
			
			return _getError();
		}

		public static void GenerateBuffers(int count, uint* bufferIds)
		{
			if (_generateBuffers == null)
			{
				Console.WriteLine($"[{nameof(GenerateBuffers)}]: Function is not supported.");
				return;
			}
			
			if (count <= 0)
			{
				Console.WriteLine($"[{nameof(GenerateBuffers)}]: Count must be greater than 0.");
				return;
			}
			
			if (bufferIds == null)
			{
				Console.WriteLine($"[{nameof(GenerateBuffers)}]: BufferIds must not be null.");
				return;
			}

			try
			{
				_ = *bufferIds;
			}
			catch (AccessViolationException)
			{
				Console.WriteLine($"[{nameof(GenerateBuffers)}]: BufferIds must point to a valid memory region.");
				return;
			}

			_generateBuffers(count, bufferIds);
			
			HandleOpenGlErrors(nameof(GenerateBuffers));
		}

		public static void DeleteBuffers(int count, uint* bufferIds)
		{
			if (_deleteBuffers == null)
			{
				Console.WriteLine($"[{nameof(DeleteBuffers)}]: Function is not supported.");
				return;
			}
			
			if (count <= 0)
			{
				Console.WriteLine($"[{nameof(DeleteBuffers)}]: Count must be greater than 0.");
				return;
			}
			
			if (bufferIds == null)
			{
				Console.WriteLine($"[{nameof(DeleteBuffers)}]: BufferIds must not be null.");
				return;
			}

			try
			{
				_ = *bufferIds;
			}
			catch (AccessViolationException)
			{
				Console.WriteLine($"[{nameof(DeleteBuffers)}]: BufferIds must point to a valid memory region.");
				return;
			}

			_deleteBuffers(count, bufferIds);
			
			HandleOpenGlErrors(nameof(DeleteBuffers));
		}

		public static void BindBuffer(OpenGlBufferType bufferType, uint bufferId)
		{
			if (_bindBuffer == null)
			{
				Console.WriteLine($"[{nameof(BindBuffer)}]: Function is not supported.");
				return;
			}
			
			_bindBuffer(bufferType, bufferId);
			
			HandleOpenGlErrors(nameof(BindBuffer));
		}

		public static void BufferData(OpenGlBufferType bufferType, long size, void* data, OpenGlUsageHint usageHint)
		{
			if (_bufferData == null)
			{
				Console.WriteLine($"[{nameof(BufferData)}]: Function is not supported.");
				return;
			}

			if (size < 0)
			{
				Console.WriteLine($"[{nameof(BufferData)}]: Size must be positive.");
				return;
			}

			_bufferData(bufferType, size, data, usageHint);
			
			HandleOpenGlErrors(nameof(BufferData));
		}

		public static void Clear(OpenGlClearMask clearMask)
		{
			if (_clear == null)
			{
				Console.WriteLine($"[{nameof(Clear)}]: Function is not supported.");
				return;
			}

			_clear(clearMask);

			HandleOpenGlErrors(nameof(Clear));
		}

		public static void ClearColor(float r, float g, float b, float a)
		{
			if (_clearColor == null)
			{
				Console.WriteLine($"[{nameof(ClearColor)}]: Function is not supported.");
				return;
			}

			if (r < 0 || r > 1)
			{
				Console.WriteLine($"[{nameof(ClearColor)}]: Red is out of range.");
				return;
			}

			if (g < 0 || g > 1)
			{
				Console.WriteLine($"[{nameof(ClearColor)}]: Green is out of range.");
				return;
			}

			if (b < 0 || b > 1)
			{
				Console.WriteLine($"[{nameof(ClearColor)}]: Blue is out of range.");
				return;
			}
			
			if (a < 0 || a > 1)
			{
				Console.WriteLine($"[{nameof(ClearColor)}]: Alpha is out of range.");
				return;
			}

			_clearColor(r, g, b, a);
			
			HandleOpenGlErrors(nameof(ClearColor));
		}

		public static void DrawArrays(OpenGlPrimitive primitive, int startIndex, int count)
		{
			if (_drawArrays == null)
			{
				Console.WriteLine($"[{nameof(DrawArrays)}]: Function is not supported.");
				return;
			}

			if (count <= 0)
			{
				Console.WriteLine($"[{nameof(DrawArrays)}]: Count must be greater than 0.");
				return;
			}

			_drawArrays(primitive, startIndex, count);

			HandleOpenGlErrors(nameof(DrawArrays));
		}

		public static void DrawElements(OpenGlPrimitive primitive, int count, OpenGlType indicesType, void* startIndex)
		{
			if (_drawElements == null)
			{
				Console.WriteLine($"[{nameof(DrawElements)}]: Function is not supported.");
				return;
			}

			if (count <= 0)
			{
				Console.WriteLine($"[{nameof(DrawElements)}]: Count must be greater than 0.");
				return;
			}

			switch (indicesType)
			{
				case OpenGlType.UnsignedByte:
				case OpenGlType.UnsignedShort:
				case OpenGlType.UnsignedInt:
					break;
				default:
					Console.WriteLine($"[{nameof(DrawElements)}]: Indices type must be unsigned byte, unsigned short or unsigned int.");
					return;
			}

			_drawElements(primitive, count, indicesType, startIndex);

			HandleOpenGlErrors(nameof(DrawElements));
		}
		
		protected static void HandleOpenGlErrors(string function)
		{
			var error = GetError();
		
			while (error != OpenGlError.None)
			{
				switch (error)
				{
					case OpenGlError.InvalidEnum: Console.WriteLine($"[{function}]: Invalid enum."); break;
					case OpenGlError.InvalidValue: Console.WriteLine($"[{function}]: Invalid value."); break;
					case OpenGlError.InvalidOperation: Console.WriteLine($"[{function}]: Invalid operation."); break;
					case OpenGlError.StackOverflow: Console.WriteLine($"[{function}]: Stack overflow."); break;
					case OpenGlError.StackUnderflow: Console.WriteLine($"[{function}]: Stack underflow."); break;
					case OpenGlError.OutOfMemory: Console.WriteLine($"[{function}]: Out of memory."); break;
					case OpenGlError.InvalidFrameBufferOperation: Console.WriteLine($"[{function}]: Invalid frame buffer operation."); break;
					case OpenGlError.ContextLost: Console.WriteLine($"[{function}]: Context lost."); break;
				}

				error = GetError();
			}
		}
	}
}

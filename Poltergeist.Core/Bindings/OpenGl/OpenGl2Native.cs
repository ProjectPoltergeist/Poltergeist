using System;
using Poltergeist.Core.Bindings.Glfw;

namespace Poltergeist.Core.Bindings.OpenGl
{
	public abstract unsafe class OpenGl2Native : OpenGl1Native
	{
		public new static readonly bool IsSupported;

		private static readonly delegate* unmanaged[Cdecl]<int, int*, void> _getIntegerV;

		private static readonly delegate* unmanaged[Cdecl]<uint, int, OpenGlType, bool, int, nuint, void> _vertexAttributePointer
			= (delegate* unmanaged[Cdecl]<uint, int, OpenGlType, bool, int, nuint, void>)GlfwNative.GetProcessAddress(
				"glVertexAttribPointer");

		private static readonly delegate* unmanaged[Cdecl]<uint, void> _enableVertexAttributeArray
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glEnableVertexAttribArray");

		static OpenGl2Native()
		{
			_getIntegerV = (delegate* unmanaged[Cdecl]<int, int*, void>)GlfwNative.GetProcessAddress("glGetIntegerv");
			IsSupported = OpenGl1Native.IsSupported && _getIntegerV != null;
		}

		public static (int major, int minor) GetVersion()
		{
			int major = 0;
			const int glMajorVersion = 0x821B;
			GetIntegerV(glMajorVersion, &major);
			int minor = 0;
			const int glMinorVersion = 0x821C;
			GetIntegerV(glMinorVersion, &minor);
			return (major, minor);
		}

		public static void GetIntegerV(int parameter, int* results)
		{
			if (_getIntegerV == null)
			{
				Console.WriteLine($"[{nameof(GetIntegerV)}]: Function is not supported.");
				return;
			}
			
			_getIntegerV(parameter, results);
			
			HandleOpenGlErrors(nameof(GetIntegerV));
		}

		public static void VertexAttributePointer(uint index, int size, OpenGlType type, bool normalized, int stride, nuint offset)
		{
			if (_vertexAttributePointer == null)
			{
				Console.WriteLine($"[{nameof(VertexAttributePointer)}]: Function is not supported.");
				return;
			}

			if (size < 1 || size > 4)
			{
				Console.WriteLine($"[{nameof(VertexAttributePointer)}]: Size must be 1, 2, 3 or 4.");
				return;
			}

			switch (type)
			{
				case OpenGlType.UnsignedByte:
				case OpenGlType.Short: 
				case OpenGlType.UnsignedShort:
				case OpenGlType.Int:
				case OpenGlType.UnsignedInt:
				case OpenGlType.Float:
				case OpenGlType.Double:
					break;
				default:
					Console.WriteLine($"[{nameof(VertexAttributePointer)}]: Type must be unsigned byte, short, unsigned short, int, unsigned int, float or double.");
					return;
			}
			
			_vertexAttributePointer(index, size, type, normalized, stride, offset);
			
			HandleOpenGlErrors(nameof(VertexAttributePointer));
		}

		public static void EnableVertexAttributeArray(uint index)
		{
			if (_enableVertexAttributeArray == null)
			{
				Console.WriteLine($"[{nameof(EnableVertexAttributeArray)}]: Function is not supported");
				return;
			}

			_enableVertexAttributeArray(index);
			
			HandleOpenGlErrors(nameof(EnableVertexAttributeArray));
		}
	}
}

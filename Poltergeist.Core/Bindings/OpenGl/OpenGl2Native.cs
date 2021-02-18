using System;
using Poltergeist.Core.Bindings.Glfw;
using Poltergeist.Core.Memory;

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

		private static readonly delegate* unmanaged[Cdecl]<uint> _createProgram
			= (delegate* unmanaged[Cdecl]<uint>)GlfwNative.GetProcessAddress("glCreateProgram");

		private static readonly delegate* unmanaged[Cdecl]<uint, void> _deleteProgram
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glDeleteProgram");

		private static readonly delegate* unmanaged[Cdecl]<uint, void> _useProgram
			= (delegate* unmanaged[Cdecl]<uint, void>) GlfwNative.GetProcessAddress("glUseProgram");

		private static readonly delegate* unmanaged[Cdecl]<uint, void> _linkProgram
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glLinkProgram");

		private static readonly delegate* unmanaged[Cdecl]<uint, OpenGlParameter, int*, void> _getProgramIntegerValue
			= (delegate* unmanaged[Cdecl]<uint, OpenGlParameter, int*, void>) GlfwNative.GetProcessAddress(
				"glGetProgramiv");

		private static readonly delegate* unmanaged[Cdecl]<uint, long, long*, byte*, void> _getProgramInfoLog
			= (delegate* unmanaged[Cdecl]<uint, long, long*, byte*, void>) GlfwNative.GetProcessAddress(
				"glGetProgramInfoLog");

		private static readonly delegate* unmanaged[Cdecl]<OpenGlShaderType, uint> _createShader
			= (delegate* unmanaged[Cdecl]<OpenGlShaderType, uint>)GlfwNative.GetProcessAddress("glCreateShader");

		private static readonly delegate* unmanaged[Cdecl]<uint, void> _deleteShader
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glDeleteShader");

		private static readonly delegate* unmanaged[Cdecl]<uint, uint, void> _attachShader
			= (delegate* unmanaged[Cdecl]<uint, uint, void>)GlfwNative.GetProcessAddress("glAttachShader");

		private static readonly delegate* unmanaged[Cdecl]<uint, uint, void> _detachShader
			= (delegate* unmanaged[Cdecl]<uint, uint, void>)GlfwNative.GetProcessAddress("glDetachShader");

		private static readonly delegate* unmanaged[Cdecl]<uint, long, byte**, int*, void> _shaderSource
			= (delegate* unmanaged[Cdecl]<uint, long, byte**, int*, void>)GlfwNative.GetProcessAddress(
				"glShaderSource");

		private static readonly delegate* unmanaged[Cdecl]<uint, void> _compileShader
			= (delegate* unmanaged[Cdecl]<uint, void>)GlfwNative.GetProcessAddress("glCompileShader");

		private static readonly delegate* unmanaged[Cdecl]<uint, OpenGlParameter, int*, void> _getShaderIntegerValue
			= (delegate* unmanaged[Cdecl]<uint, OpenGlParameter, int*, void>) GlfwNative.GetProcessAddress(
				"glGetShaderiv");

		private static readonly delegate* unmanaged[Cdecl]<uint, long, long*, byte*, void> _getShaderInfoLog
			= (delegate* unmanaged[Cdecl]<uint, long, long*, byte*, void>)GlfwNative.GetProcessAddress(
				"glGetShaderInfoLog");

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

		public static uint CreateProgram()
		{
			if (_createProgram == null)
			{
				Console.WriteLine($"[{nameof(CreateProgram)}]: Function is not supported.");
				return 0;
			}

			var programId = _createProgram();

			HandleOpenGlErrors(nameof(CreateProgram));

			return programId;
		}

		public static void DeleteProgram(uint programId)
		{
			if (_deleteProgram == null)
			{
				Console.WriteLine($"[{nameof(DeleteProgram)}]: Function is not supported.");
				return;
			}

			_deleteProgram(programId);

			HandleOpenGlErrors(nameof(DeleteProgram));
		}

		public static void UseProgram(uint programId)
		{
			if (_useProgram == null)
			{
				Console.WriteLine($"[{nameof(UseProgram)}]: Function is not supported.");
				return;
			}

			_useProgram(programId);

			HandleOpenGlErrors(nameof(UseProgram));
		}

		public static void LinkProgram(uint programId)
		{
			if (_linkProgram == null)
			{
				Console.WriteLine($"[{nameof(LinkProgram)}]: Function is not supported.");
				return;
			}

			_linkProgram(programId);

			HandleOpenGlErrors(nameof(LinkProgram));
		}

		public static void GetProgramIntegerValue(uint programId, OpenGlParameter parameter, int* value)
		{
			if (_getProgramIntegerValue == null)
			{
				Console.WriteLine($"[{nameof(GetProgramIntegerValue)}]: Function is not supported.");
				return;
			}

			_getProgramIntegerValue(programId, parameter, value);

			HandleOpenGlErrors(nameof(GetProgramIntegerValue));
		}

		public static void GetProgramInfoLog(uint programId, long maxLength, long* length, byte* infoLog)
		{
			if (_getProgramInfoLog == null)
			{
				Console.WriteLine($"[{nameof(GetProgramInfoLog)}]: Function is not supported.");
				return;
			}

			if (infoLog == null)
			{
				Console.WriteLine($"[{nameof(GetProgramInfoLog)}]: Info log must not be null.");
			}

			if (!PointerUtils.IsReadable(infoLog))
			{
				Console.WriteLine($"[{nameof(GetProgramInfoLog)}]: Info log must point to a valid memory region.");
				return;
			}

			_getProgramInfoLog(programId, maxLength, length, infoLog);

			HandleOpenGlErrors(nameof(GetProgramInfoLog));
		}

		public static uint CreateShader(OpenGlShaderType shaderType)
		{
			if (_createShader == null)
			{
				Console.WriteLine($"[{nameof(CreateShader)}]: Function is not supported.");
				return 0;
			}

			var shaderId = _createShader(shaderType);

			HandleOpenGlErrors(nameof(CreateShader));

			return shaderId;
		}

		public static void DeleteShader(uint shaderId)
		{
			if (_deleteShader == null)
			{
				Console.WriteLine($"[{nameof(DeleteShader)}]: Function is not supported.");
				return;
			}

			_deleteShader(shaderId);

			HandleOpenGlErrors(nameof(DeleteShader));
		}

		public static void AttachShader(uint programId, uint shaderId)
		{
			if (_attachShader == null)
			{
				Console.WriteLine($"[{nameof(AttachShader)}]: Function is not supported.");
				return;
			}

			_attachShader(programId, shaderId);

			HandleOpenGlErrors(nameof(AttachShader));
		}

		public static void DetachShader(uint programId, uint shaderId)
		{
			if (_detachShader == null)
			{
				Console.WriteLine($"[{nameof(DetachShader)}]: Function is not supported.");
				return;
			}

			_detachShader(programId, shaderId);

			HandleOpenGlErrors(nameof(DetachShader));
		}

		public static void ShaderSource(uint shaderId, long count, byte** source, int* lengths)
		{
			if (_shaderSource == null)
			{
				Console.WriteLine($"[{nameof(ShaderSource)}]: Function is not supported.");
				return;
			}

			if (count <= 0)
			{
				Console.WriteLine($"[{nameof(ShaderSource)}]: Count must be greater than 0.");
				return;
			}

			if (source == null)
			{
				Console.WriteLine($"[{nameof(ShaderSource)}]: Source must not be null.");
				return;
			}

			if (!PointerUtils.IsReadable(source))
			{
				Console.WriteLine($"[{nameof(ShaderSource)}]: Source must point to a valid memory region.");
				return;
			}

			if (lengths != null && !PointerUtils.IsReadable(lengths))
			{
				Console.WriteLine($"[{nameof(ShaderSource)}]: Lengths must point to a valid memory region.");
				return;
			}

			for (long i = 0; i < count; i++)
			{
				if (!PointerUtils.IsReadable(source + i))
				{
					Console.WriteLine($"[{nameof(ShaderSource)}]: Source doesn't contain enough elements.");
					return;
				}

				if (lengths != null && !PointerUtils.IsReadable(lengths + i))
				{
					Console.WriteLine($"[{nameof(ShaderSource)}]: Lengths don't contain enough elements.");
					return;
				}
			}

			_shaderSource(shaderId, count, source, lengths);

			HandleOpenGlErrors(nameof(ShaderSource));
		}

		public static void CompileShader(uint shaderId)
		{
			if (_compileShader == null)
			{
				Console.WriteLine($"[{nameof(CompileShader)}]: Function is not supported.");
				return;
			}

			_compileShader(shaderId);

			HandleOpenGlErrors(nameof(CompileShader));
		}

		public static void GetShaderIntegerValue(uint shaderId, OpenGlParameter parameter, int* value)
		{
			if (_getShaderIntegerValue == null)
			{
				Console.WriteLine($"[{nameof(GetShaderIntegerValue)}]: Function is not supported.");
				return;
			}

			_getShaderIntegerValue(shaderId, parameter, value);

			HandleOpenGlErrors(nameof(GetShaderIntegerValue));
		}

		public static void GetShaderInfoLog(uint shaderId, long maxLength, long* length, byte* infoLog)
		{
			if (_getShaderInfoLog == null)
			{
				Console.WriteLine($"[{nameof(GetShaderInfoLog)}]: Function is not supported.");
				return;
			}

			if (infoLog == null)
			{
				Console.WriteLine($"[{nameof(GetShaderInfoLog)}]: Info log must not be null.");
			}

			if (!PointerUtils.IsReadable(infoLog))
			{
				Console.WriteLine($"[{nameof(GetShaderInfoLog)}]: Info log must point to a valid memory region.");
				return;
			}

			_getShaderInfoLog(shaderId, maxLength, length, infoLog);

			HandleOpenGlErrors(nameof(GetShaderInfoLog));
		}
	}
}

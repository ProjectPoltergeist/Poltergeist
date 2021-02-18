using System;
using System.Buffers;
using System.Text;
using Poltergeist.Core.Bindings.OpenGl;

namespace Poltergeist.Core.Rendering
{
	public readonly unsafe struct Shader : IDisposable
	{
		private readonly uint _shaderId;

		private Shader(uint shaderId)
		{
			_shaderId = shaderId;
		}

		public static Shader Create(string fragmentShaderSource, string vertexShaderSource)
		{
			uint programId = OpenGl3Native.CreateProgram();
			uint fragmentShaderId = CompileShader(OpenGlShaderType.Fragment, fragmentShaderSource);
			uint vertexShaderId = CompileShader(OpenGlShaderType.Vertex, vertexShaderSource);

			OpenGl3Native.AttachShader(programId, fragmentShaderId);
			OpenGl3Native.AttachShader(programId, vertexShaderId);
			OpenGl3Native.LinkProgram(programId);
			OpenGl3Native.DetachShader(programId, fragmentShaderId);
			OpenGl3Native.DetachShader(programId, vertexShaderId);
			OpenGl3Native.DeleteShader(fragmentShaderId);
			OpenGl3Native.DeleteShader(vertexShaderId);

			int success;
			OpenGl3Native.GetProgramIntegerValue(programId, OpenGlParameter.LinkStatus, &success);

			if (success == 0)
			{
				byte* buffer = stackalloc byte[1024];
				long length;

				OpenGl3Native.GetProgramInfoLog(programId, 1024, &length, buffer);

				string infoLog = Encoding.UTF8.GetString(buffer, (int)length);

				Console.WriteLine($"[{nameof(Shader)}::{nameof(Create)}]: Failed to link.\n{infoLog}");
			}

			return new Shader(programId);
		}

		private static uint CompileShader(OpenGlShaderType shaderType, string source)
		{
			uint shaderId = OpenGl3Native.CreateShader(shaderType);

			byte[] sourceBytes = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(source.Length));
			try
			{
				int length = Encoding.UTF8.GetBytes(source, sourceBytes);
				fixed (byte* sourceBytesPointer = sourceBytes)
					OpenGl3Native.ShaderSource(shaderId, 1, &sourceBytesPointer, &length);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(sourceBytes);
			}

			OpenGl3Native.CompileShader(shaderId);

			int success = 0;
			OpenGl3Native.GetShaderIntegerValue(shaderId, OpenGlParameter.CompileStatus, &success);

			if (success == 0)
			{
				byte* buffer = stackalloc byte[1024];
				long length;

				OpenGl3Native.GetShaderInfoLog(shaderId, 1024, &length, buffer);

				string infoLog = Encoding.UTF8.GetString(buffer, (int)length);

				Console.WriteLine($"[{nameof(Shader)}::{nameof(CompileShader)}]: Failed to compile.\n{infoLog}");
			}

			return shaderId;
		}

		public void Bind()
		{
			OpenGl3Native.UseProgram(_shaderId);
		}

		public void Unbind()
		{
			OpenGl3Native.UseProgram(0);
		}

		public void Dispose()
		{
			OpenGl3Native.DeleteProgram(_shaderId);
		}
	}
}

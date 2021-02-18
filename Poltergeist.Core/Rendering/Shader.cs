using System;
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
				byte[] infoLogBuffer = new byte[512];
				long length;
				
				fixed (byte* infoLogPointer = infoLogBuffer)
					OpenGl3Native.GetProgramInfoLog(programId, 512, &length, infoLogPointer);

				string infoLog = Encoding.UTF8.GetString(infoLogBuffer, 0, (int)length);

				Console.WriteLine($"[{nameof(Shader)}::{nameof(Create)}]: Failed to link.");
				Console.WriteLine(infoLog);
			}
			
			return new Shader(programId);
		}

		private static uint CompileShader(OpenGlShaderType shaderType, string source)
		{
			uint shaderId = OpenGl3Native.CreateShader(shaderType);

			byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
			int[] lengths = { sourceBytes.Length };
			
			fixed (byte* sourceBytesPointer = sourceBytes)
			fixed (int* lengthsPointer = lengths)
			{
				byte** sources = stackalloc byte*[1] { sourceBytesPointer };

				OpenGl3Native.ShaderSource(shaderId, 1, sources, lengthsPointer);
			}

			OpenGl3Native.CompileShader(shaderId);

			int success = 0;
			OpenGl3Native.GetShaderIntegerValue(shaderId, OpenGlParameter.CompileStatus, &success);

			if (success == 0)
			{
				byte[] infoLogBuffer = new byte[512];
				long length;
				
				fixed (byte* infoLogPointer = infoLogBuffer)
					OpenGl3Native.GetShaderInfoLog(shaderId, 512, &length, infoLogPointer);

				string infoLog = Encoding.UTF8.GetString(infoLogBuffer, 0, (int)length);

				Console.WriteLine($"[{nameof(Shader)}::{nameof(CompileShader)}]: Failed to compile.");
				Console.WriteLine(infoLog);
			}

			return shaderId;
		}

		public void Bind()
		{
			OpenGl3Native.UseProgram(_shaderId);
		}

		public void Unbind()
		{
			OpenGl3Native.UseProgram(_shaderId);
		}

		public void Dispose()
		{
			OpenGl3Native.DeleteProgram(_shaderId);
		}
	}
}
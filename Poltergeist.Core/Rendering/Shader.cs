using System;
using OpenTK.Graphics.ES30;

namespace Poltergeist.Core.Rendering
{
	public readonly unsafe struct Shader : IDisposable
	{
		private readonly int _shaderId;

		private Shader(int shaderId)
		{
			_shaderId = shaderId;
		}

		public static Shader Create(string fragmentShaderSource, string vertexShaderSource)
		{
			var vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vertexShaderSource);
			GL.CompileShader(vertexShader);

			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fragmentShaderSource);
			GL.CompileShader(fragmentShader);

			var programId = GL.CreateProgram();
			GL.AttachShader(programId, vertexShader);
			GL.AttachShader(programId, fragmentShader);
			GL.LinkProgram(programId);

			GL.DetachShader(programId, vertexShader);
			GL.DetachShader(programId, fragmentShader);
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);

			return new Shader(programId);
		}

		private static int CompileShader(ShaderType shaderType, string source)
		{
			var shaderId = GL.CreateShader(shaderType);
			GL.ShaderSource(shaderId, source);
			GL.CompileShader(shaderId);

			return shaderId;
		}

		public void Bind()
		{
			GL.UseProgram(_shaderId);
		}

		public void Unbind()
		{
			GL.UseProgram(0);
		}

		public void SetUniform(string name, float value)
		{
			var uniformLocation = GL.GetUniformLocation(_shaderId, name);
			GL.Uniform1(uniformLocation, value);
		}

		public void Dispose()
		{
			GL.DeleteProgram(_shaderId);
		}
	}
}

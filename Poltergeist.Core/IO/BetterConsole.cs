using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Poltergeist.Core.IO
{
	public static unsafe class BetterConsole
	{
		private static readonly ConcurrentQueue<string> Lines = new();
		private static readonly StringBuilder Buffer = new(16384);
		private static readonly AutoResetEvent Wait = new(false);
		private static readonly Thread WriteThread;
		private static readonly IntPtr OutHandle;

		private static bool _exit;
		private static bool _paused;

		static BetterConsole()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !Console.IsOutputRedirected)
				OutHandle = GetStdHandle(-11);
			ExitHandler.Exit += () =>
			{
				_exit = true;
				if (WriteThread == null || !WriteThread.IsAlive)
					return;
				Wait.Set();
				WriteThread.Join();
			};
			WriteThread = new Thread(WriteLoop)
			{
				IsBackground = true
			};
			WriteThread.Start();
		}

		public static void WriteLine(string line)
		{
			Lines.Enqueue(line);
		}

		public static void Flush()
		{
			Wait.Set();
		}

		public static void BeginConsoleUpdate()
		{
			_paused = true;
		}

		public static void EndConsoleUpdate()
		{
			_paused = false;
		}

		private static void WriteLoop()
		{
			while (true)
			{
				Wait.WaitOne(100);

				if (_paused && !_exit)
					continue;

				while (Lines.TryDequeue(out string line))
					Buffer.AppendLine(line);

				if (Buffer.Length == 0)
				{
					if (_exit)
						return;
					continue;
				}

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && OutHandle != IntPtr.Zero)
				{
					foreach (ReadOnlyMemory<char> chunk in Buffer.GetChunks())
					{
						ReadOnlySpan<char> span = chunk.Span;
						fixed (char* ptr = span)
							WriteConsole(OutHandle, ptr, (uint)span.Length, out _);
					}
				}
				else
				{
					Console.Write(Buffer.ToString());
				}

				Buffer.Clear();

				if (_exit)
					return;
			}
		}

		[DllImport("Kernel32", EntryPoint = "GetStdHandle", CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
		private static extern IntPtr GetStdHandle(int handle);

		[DllImport("Kernel32", EntryPoint = "WriteConsoleW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
		private static extern bool WriteConsole(IntPtr handle, char* buffer, uint count, out uint written,
												void* reserved = null);
	}
}

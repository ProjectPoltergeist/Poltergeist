namespace Poltergeist.Core.Bindings.OpenGl
{
    public sealed unsafe class OpenGlNative
    {
        public readonly delegate* unmanaged[Cdecl] <int, uint*, void> GenerateVertexArrays;
        public readonly delegate* unmanaged[Cdecl] <uint, void> BindVertexArray;
        public readonly delegate* unmanaged[Cdecl] <int, uint*, void> GenerateBuffers;
        public readonly delegate* unmanaged[Cdecl] <int, uint, void> BindBuffer;
        public readonly delegate* unmanaged[Cdecl] <int, long, void*, int, void> BufferData;
        public readonly delegate* unmanaged[Cdecl] <uint, int, int, int, int, void*, void> VertexAttributePointer;
        public readonly delegate* unmanaged[Cdecl] <uint, void> EnableVertexAttributeArray;
        public readonly delegate* unmanaged[Cdecl] <float, float, float, float, void> ClearColor;
        public readonly delegate* unmanaged[Cdecl] <int, void> Clear;
        public readonly delegate* unmanaged[Cdecl] <int, int, int, void> DrawArrays;
        public readonly delegate* unmanaged[Cdecl] <int, uint*, void> DeleteVertexArrays;
        public readonly delegate* unmanaged[Cdecl] <int, uint*, void> DeleteBuffers;

        public OpenGlNative(delegate* <string, void*> loader)
        {
            GenerateVertexArrays = (delegate* unmanaged[Cdecl] <int, uint*, void>) loader("glGenVertexArrays");
            BindVertexArray = (delegate* unmanaged[Cdecl] <uint, void>) loader("glBindVertexArray");
            GenerateBuffers = (delegate* unmanaged[Cdecl] <int, uint*, void>) loader("glGenBuffers"); 
            BindBuffer = (delegate* unmanaged[Cdecl] <int, uint, void>) loader("glBindBuffer");
            BufferData = (delegate* unmanaged[Cdecl] <int, long, void*, int, void>) loader("glBufferData"); 
            VertexAttributePointer =  (delegate* unmanaged[Cdecl] <uint, int, int, int, int, void*, void>) loader("glVertexAttribPointer"); 
            EnableVertexAttributeArray = (delegate* unmanaged[Cdecl] <uint, void>) loader("glEnableVertexAttribArray");
            ClearColor = (delegate* unmanaged[Cdecl] <float, float, float, float, void>) loader("glClearColor");
            Clear = (delegate* unmanaged[Cdecl] <int, void>) loader("glClear");
            DrawArrays = (delegate* unmanaged[Cdecl] <int, int, int, void>) loader("glDrawArrays");
            DeleteVertexArrays = (delegate* unmanaged[Cdecl] <int, uint*, void>) loader("glDeleteVertexArrays");
            DeleteBuffers = (delegate* unmanaged[Cdecl] <int, uint*, void>) loader("glDeleteBuffers");
        }
    }
}
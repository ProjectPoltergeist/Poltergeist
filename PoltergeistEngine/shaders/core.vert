#version 330 core

layout (location = 0) in vec2 position;
layout (location = 1) in vec4 color;
layout (location = 2) in vec2 texture_coordinates;

out vec4 v_Color;
out vec2 v_TextureCoordinates;

void main()
{
    gl_Position = vec4(position, 1.0, 1.0);
    v_Color = color;
    v_TextureCoordinates = texture_coordinates;
}

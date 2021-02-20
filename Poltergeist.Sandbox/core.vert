#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texture_coordinates;

out vec2 v_TextureCoordinates;

void main()
{
    gl_Position = vec4(position, 1.0);
    v_TextureCoordinates = texture_coordinates;
}

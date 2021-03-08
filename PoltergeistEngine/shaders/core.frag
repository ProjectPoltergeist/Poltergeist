#version 330 core

layout(location = 0) out vec4 color;

in vec4 v_Color;
in vec2 v_TextureCoordinates;

uniform sampler2D u_Texture;

void main()
{
	color = v_Color * texture(u_Texture, v_TextureCoordinates);
}

#include <array>
#include "Renderer.hpp"
#include "VertexArray.hpp"
#include "VertexBuffer.hpp"
#include "VertexBufferLayout.hpp"
#include "IndexBuffer.hpp"

std::shared_ptr<Renderer> Renderer::Create()
{
	std::shared_ptr<Renderer> renderer(new Renderer());

	renderer->m_coreShader = Shader::Create("core.vert", "core.frag");
	renderer->m_whiteTexture = Texture::Create("white.png", 0);

	return renderer;
}

void Renderer::Clear(glm::vec3 color) const noexcept
{
	Clear(glm::vec4(color, 1.0));
}

void Renderer::Clear(glm::vec4 color) const noexcept
{
	glClear(GL_COLOR_BUFFER_BIT);
	glClearColor(color.x, color.y, color.z, color.w);
}

void Renderer::DrawQuad(glm::vec2 position, float rotation, glm::vec2 scale, glm::vec3 color) const noexcept
{
	DrawQuad(position, rotation, scale, glm::vec4(color, 1.0));
}

void Renderer::DrawQuad(glm::vec2 position, float rotation, glm::vec2 scale, glm::vec4 color) const noexcept
{
	std::vector<Vertex> vertices {
			Vertex(glm::vec2(position.x + scale.x, position.y + scale.y), color, glm::vec2(1.0f, 0.0f)),
			Vertex(glm::vec2(position.x + scale.x, position.y - scale.y), color, glm::vec2(1.0f, 1.0f)),
			Vertex(glm::vec2(position.x - scale.x, position.y - scale.y), color, glm::vec2(0.0f, 1.0f)),
			Vertex(glm::vec2(position.x - scale.x, position.y + scale.y), color, glm::vec2(0.0f, 0.0f)),
	};

	RotateVertices(vertices, rotation);

	std::vector<uint32_t> indices {
			0, 1, 3,
			1, 2, 3
	};

	DrawVertices(vertices, indices, m_coreShader, m_whiteTexture);
}

void Renderer::DrawQuad(glm::vec2 position, float rotation, glm::vec2 scale, std::shared_ptr<Texture> texture) const noexcept
{
	std::vector<Vertex> vertices {
			Vertex(glm::vec2(position.x + scale.x, position.y + scale.y), glm::vec4(1.0f, 1.0f, 1.0f, 1.0f), glm::vec2(1.0f, 0.0f)),
			Vertex(glm::vec2(position.x + scale.x, position.y - scale.y), glm::vec4(1.0f, 1.0f, 1.0f, 1.0f), glm::vec2(1.0f, 1.0f)),
			Vertex(glm::vec2(position.x - scale.x, position.y - scale.y), glm::vec4(1.0f, 1.0f, 1.0f, 1.0f), glm::vec2(0.0f, 1.0f)),
			Vertex(glm::vec2(position.x - scale.x, position.y + scale.y), glm::vec4(1.0f, 1.0f, 1.0f, 1.0f), glm::vec2(0.0f, 0.0f)),
	};

	RotateVertices(vertices, rotation);

	std::vector<uint32_t> indices {
			0, 1, 3,
			1, 2, 3
	};

	DrawVertices(vertices, indices, m_coreShader, texture);
}

void Renderer::DrawVertices(std::vector<Vertex>& vertices, std::vector<uint32_t>& indices, std::shared_ptr<Shader> shader, std::shared_ptr<Texture> texture) const noexcept
{
	m_coreShader->Bind();

	VertexArray vertexArray;
	vertexArray.Bind();

	VertexBufferLayout layout;
	layout.AddElement<float>(2);
	layout.AddElement<float>(4);
	layout.AddElement<float>(2);

	VertexBuffer vertexBuffer(vertices.data(), vertices.size() * sizeof(Vertex), layout);

	IndexBuffer indexBuffer(indices.data(), indices.size());
	indexBuffer.Bind();

	texture->Bind();
	shader->SetUniform("u_Texture", texture->GetSlot());

	glDrawElements(GL_TRIANGLES, indices.size(), GL_UNSIGNED_INT, nullptr);

	texture->Unbind();
	indexBuffer.Unbind();
	vertexArray.Unbind();
	shader->Unbind();
}

void Renderer::RotateVertices(std::vector<Vertex>& vertices, float rotation) const noexcept
{
	for (Vertex& vertex : vertices)
	{
		vertex.m_position = glm::vec2(
				vertex.m_position.x * glm::cos(glm::radians(rotation)) + vertex.m_position.y * sin(glm::radians(rotation)),
				vertex.m_position.y * glm::cos(glm::radians(rotation)) - vertex.m_position.x * sin(glm::radians(rotation))
		);
	}
}

#ifndef POLTERGEIST_RENDERER_HPP
#define POLTERGEIST_RENDERER_HPP

#include <vector>
#include <glm/glm.hpp>
#include "Shader.hpp"
#include "Texture.hpp"
#include "Vertex.hpp"
#include "FrameBuffer.hpp"

class Renderer
{
private:
	std::shared_ptr<Shader> m_coreShader;
	std::shared_ptr<Texture> m_whiteTexture;
public:
	[[nodiscard]] static std::shared_ptr<Renderer> Create();

	void BeginRenderPass() const noexcept;
	void BeginRenderPass(FrameBuffer& frameBuffer) const noexcept;

	void EndRenderPass() const noexcept;

	void Clear(glm::vec3 color) const noexcept;
	void Clear(glm::vec4 color) const noexcept;

	void DrawQuad(glm::vec2 position, float rotation, glm::vec2 scale, glm::vec3 color) const noexcept;
	void DrawQuad(glm::vec2 position, float rotation, glm::vec2 scale, glm::vec4 color) const noexcept;
	void DrawQuad(glm::vec2 position, float rotation, glm::vec2 scale, std::shared_ptr<Texture> texture) const noexcept;

	void DrawVertices(std::vector<Vertex>& vertices, std::vector<uint32_t>& indices, std::shared_ptr<Shader> shader, std::shared_ptr<Texture> texture) const noexcept;
private:
	void RotateVertices(std::vector<Vertex>& vertices, float rotation) const noexcept;
};

#endif

#version 450
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aTexCoords;

uniform mat3 transform;
uniform mat4 projection;

out vec2 texCoords;

void main() {
    texCoords = aTexCoords;

    vec3 pos = transform * vec3(aPosition.xy, 1.0f);

    gl_Position = projection * vec4(pos.xy, 0.0f, 1.0f);
}
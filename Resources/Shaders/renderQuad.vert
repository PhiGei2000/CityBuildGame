#version 450
layout (location = 0) in vec2 position;

uniform mat3 transform;
uniform mat4 projection;

void main() {
    gl_Position = vec4((transform * vec3(position, 1.0f)).xy, 0.0f, 1.0f);
}

#version 450
layout (location = 0) in vec2 position;

uniform mat3 transform;
uniform mat4 projection;

out vec2 fragPosition;

void main() {
    vec3 pos = transform * vec3(position.xy, 1.0f);

    fragPosition = pos.xy;
    gl_Position = projection * vec4(pos.xy, 0.0f, 1.0f);
}
